using api.Dtos.Course;
using api.Models;

namespace api.Mappers
{
  public static class CourseMapper
  {
    public static CourseDto ToDto(this Course courseItem)
    {
      return new CourseDto
      {
        Id = courseItem.Id,
        Name = courseItem.Name,
        Description = courseItem.Description,
        ImageUrl = courseItem.ImageUrl,
        Schedule = courseItem.Schedule,
        Professor = courseItem.Professor
      };
    }

    public static Course ToCourseFromCreateDto(this CreateCourseRequestDto createCourseRequest)
    {
      return new Course
      {
        Name = createCourseRequest.Name,
        Description = createCourseRequest.Description,
        Schedule = createCourseRequest.Schedule,
        Professor = createCourseRequest.Professor
      };
    }
  }
}