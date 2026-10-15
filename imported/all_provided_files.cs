/* Imported user-provided files (concatenated). Original metadata lines preserved as comments. */

// File: namespace StyleAI.Core.DTOs.AuthLogin.cs
namespace StyleAI.Core.DTOs.Auth.Login;

public record LoginDto(string Email, string Password);

// File: namespace StyleAI.Core.DTOs.AuthRegisterDto.cs
namespace StyleAI.Core.DTOs.Auth.RegisterDto;

public record RegisterDto(string Email, string Password);

// File: namespace StyleAI.Core.DTOs.AuthRegistrationfull.cs
namespace StyleAI.Core.DTOs.Auth.Registrationfull;

public record AuthResponseDto(string Token, string Email, string Role, DateTime ExpiresAt);

// File: namespace StyleAI.Core.DTOs.StyleAnalytickspy.cs
namespace StyleAI.Core.DTOs.Style.Analytickspy;

public record StyleAnalysisDto(
    int Id,
    string PredictedStyleId,
    string PredictedStyleName,
    float Confidence,
    DateTime CreatedAt
);

// File: namespace StyleAI.Core.DTOs.StyleonfirmWithPhotoDtoC.cs
namespace StyleAI.Core.DTOs.Style.onfirmWithPhotoDto;

public record onfirmWithPhotoDtoC(
    string TestStyle,
    string AiStyleId,
    string AiStyleName,
    float Confidence,
    bool Confirmed,
    StyleRecommendationDto Recommendation
);

// File: namespace StyleAI.Core.DTOs.StyleRecomandadionpy.cs
namespace StyleAI.Core.DTOs.Style.Recomandadionpy;

public record StyleRecommendationDto(
    string Id,
    string Name,
    string Summary,
    string[] Palette,
    string[] Clothing,
    string[] Hobbies,
    string[] Directions
);

// File: namespace StyleAI.Core.DTOs.StyleStyleAnalysisDto.cs
namespace StyleAI.Core.DTOs.Style.StyleAnalysisDto;

public record StyleAnalysisDto(
    int Id,
    string PredictedStyleId,
    string PredictedStyleName,
    float Confidence,
    DateTime CreatedAt
);
//Айпишник возращает псле анализа фото

// File: namespace StyleAI.Core.DTOs.Tests.create.cs
namespace StyleAI.Core.DTOs.Tests.createtestdto;

public record CreateTestDto(
    string Title,
    string Description,
    List<CreateQuestionDto> Questions
);

// File: namespace StyleAI.Core.DTOs.Tests.createqwestions.cs
namespace StyleAI.Core.DTOs.Tests.Createquestionandanswer;

public record CreateQuestionDto(
    string Text,
    int OrderIndex,
    List<CreateAnswerOptionDto> Options
);

public record CreateAnswerOptionDto(
    string Text,
    Dictionary<string, int> StyleWeights
);

// File: namespace StyleAI.Core.DTOs.Tests.cs
namespace StyleAI.Core.DTOs.Tests.TestResultDt;

public record TestResultDto(
    int Id,
    int TestId,
    int Score,
    string PredictedStyleId,
    DateTime CompletedAt,
    bool? AiConfirmed
);

// File: namespace StyleAI.Core.DTOs.Tests.sumbut.cs
namespace StyleAI.Core.DTOs.Tests.sumbut;

public record SubmitAnswersDto(List<AnswerDto> Answers);
public record AnswerDto(int QuestionId, int OptionId);

// File: namespace StyleAI.Core.Entities.cs
namespace StyleAI.Core.Entities.Test;
//контейнер вопросов 
public class Test
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CreatedByUserId { get; set; }
    public User CreatedBy { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<TestResult> Results { get; set; } = new List<TestResult>();
}
//Questions — один тест  много вопросов,Results — один тест  много попыток прохождения (от разных учеников). удалили тест следовательно удалились его вопросы

// File: namespace StyleAI.Core.EntitiesQwestions.cs
namespace StyleAI.Core.Entities.Question;

public class Question
{
    public int Id { get; set; }
    public int TestId { get; set; }
    public Test Test { get; set; } = null;
    public string Text { get; set; } = string.Empty;
    public int OrderIndex { get; set; }

    public ICollection<AnswerOption> Options { get; set; } = new List<AnswerOption>();
}
//последовательность ответов

// File: namespace StyleAI.Core.Entitiesstyleanalyticks.cs
namespace StyleAI.Core.Entities.StyleAnalysis;

public class StyleAnalysis
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public string PredictedStyleId { get; set; } = "";
    public string PredictedStyleName { get; set; } = "";
    public float Confidence { get; set; }              // 0..1
    public string FeaturesJson { get; set; } = "[]";   // 15 признаков из features.py
    public string AllProbsJson { get; set; } = "{}";   // распределение по всем 8 стилям
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

// File: namespace StyleAI.Core.EntitiesTestResult.cs
namespace StyleAI.Core.Entities.TestResult;

public class TestResult
{
    public int Id { get; set; }
    public int TestId { get; set; }
    public Test Test { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int Score { get; set; }                       // % правильных (если тест с ключом)
    public string PredictedStyleId { get; set; } = "";   // стиль из ответов теста
    public string AnswersJson { get; set; } = "[]";      // сырые ответы пользователя
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

    //  Связь с AI на python
    public int? StyleAnalysisId { get; set; }            // null, пока фото не загружено
    public StyleAnalysis? StyleAnalysis { get; set; }
    public bool? AiConfirmed { get; set; }               // совпало ли фото с тестом
}

// File: namespace StyleAI.CoreIPasswordHasher.cs
namespace StyleAI.Core.Interfaces;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}

// File: namespace StyleAI.CoreIPythonMlClient.cs
namespace StyleAI.Core.Interfaces;

public interface IPythonMlClient
{
    Task<MlPrediction> PredictAsync(Stream image, string fileName, CancellationToken ct);
}

public record MlPrediction(
    string StyleId,
    string StyleName,
    float Confidence,
    float[] Features,
    Dictionary<string, float> AllProbs
);

// File: Services.cs
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StyleAI.Core.Entities;
using StyleAI.Core.Interfaces;

namespace StyleAI.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config) => _config = config;

    public (string Token, DateTime ExpiresAt) GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddHours(2);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), expires);
    }
}

// File: using MicrosoftPersistence.cs
using Microsoft.EntityFrameworkCore;
using StyleAI.Core.Entities;

namespace StyleAI.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Test> Tests => Set<Test>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<AnswerOption> AnswerOptions => Set<AnswerOption>();
    public DbSet<TestResult> TestResults => Set<TestResult>();
    public DbSet<StyleAnalysis> StyleAnalyses => Set<StyleAnalysis>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        // User
        b.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Test -> Questions (каскад: удалили тест — ушли вопросы)
        b.Entity<Test>()
            .HasMany(t => t.Questions)
            .WithOne(q => q.Test)
            .HasForeignKey(q => q.TestId)
            .OnDelete(DeleteBehavior.Cascade);

        // Question -> Options
        b.Entity<Question>()
            .HasMany(q => q.Options)
            .WithOne(o => o.Question)
            .HasForeignKey(o => o.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        // User -> CreatedTests (Restrict: нельзя удалить менеджера, если у него тесты)
        b.Entity<Test>()
            .HasOne(t => t.CreatedBy)
            .WithMany(u => u.CreatedTests)
            .HasForeignKey(t => t.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // TestResult -> Test (Restrict: не удаляем тест, если есть результаты)
        b.Entity<TestResult>()
            .HasOne(r => r.Test)
            .WithMany(t => t.Results)
            .HasForeignKey(r => r.TestId)
            .OnDelete(DeleteBehavior.Restrict);

        // TestResult -> User
        b.Entity<TestResult>()
            .HasOne(r => r.User)
            .WithMany(u => u.TestResults)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        //  TestResult <-> StyleAnalysis (один-к-одному, опционально)
        b.Entity<TestResult>()
            .HasOne(r => r.StyleAnalysis)
            .WithMany()
            .HasForeignKey(r => r.StyleAnalysisId)
            .OnDelete(DeleteBehavior.SetNull);

        // StyleAnalysis -> User
        b.Entity<StyleAnalysis>()
            .HasOne(a => a.User)
            .WithMany(u => u.StyleAnalyses)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

// File: using StyleAI.Core.cs
using StyleAI.Core.Enums;

namespace StyleAI.Core.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Student;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Навигационные свойства в случае ответа на защите
    public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    public ICollection<StyleAnalysis> StyleAnalyses { get; set; } = new List<StyleAnalysis>();
    public ICollection<Test> CreatedTests { get; set; } = new List<Test>();

}
//хранение эмайла пароля 
//полиморфизм

// File: using StyleAI.Core.DTOsInterfaces.cs
using StyleAI.Core.DTOs.Style;

namespace StyleAI.Core.Interfaces;

public interface IStyleService
{
    Task<StyleAnalysisDto> SaveAnalysisAsync(int userId, MlPrediction prediction, CancellationToken ct);
    Task<List<StyleAnalysisDto>> GetHistoryAsync(int userId, CancellationToken ct);
    StyleRecommendationDto GetRecommendation(string styleId);
}

// File: using StyleAI.Core.DTOsIStyleCatalog.cs
using StyleAI.Core.DTOs.Style;

namespace StyleAI.Core.Data;

public interface IStyleCatalog
{
    StyleRecommendationDto? GetById(string id);
    IReadOnlyList<StyleRecommendationDto> All { get; }
}

// File: using StyleAI.Core.DTOsITestService.cs
using StyleAI.Core.DTOs.Style;
using StyleAI.Core.DTOs.Tests;

namespace StyleAI.Core.Interfaces;

public interface ITestService
{
    Task<int> CreateTestAsync(int managerId, CreateTestDto dto, CancellationToken ct);
    Task<List<TestResultDto>> GetUserResultsAsync(int userId, CancellationToken ct);
    Task<TestResultDto> SubmitAsync(int testId, int userId, SubmitAnswersDto dto, CancellationToken ct);
    Task<ConfirmWithPhotoDto> ConfirmWithPhotoAsync(int resultId, int userId, MlPrediction prediction, CancellationToken ct);
}

// File: using StyleAI.Core.Entities.cs
using StyleAI.Core.Entities.Interfaces;

namespace StyleAI.Core.Interfaces;

public interface IJwtService
{
    (string Token, DateTime ExpiresAt) GenerateToken(User user);
}

// File: using StyleAI.CoreServices.cs
using StyleAI.Core.Interfaces;

namespace StyleAI.Infrastructure.Services;

public class BcryptPasswordHasher : IPasswordHasher
{
    public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);
    public bool Verify(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
}

// File: using System.IdentityModel.Tokens.cs
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StyleAI.Core.Entities;
using StyleAI.Core.Interfaces;

namespace StyleAI.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config) => _config = config;

    public (string Token, DateTime ExpiresAt) GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddHours(2);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), expires);
    }
}

// File: using System.Net.Http.cs
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using StyleAI.Core.Interfaces;

namespace StyleAI.Infrastructure.Services;

public class PythonMlClient : IPythonMlClient
{
    private readonly HttpClient _http;
    public PythonMlClient(HttpClient http) => _http = http;

    public async Task<MlPrediction> PredictAsync(Stream image, string fileName, CancellationToken ct)
    {
        using var content = new MultipartFormDataContent();
        var streamContent = new StreamContent(image);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        content.Add(streamContent, "file", fileName);

        var response = await _http.PostAsync("/predict", content, ct);
        response.EnsureSuccessStatusCode();

        var raw = await response.Content.ReadFromJsonAsync<RawPrediction>(cancellationToken: ct)
                  ?? throw new InvalidOperationException("Empty ML response");

        return new MlPrediction(
            raw.StyleId,
            raw.StyleName,
            raw.Confidence,
            raw.Features,
            raw.AllProbs
        );
    }

    private record RawPrediction(
        [property: JsonPropertyName("style_id")] string StyleId,
        [property: JsonPropertyName("style_name")] string StyleName,
        [property: JsonPropertyName("confidence")] float Confidence,
        [property: JsonPropertyName("features")] float[] Features,
        [property: JsonPropertyName("all_probs")] Dictionary<string, float> AllProbs
    );
}

// File: using System.Text.cs
using System.Text.Json;
using StyleAI.Core.Data;
using StyleAI.Core.DTOs.Style;

namespace StyleAI.Infrastructure.Data;

public class StyleCatalog : IStyleCatalog
{
    private readonly Dictionary<string, StyleRecommendationDto> _byId;

    public StyleCatalog(string stylesJsonPath)
    {
        var json = File.ReadAllText(stylesJsonPath);
        var styles = JsonSerializer.Deserialize<List<StyleRecommendationDto>>(json)
                     ?? new List<StyleRecommendationDto>();
        _byId = styles.ToDictionary(s => s.Id);
    }

    public StyleRecommendationDto? GetById(string id)
        => _byId.TryGetValue(id, out var s) ? s : null;

    public IReadOnlyList<StyleRecommendationDto> All => _byId.Values.ToList();
}

// File: using System.Textserv.cs
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

// File: using System.Textservises.cs
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
