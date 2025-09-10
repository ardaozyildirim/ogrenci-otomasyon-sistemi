using StudentManagementSystem.Domain.Events.Grade;

namespace StudentManagementSystem.Domain.Entities;

public class Grade : BaseEntity
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public decimal Score { get; set; }
    public string? Comment { get; set; }
    public DateTime GradeDate { get; set; } = DateTime.UtcNow;
    public string? GradeType { get; set; } // Midterm, Final, Assignment, etc.
    
    public Student Student { get; set; } = null!;
    public Course Course { get; set; } = null!;

    public static Grade Create(int studentId, int courseId, decimal score, string? gradeType = null, string? comment = null)
    {
        if (score < 0 || score > 100)
            throw new ArgumentException("Score must be between 0 and 100", nameof(score));

        var grade = new Grade
        {
            StudentId = studentId,
            CourseId = courseId,
            Score = score,
            GradeType = gradeType,
            Comment = comment,
            GradeDate = DateTime.UtcNow
        };

        grade.AddDomainEvent(new GradeAssignedEvent(grade, "", ""));
        
        return grade;
    }

    public void UpdateScore(decimal newScore, string? comment = null)
    {
        if (newScore < 0 || newScore > 100)
            throw new ArgumentException("Score must be between 0 and 100", nameof(newScore));

        Score = newScore;
        Comment = comment;
        UpdatedAt = DateTime.UtcNow;
    }

    public string GetLetterGrade()
    {
        return Score switch
        {
            >= 90 => "A",
            >= 80 => "B",
            >= 70 => "C",
            >= 60 => "D",
            _ => "F"
        };
    }

    public bool IsPassingGrade()
    {
        return Score >= 60;
    }
}
