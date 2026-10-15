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