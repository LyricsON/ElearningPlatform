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

        // Configure cascade delete behavior to avoid SQL Server multiple cascade paths
        // Enrollment -> User should not cascade (conflict with Course -> User -> Enrollment)
        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // QuizResult -> User should not cascade (conflict with Quiz -> Course -> User)
        modelBuilder.Entity<QuizResult>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

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
            .ToTable(t => t.HasCheckConstraint("CK_QuizQuestions_CorrectAnswer",
                "CorrectAnswer IN ('A','B','C','D')"));
    }
}
