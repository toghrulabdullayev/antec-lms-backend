using AntecLMS.Domain.Common;
using AntecLMS.Domain.Enums;

namespace AntecLMS.Domain.Entities;

public class User : BaseEntity
{
  public string Name { get; private set; } = default!;
  public string Surname { get; private set; } = default!;
  public string Email { get; private set; } = default!;
  public string Password { get; private set; } = default!;
  public string? Phone { get; private set; }
  public UserRole Role { get; private set; }
  public UserStatus Status { get; private set; }

  // Nav
  public Teacher? TeacherProfile { get; set; }
  public Student? StudentProfile { get; set; }

  protected User() { }

  public static User Create(
    string name,
    string surname,
    string email,
    string hashedPassword,
    UserRole role,
    string? phone = null,
    UserStatus status = UserStatus.Active
  )
  {
    return new User
    {
      Name = name,
      Surname = surname,
      Email = email,
      Password = hashedPassword,
      Role = role,
      Phone = phone,
      Status = status,
    };
  }

  public void Update(string name, string surname, string? phone, UserStatus status)
  {
    Name = name;
    Surname = surname;
    Phone = phone;
    Status = status;
    MarkUpdated();
  }

  public void ChangeStatus(UserStatus status)
  {
    Status = status;
    MarkUpdated();
  }

  public void ChangePassword(string hashedPassword)
  {
    Password = hashedPassword;
    MarkUpdated();
  }
}
