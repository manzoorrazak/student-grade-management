using Microsoft.EntityFrameworkCore;
using StudentGrades.Api.Data;
using StudentGrades.Api.Models;

namespace StudentGrades.Api.Endpoints;

public static class SubjectEndpoints
{
    public static void MapSubjectEndpoints(this WebApplication app)
    {
        // Get all subjects
        app.MapGet("/api/subjects", async (AppDbContext db) =>
        {
            return await db.Subjects.ToListAsync();
        });

        // Get subject by id
        app.MapGet("/api/subjects/{id}", async (int id, AppDbContext db) =>
        {
            var subject = await db.Subjects.FindAsync(id);

            return subject is null ? Results.NotFound() : Results.Ok(subject);
        });

        // Add subject
        app.MapPost("/api/subjects", async (Subject subject, AppDbContext db) =>
        {
            db.Subjects.Add(subject);
            await db.SaveChangesAsync();

            return Results.Created($"/api/subjects/{subject.SubjectId}", subject);
        });

        // Update subject
        app.MapPut("/api/subjects/{id}", async (int id, Subject input, AppDbContext db) =>
        {
            var subject = await db.Subjects.FindAsync(id);

            if (subject is null)
                return Results.NotFound();

            subject.SubjectName = input.SubjectName;

            await db.SaveChangesAsync();

            return Results.Ok(subject);
        });

        // Delete subject
        app.MapDelete("/api/subjects/{id}", async (int id, AppDbContext db) =>
        {
            var subject = await db.Subjects.FindAsync(id);

            if (subject is null)
                return Results.NotFound();

            db.Subjects.Remove(subject);

            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
