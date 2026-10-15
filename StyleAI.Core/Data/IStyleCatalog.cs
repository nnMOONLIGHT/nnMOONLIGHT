using StyleAI.Core.DTOs.Style;

namespace StyleAI.Core.Data;

public interface IStyleCatalog
{
    StyleRecommendationDto? GetById(string id);
    IReadOnlyList<StyleRecommendationDto> All { get; }
}