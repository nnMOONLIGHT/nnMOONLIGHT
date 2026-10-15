using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StyleAI.Core.Data;
using StyleAI.Core.DTOs.Style;
using StyleAI.Core.DTOs.Tests;
using StyleAI.Core.Entities;
using StyleAI.Core.Interfaces;
using StyleAI.Infrastructure.Persistence;

namespace StyleAI.Infrastructure.Services;

public class TestService : ITestService
{
    private readonly AppDbContext _db;
    private readonly IStyleCatalog _catalog;

    public TestService(AppDbContext db, IStyleCatalog catalog)
    {
        _db = db; _catalog = catalog;
    }

    public async Task<int> CreateTestAsync(int managerId, CreateTestDto dto, CancellationToken ct)
    {
        var test = new Test
        {
            Title = dto.Title,
            Description = dto.Description,
            CreatedByUserId = managerId,
            Questions = dto.Questions.Select(q => new Question
            {
                Text = q.Text,
                OrderIndex = q.OrderIndex,
                Options = q.Options.Select(o => new AnswerOption
                {
                    Text = o.Text,
                    StyleWeightsJson = JsonSerializer.Serialize(o.StyleWeights)
                }).ToList()
            }).ToList()
        };
        _db.Tests.Add(test);
        await _db.SaveChangesAsync(ct);
        return test.Id;
    }

    public async Task<List<TestResultDto>> GetUserResultsAsync(int userId, CancellationToken ct)
    {
        return await _db.TestResults
            .AsNoTracking()
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CompletedAt)
            .Select(r => new TestResultDto(
                r.Id, r.TestId, r.Score, r.PredictedStyleId,
                r.CompletedAt, r.AiConfirmed))
            .ToListAsync(ct);
    }

    public async Task<TestResultDto> SubmitAsync(int testId, int userId, SubmitAnswersDto dto, CancellationToken ct)
    {
        // Достаём все варианты, которые выбрал пользователь
        var optionIds = dto.Answers.Select(a => a.OptionId).ToList();
        var options = await _db.AnswerOptions
            .Where(o => optionIds.Contains(o.Id))
            .ToListAsync(ct);

        // Суммируем веса по стилям
        var totals = new Dictionary<string, int>();
        foreach (var opt in options)
        {
            var w = JsonSerializer.Deserialize<Dictionary<string, int>>(opt.StyleWeightsJson)
                    ?? new Dictionary<string, int>();
            foreach (var kv in w)
                totals[kv.Key] = totals.GetValueOrDefault(kv.Key) + kv.Value;
        }
        var predicted = totals.OrderByDescending(kv => kv.Value).FirstOrDefault().Key ?? "minimal";

        var result = new TestResult
        {
            TestId = testId,
            UserId = userId,
            PredictedStyleId = predicted,
            AnswersJson = JsonSerializer.Serialize(dto.Answers),
            Score = 100,
            CompletedAt = DateTime.UtcNow
        };
        _db.TestResults.Add(result);
        await _db.SaveChangesAsync(ct);

        return new TestResultDto(result.Id, testId, result.Score, predicted, result.CompletedAt, null);
    }

    public async Task<ConfirmWithPhotoDto> ConfirmWithPhotoAsync(
        int resultId, int userId, MlPrediction prediction, CancellationToken ct)
    {
        var result = await _db.TestResults
            .FirstOrDefaultAsync(r => r.Id == resultId && r.UserId == userId, ct)
            ?? throw new KeyNotFoundException("Результат теста не найден");

        var analysis = new StyleAnalysis
        {
            UserId = userId,
            PredictedStyleId = prediction.StyleId,
            PredictedStyleName = prediction.StyleName,
            Confidence = prediction.Confidence,
            FeaturesJson = JsonSerializer.Serialize(prediction.Features),
            AllProbsJson = JsonSerializer.Serialize(prediction.AllProbs),
            CreatedAt = DateTime.UtcNow
        };
        _db.StyleAnalyses.Add(analysis);
        await _db.SaveChangesAsync(ct);

        result.StyleAnalysisId = analysis.Id;
        result.AiConfirmed = result.PredictedStyleId == prediction.StyleId;
        await _db.SaveChangesAsync(ct);

        var reco = _catalog.GetById(prediction.StyleId)
                   ?? throw new KeyNotFoundException("Стиль не найден");

        return new ConfirmWithPhotoDto(
            result.PredictedStyleId,
            prediction.StyleId,
            prediction.StyleName,
            prediction.Confidence,
            result.AiConfirmed ?? false,
            reco
        );
    }
}