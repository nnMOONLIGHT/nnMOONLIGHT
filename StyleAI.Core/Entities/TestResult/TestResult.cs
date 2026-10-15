namespace StyleAI.Core.Entities.TestResult;

public class TestResult
{
    public int Id { get; set; }
    public int TestId { get; set; }
    public Test Test { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int Score { get; set; }                       // % правильных (если тест с ключом)
    public string PredictedStyleId { get; set; } = "";   // стиль из ответов теста
    public string AnswersJson { get; set; } = "[]";      // сырые ответы пользователя
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

    //  Связь с AI на python
    public int? StyleAnalysisId { get; set; }            // null, пока фото не загружено
    public StyleAnalysis? StyleAnalysis { get; set; }
    public bool? AiConfirmed { get; set; }               // совпало ли фото с тестом
}