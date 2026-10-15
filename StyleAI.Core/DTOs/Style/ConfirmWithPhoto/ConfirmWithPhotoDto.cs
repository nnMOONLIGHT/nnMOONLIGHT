namespace StyleAI.Core.DTOs.Style.onfirmWithPhotoDto;

public record onfirmWithPhotoDtoC(
    string TestStyle,
    string AiStyleId,
    string AiStyleName,
    float Confidence,
    bool Confirmed,
    StyleRecommendationDto Recommendation
);