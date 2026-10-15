namespace StyleAI.Core.DTOs.Tests.createtestdto;

public record CreateTestDto(
    string Title,
    string Description,
    List<CreateQuestionDto> Questions
);