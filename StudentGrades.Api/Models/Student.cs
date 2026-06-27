namespace StudentGrades.Api.Models;

public class Student
{
    public int StudentId { get; set; }

    public string StudentName { get; set; } = string.Empty;

    public int SubjectId { get; set; }

    public Subject? Subject { get; set; }

    public decimal Grade { get; set; }

    public string Remarks { get; set; } = string.Empty;
}
