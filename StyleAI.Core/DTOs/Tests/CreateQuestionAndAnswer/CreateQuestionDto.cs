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