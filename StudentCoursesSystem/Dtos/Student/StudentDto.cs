using api.Dtos.Course;

namespace api.Dtos.Student
{
  public class StudentDto
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public int CourseId { get; set; }
    public CourseBasicDto Course { get; set; }
  }
}