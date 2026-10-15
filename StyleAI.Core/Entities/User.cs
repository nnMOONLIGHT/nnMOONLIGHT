using StyleAI.Core.Enums;

namespace StyleAI.Core.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Student;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Навигационные свойства в случае ответа на защите
    public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    public ICollection<StyleAnalysis> StyleAnalyses { get; set; } = new List<StyleAnalysis>();
    public ICollection<Test> CreatedTests { get; set; } = new List<Test>();

}
//хранение эмайла пароля 
//полиморфизм