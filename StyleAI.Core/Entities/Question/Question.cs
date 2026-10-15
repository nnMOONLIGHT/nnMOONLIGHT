namespace StyleAI.Core.Entities.Question;

public class Question
{
    public int Id { get; set; }
    public int TestId { get; set; }
    public Test Test { get; set; } = null;
    public string Text { get; set; } = string.Empty;
    public int OrderIndex { get; set; }

    public ICollection<AnswerOption> Options { get; set; } = new List<AnswerOption>();
}
//последовательность ответов