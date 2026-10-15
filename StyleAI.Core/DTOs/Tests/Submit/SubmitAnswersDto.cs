namespace StyleAI.Core.DTOs.Tests.sumbut;

public record SubmitAnswersDto(List<AnswerDto> Answers);
public record AnswerDto(int QuestionId, int OptionId);