using Microsoft.EntityFrameworkCore;
using StudentGrades.Api.Models;

namespace StudentGrades.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students => Set<Student>();

    public DbSet<Subject> Subjects => Set<Subject>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .HasKey(s => s.StudentId);

        modelBuilder.Entity<Subject>()
            .HasKey(s => s.SubjectId);

        modelBuilder.Entity<Student>()
            .HasOne(s => s.Subject)
            .WithMany(s => s.Students)
            .HasForeignKey(s => s.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
