using Microsoft.EntityFrameworkCore;
using StudentGrades.Api.Data;
using StudentGrades.Api.Models;

namespace StudentGrades.Api.Endpoints;

public static class StudentEndpoints
{
    public static void MapStudentEndpoints(this WebApplication app)
    {
        // Get all students
        app.MapGet("/api/students", async (
    string? search,
    string? remarks,
    AppDbContext db) =>
{
    var query = db.Students
        .Include(s => s.Subject)
        .AsQueryable();

    if (!string.IsNullOrWhiteSpace(search))
    {
        query = query.Where(s =>
            s.StudentName.Contains(search));
    }

    if (!string.IsNullOrWhiteSpace(remarks))
    {
        query = query.Where(s =>
            s.Remarks == remarks);
    }

    var students = await query
        .Select(s => new
        {
            s.StudentId,
            s.StudentName,
            s.SubjectId,
            SubjectName = s.Subject!.SubjectName,
            s.Grade,
            s.Remarks
        })
        .ToListAsync();

    return Results.Ok(students);
});

        // Get student by Id
        app.MapGet("/api/students/{id}", async (int id, AppDbContext db) =>
        {
            var student = await db.Students
                .Include(s => s.Subject)
                .Where(s => s.StudentId == id)
                .Select(s => new
                {
                    s.StudentId,
                    s.StudentName,
                    s.SubjectId,
                    SubjectName = s.Subject!.SubjectName,
                    s.Grade,
                    s.Remarks
                })
                .FirstOrDefaultAsync();

            return student is null
                ? Results.NotFound()
                : Results.Ok(student);
        });

        // Add student
        app.MapPost("/api/students", async (Student student, AppDbContext db) =>
        {
            student.Remarks = student.Grade >= 75 ? "PASS" : "FAIL";

            db.Students.Add(student);

            await db.SaveChangesAsync();

            return Results.Created($"/api/students/{student.StudentId}", student);
        });

        // Update student
        app.MapPut("/api/students/{id}", async (int id, Student input, AppDbContext db) =>
        {
            var student = await db.Students.FindAsync(id);

            if (student is null)
                return Results.NotFound();

            student.StudentName = input.StudentName;
            student.SubjectId = input.SubjectId;
            student.Grade = input.Grade;
            student.Remarks = input.Grade >= 75 ? "PASS" : "FAIL";

            await db.SaveChangesAsync();

            return Results.Ok(student);
        });

        // Delete student
        app.MapDelete("/api/students/{id}", async (int id, AppDbContext db) =>
        {
            var student = await db.Students.FindAsync(id);

            if (student is null)
                return Results.NotFound();

            db.Students.Remove(student);

            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
