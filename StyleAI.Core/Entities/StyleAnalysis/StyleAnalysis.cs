namespace StyleAI.Core.Entities.StyleAnalysis;

public class StyleAnalysis
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public string PredictedStyleId { get; set; } = "";
    public string PredictedStyleName { get; set; } = "";
    public float Confidence { get; set; }              // 0..1
    public string FeaturesJson { get; set; } = "[]";   // 15 признаков из features.py
    public string AllProbsJson { get; set; } = "{}";   // распределение по всем 8 стилям
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}