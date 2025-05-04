using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Course;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
  [Route("api/course")]
  [ApiController]
  public class CourseController : ControllerBase
  {
    private readonly ApplicationDBContext _context;
    private readonly string _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedImages");
    public CourseController(ApplicationDBContext context)
    {
      _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var courses = await _context.Courses
        .Include(c => c.Students)  
        .ToListAsync();

      var coursesDto = courses.Select(course => 
      {
        var courseDto = course.ToDto();
        courseDto.Students = course.Students?.Select(s => s.ToBasicDto()).ToList(); // Only basic info of each student, its Course isnt required
        return courseDto;
      });
      return Ok(coursesDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
      var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
      if (course == null)
      {
        return NotFound();
      }
      return Ok(course.ToDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateCourseRequestDto courseDto)
    {
      if (courseDto.File == null || courseDto.File.Length == 0)
        return BadRequest("No file uploaded.");

      var courseModel = courseDto.ToCourseFromCreateDto();
      await _context.Courses.AddAsync(courseModel);
      await _context.SaveChangesAsync();

      var fileName = courseModel.Id.ToString() + Path.GetExtension(courseDto.File.FileName);
      var filePath = Path.Combine(_imagePath, fileName);

      using (var stream = new FileStream(filePath, FileMode.Create))
      {
        await courseDto.File.CopyToAsync(stream);
      }

      courseModel.ImageUrl = fileName;
      _context.Courses.Update(courseModel);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetById), new { id = courseModel.Id }, courseModel.ToDto());
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCourseRequestDto courseDto)
    {
      var courseModel = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
      if (courseModel == null)
      {
        return NotFound();
      }
      courseModel.Name = courseDto.Name;
      courseModel.Description = courseDto.Description;
      courseModel.Schedule = courseDto.Schedule;
      courseModel.Professor = courseDto.Professor;

      await _context.SaveChangesAsync();
/*
      // Send notification to all users subscribed to "course_notifications" topic
      await FirebaseHelper.SendPushNotificationToTopicAsync(
          topic: "course_notifications",
          title: "Course Updated!",
          body: $"The course \"{courseModel.Name}\" has been updated!"
      );
*/
      return Ok(courseModel.ToDto());
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
      var courseModel = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
      if (courseModel == null)
      {
        return NotFound();
      }
      _context.Courses.Remove(courseModel);

      await _context.SaveChangesAsync();

      return NoContent();
    }
  }
}