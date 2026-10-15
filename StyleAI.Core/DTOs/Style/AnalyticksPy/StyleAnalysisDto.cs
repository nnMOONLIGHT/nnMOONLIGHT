namespace StyleAI.Core.DTOs.Style.Analytickspy;

public record StyleAnalysisDto(
    int Id,
    string PredictedStyleId,
    string PredictedStyleName,
    float Confidence,
    DateTime CreatedAt
);