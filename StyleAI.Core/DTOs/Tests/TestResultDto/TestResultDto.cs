namespace StyleAI.Core.DTOs.Tests.TestResultDt;

public record TestResultDto(
    int Id,
    int TestId,
    int Score,
    string PredictedStyleId,
    DateTime CompletedAt,
    bool? AiConfirmed
);