namespace StudentGrades.Api.Models;

public class Subject
{
    public int SubjectId { get; set; }

    public string SubjectName { get; set; } = string.Empty;

    public ICollection<Student> Students { get; set; } = new List<Student>();
}
