using Microsoft.EntityFrameworkCore;
using StyleAI.Core.Entities;

namespace StyleAI.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Test> Tests => Set<Test>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<AnswerOption> AnswerOptions => Set<AnswerOption>();
    public DbSet<TestResult> TestResults => Set<TestResult>();
    public DbSet<StyleAnalysis> StyleAnalyses => Set<StyleAnalysis>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        // User
        b.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Test -> Questions (каскад: удалили тест — ушли вопросы)
        b.Entity<Test>()
            .HasMany(t => t.Questions)
            .WithOne(q => q.Test)
            .HasForeignKey(q => q.TestId)
            .OnDelete(DeleteBehavior.Cascade);

        // Question -> Options
        b.Entity<Question>()
            .HasMany(q => q.Options)
            .WithOne(o => o.Question)
            .HasForeignKey(o => o.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        // User -> CreatedTests (Restrict: нельзя удалить менеджера, если у него тесты)
        b.Entity<Test>()
            .HasOne(t => t.CreatedBy)
            .WithMany(u => u.CreatedTests)
            .HasForeignKey(t => t.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // TestResult -> Test (Restrict: не удаляем тест, если есть результаты)
        b.Entity<TestResult>()
            .HasOne(r => r.Test)
            .WithMany(t => t.Results)
            .HasForeignKey(r => r.TestId)
            .OnDelete(DeleteBehavior.Restrict);

        // TestResult -> User
        b.Entity<TestResult>()
            .HasOne(r => r.User)
            .WithMany(u => u.TestResults)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        //  TestResult <-> StyleAnalysis (один-к-одному, опционально)
        b.Entity<TestResult>()
            .HasOne(r => r.StyleAnalysis)
            .WithMany()
            .HasForeignKey(r => r.StyleAnalysisId)
            .OnDelete(DeleteBehavior.SetNull);

        // StyleAnalysis -> User
        b.Entity<StyleAnalysis>()
            .HasOne(a => a.User)
            .WithMany(u => u.StyleAnalyses)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}