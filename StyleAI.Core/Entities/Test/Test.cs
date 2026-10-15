namespace StyleAI.Core.Entities.Test;
//контейнер вопросов 
public class Test
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CreatedByUserId { get; set; }
    public User CreatedBy { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<TestResult> Results { get; set; } = new List<TestResult>();
}
//Questions — один тест  много вопросов,Results — один тест  много попыток прохождения (от разных учеников). удалили тест следовательно удалились его вопросы