using api.Dtos.Student;
using api.Models;

namespace api.Mappers
{
  public static class StudentMapper
  {
    public static StudentDto ToDto(this Student studentItem)
    {
      return new StudentDto
      {
        Id = studentItem.Id,
        Name = studentItem.Name,
        Email = studentItem.Email,
        Phone = studentItem.Phone,
        CourseId = studentItem.CourseId
      };
    }

    public static Student ToStudentFromCreateDto(this CreateStudentRequestDto createStudentRequest)
    {
      return new Student
      {
        Name = createStudentRequest.Name,
        Email = createStudentRequest.Email,
        Phone = createStudentRequest.Phone,
        CourseId = createStudentRequest.CourseId
      };
    }
  }
}