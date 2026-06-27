namespace StudentGrades.Api.DTOs;

public class StudentResponseDto
{
    public int StudentId { get; set; }

    public string StudentName { get; set; } = string.Empty;

    public string SubjectName { get; set; } = string.Empty;

    public decimal Grade { get; set; }

    public string Remarks { get; set; } = string.Empty;
}
