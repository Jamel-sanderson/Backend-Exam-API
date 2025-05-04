using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Student;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
  [Route("api/student")]
  [ApiController]
  public class StudentController : ControllerBase
  {
    private readonly ApplicationDBContext _context;
    public StudentController(ApplicationDBContext context)
    {
      _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var students = await _context.Students.ToListAsync();
      var studentsDto = students.Select(student => student.ToDto());
      return Ok(studentsDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
      var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
      if (student == null)
      {
        return NotFound();
      }
      return Ok(student.ToDto());
    }

    [HttpGet("course/{courseId}")]
    public async Task<IActionResult> GetByCourseId([FromRoute] int courseId)
    {
      var students = await _context.Students
        .Where(s => s.CourseId == courseId)
        .ToListAsync();
      
      var studentsDto = students.Select(student => student.ToDto());
      return Ok(studentsDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStudentRequestDto studentDto)
    {
      // Check if the course exists
      var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == studentDto.CourseId);
      if (course == null)
      {
        return BadRequest("Course does not exist");
      }

      var studentModel = studentDto.ToStudentFromCreateDto();
      await _context.Students.AddAsync(studentModel);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetById), new { id = studentModel.Id }, studentModel.ToDto());
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStudentRequestDto studentDto)
    {
      var studentModel = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
      if (studentModel == null)
      {
        return NotFound();
      }

      // If course ID is changing, verify the new course exists
      if (studentDto.CourseId != studentModel.CourseId)
      {
        var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == studentDto.CourseId);
        if (course == null)
        {
          return BadRequest("Course does not exist");
        }
      }

      studentModel.Name = studentDto.Name;
      studentModel.Email = studentDto.Email;
      studentModel.Phone = studentDto.Phone;
      studentModel.CourseId = studentDto.CourseId;

      await _context.SaveChangesAsync();

      return Ok(studentModel.ToDto());
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
      var studentModel = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
      if (studentModel == null)
      {
        return NotFound();
      }
      _context.Students.Remove(studentModel);

      await _context.SaveChangesAsync();

      return NoContent();
    }
  }
}