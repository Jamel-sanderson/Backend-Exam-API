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
      // 1. Getting all students and including their corresponding course
      var students = await _context.Students
        .Include(s => s.Course)  // Including Course object
        .ToListAsync();

      // 2. Mapping each student to DTO
      var studentsDto = students.Select(student => student.ToDto()); // Just Basic info
      
      // 3. Response
      return Ok(studentsDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
      // 1. Getting the specific student and including corresponding course
      var student = await _context.Students
        .Include(s => s.Course)  
        .FirstOrDefaultAsync(s => s.Id == id);
      
      // 2. If no student was found
      if (student == null)
      {
        return NotFound();
      }
      
      // 3. Response
      return Ok(student.ToDto());
    }

    [HttpGet("course/{courseId}")]
    public async Task<IActionResult> GetByCourseId([FromRoute] int courseId)
    {
      // 1. Getting all students that belong to the specified course
      var students = await _context.Students
        .Where(s => s.CourseId == courseId)
        .ToListAsync();
      
      // 2. Mapping each student to DTO
      var studentsDto = students.Select(student => student.ToDto());
      
      // 3. Response
      return Ok(studentsDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStudentRequestDto studentDto)
    {
      // 1. First its gonna be validated by DTO definition
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      // 2. Custom validation for unique Course name 
      if (await IsNameUniqueAsync(studentDto.Name) == false)
      {
        ModelState.AddModelError(nameof(studentDto.Name), StudentValidationConstants.NameUniqueErrorMessage);
        return BadRequest(ModelState);
      }

      // 3. Check if the course exists
      var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == studentDto.CourseId);
      if (course == null)
      {
        return BadRequest("Course does not exist");
      }

      // 4. Inserting on DB through model definition
      var studentModel = studentDto.ToStudentFromCreateDto();
      await _context.Students.AddAsync(studentModel);
      await _context.SaveChangesAsync();

      // 5. Response
      return CreatedAtAction(nameof(GetById), new { id = studentModel.Id }, studentModel.ToDto());
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStudentRequestDto studentDto)
    {
      // 1. First its gonna be validated by DTO definition
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      // 2. Custom validation for unique Course name 
      if (await IsNameUniqueAsync(studentDto.Name, id) == false)
      {
        ModelState.AddModelError(nameof(studentDto.Name), StudentValidationConstants.NameUniqueErrorMessage);
        return BadRequest(ModelState);
      }
      
      // 3. Recovering specific db row of the student and including its course
      var studentModel = await _context.Students
        .Include(s => s.Course)  
        .FirstOrDefaultAsync(s => s.Id == id);
      
      // 4. If no student was found
      if (studentModel == null)
      {
        return NotFound();
      }

      // 5. If course ID is changing, verify the new course exists
      if (studentDto.CourseId != studentModel.CourseId)
      {
        var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == studentDto.CourseId);
        if (course == null)
        {
          return BadRequest("Course does not exist");
        }
      }

      // 6. Updating student attributes
      studentModel.Name = studentDto.Name;
      studentModel.Email = studentDto.Email;
      studentModel.Phone = studentDto.Phone;
      studentModel.CourseId = studentDto.CourseId;
      await _context.SaveChangesAsync();

      // 7. Response
      return Ok(studentModel.ToDto());
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
      // 1. Recovering specific db row
      var studentModel = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
      
      // 2. If no student was found
      if (studentModel == null)
      {
        return NotFound();
      }
      
      // 3. Deleting from db
      _context.Students.Remove(studentModel);
      await _context.SaveChangesAsync();

      // 4. Response
      return NoContent();
    }

    // Special service: verify unique Student name
    [NonAction]
    private async Task<bool> IsNameUniqueAsync(string name, int? excludeId = null)
    {
        // 1. Search students with the same name
        var query = _context.Courses.Where(s => s.Name == name);
        
        // 2. We need to exclude the own student from the check (if it's an update)
        if (excludeId.HasValue)
        {
          query = query.Where(ss => s.Id != excludeId.Value);
        }
        
        // 3. Executing the query
        return !await query.AnyAsync();
    }
  }
}