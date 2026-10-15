using StyleAI.Core.DTOs.Style;

namespace StyleAI.Core.Interfaces;

public interface IStyleService
{
    Task<StyleAnalysisDto> SaveAnalysisAsync(int userId, MlPrediction prediction, CancellationToken ct);
    Task<List<StyleAnalysisDto>> GetHistoryAsync(int userId, CancellationToken ct);
    StyleRecommendationDto GetRecommendation(string styleId);
}