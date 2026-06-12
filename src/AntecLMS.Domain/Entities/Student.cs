using AntecLMS.Domain.Common;
using AntecLMS.Domain.Enums;

namespace AntecLMS.Domain.Entities;

public class Student : BaseEntity
{
  public int UserId { get; private set; }
  public DateOnly? BirthDate { get; private set; }
  public string? Note { get; private set; }
  public UserStatus Status { get; private set; }

  // Nav
  public User User { get; set; } = default!;
  public ICollection<GroupStudent> GroupStudents { get; set; } = new List<GroupStudent>();
  public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
  public ICollection<Grade> Grades { get; set; } = new List<Grade>();

  protected Student() { }

  public static Student Create(int userId, DateOnly? birthDate, string? note, UserStatus status) =>
    new()
    {
      UserId = userId,
      BirthDate = birthDate,
      Note = note,
      Status = status,
    };

  public void Update(string? note, UserStatus status, DateOnly? birthDate = null)
  {
    Note = note;
    Status = status;
    BirthDate = birthDate ?? BirthDate;
    MarkUpdated();
  }
}
