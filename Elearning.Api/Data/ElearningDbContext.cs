using Elearning.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Elearning.Api.Data;

public class ElearningDbContext : DbContext
{
    public ElearningDbContext(DbContextOptions<ElearningDbContext> options)
        : base(options)
    {
    }

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<CourseCategory> CourseCategories => Set<CourseCategory>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Quiz> Quizzes => Set<Quiz>();
    public DbSet<QuizQuestion> QuizQuestions => Set<QuizQuestion>();
    public DbSet<QuizResult> QuizResults => Set<QuizResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // prevent same student enrolling twice in same course
        modelBuilder.Entity<Enrollment>()
            .HasIndex(e => new { e.UserId, e.CourseId })
            .IsUnique();

        // prevent same user result twice for same quiz
        modelBuilder.Entity<QuizResult>()
            .HasIndex(r => new { r.UserId, r.QuizId })
            .IsUnique();

        // Check constraint for CorrectAnswer
        modelBuilder.Entity<QuizQuestion>()
            .HasCheckConstraint("CK_QuizQuestions_CorrectAnswer",
                "CorrectAnswer IN ('A','B','C','D')");
    }
}
