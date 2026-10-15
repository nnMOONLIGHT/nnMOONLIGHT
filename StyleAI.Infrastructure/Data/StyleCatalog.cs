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