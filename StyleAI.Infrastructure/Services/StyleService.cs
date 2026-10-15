using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StyleAI.Core.Data;
using StyleAI.Core.DTOs.Style;
using StyleAI.Core.Entities;
using StyleAI.Core.Interfaces;
using StyleAI.Infrastructure.Persistence;

namespace StyleAI.Infrastructure.Services;

public class StyleService : IStyleService
{
    private readonly AppDbContext _db;
    private readonly IStyleCatalog _catalog;

    public StyleService(AppDbContext db, IStyleCatalog catalog)
    {
        _db = db; _catalog = catalog;
    }

    public async Task<StyleAnalysisDto> SaveAnalysisAsync(int userId, MlPrediction p, CancellationToken ct)
    {
        var entity = new StyleAnalysis
        {
            UserId = userId,
            PredictedStyleId = p.StyleId,
            PredictedStyleName = p.StyleName,
            Confidence = p.Confidence,
            FeaturesJson = JsonSerializer.Serialize(p.Features),
            AllProbsJson = JsonSerializer.Serialize(p.AllProbs),
            CreatedAt = DateTime.UtcNow
        };
        _db.StyleAnalyses.Add(entity);
        await _db.SaveChangesAsync(ct);

        return new StyleAnalysisDto(
            entity.Id, entity.PredictedStyleId, entity.PredictedStyleName,
            entity.Confidence, entity.CreatedAt);
    }

    public async Task<List<StyleAnalysisDto>> GetHistoryAsync(int userId, CancellationToken ct)
    {
        return await _db.StyleAnalyses
            .AsNoTracking()
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new StyleAnalysisDto(
                a.Id, a.PredictedStyleId, a.PredictedStyleName,
                a.Confidence, a.CreatedAt))
            .ToListAsync(ct);
    }

    public StyleRecommendationDto GetRecommendation(string styleId)
        => _catalog.GetById(styleId)
           ?? throw new KeyNotFoundException($"Стиль {styleId} не найден");
}