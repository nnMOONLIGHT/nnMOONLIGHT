namespace StyleAI.Core.DTOs.Style.StyleAnalysisDto;

public record StyleAnalysisDto(
    int Id,
    string PredictedStyleId,
    string PredictedStyleName,
    float Confidence,
    DateTime CreatedAt
);
//Айпишник возращает псле анализа фото