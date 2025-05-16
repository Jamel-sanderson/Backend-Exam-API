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
      // 1. Including corresponding students to each course
      var courses = await _context.Courses
        .Include(c => c.Students)  
        .ToListAsync();

      // 2. Mapping each course to DTO
      var coursesDto = courses.Select(course => 
      {
        var courseDto = course.ToDto();
        courseDto.Students = course.Students?.Select(s => s.ToBasicDto()).ToList(); // Only basic info of each student, its Course isnt required
        return courseDto;
      });

      // 3. Response set
      return Ok(coursesDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
      // 1. Getting the specific course and including corresponding students 
      var course = await _context.Courses
        .Include(c => c.Students) 
        .FirstOrDefaultAsync(c => c.Id == id);

      // 2. If no course was found
      if (course == null)
      {
        return NotFound();
      }

      // 3. Mapping the course to DTO and also mapping its students
      var courseDto = course.ToDto();
      courseDto.Students = course.Students?.Select(s => s.ToBasicDto()).ToList(); // Only basic info of each student, its Course isnt required

      // 4. Response
      return Ok(courseDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateCourseRequestDto courseDto)
    {
      // 1. First its gonna be validated by DTO definition
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      // 2. Custom validation for unique Course name 
      if (await IsCourseNameUniqueAsync(courseDto.Name) == false)
      {
        ModelState.AddModelError(nameof(courseDto.Name), CourseValidationConstants.NameUniqueErrorMessage);
        return BadRequest(ModelState);
      }

      // 3. If not file was uploaded (shouldnt be happening because of previous validation)
      if (courseDto.File == null || courseDto.File.Length == 0)
        return BadRequest("No file uploaded.");

      // 4. Inserting on DB through model definition
      var courseModel = courseDto.ToCourseFromCreateDto();
      await _context.Courses.AddAsync(courseModel);
      await _context.SaveChangesAsync();

      // 5. Parsing file
      var fileName = courseModel.Id.ToString() + Path.GetExtension(courseDto.File.FileName);
      var filePath = Path.Combine(_imagePath, fileName);

      using (var stream = new FileStream(filePath, FileMode.Create))
      {
        await courseDto.File.CopyToAsync(stream);
      }

      // 6. Updating this attribute with the result of parsed file
      courseModel.ImageUrl = fileName;
      _context.Courses.Update(courseModel);
      await _context.SaveChangesAsync();

      // 7. Response
      return CreatedAtAction(nameof(GetById), new { id = courseModel.Id }, courseModel.ToDto());
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromForm] UpdateCourseRequestDto courseDto)
    {
      // 1. First its gonna be validated by DTO definition
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      // 2. Custom validation for unique Course name 
      if (await IsCourseNameUniqueAsync(courseDto.Name, id) == false)
      {
        ModelState.AddModelError(nameof(courseDto.Name), CourseValidationConstants.NameUniqueErrorMessage);
        return BadRequest(ModelState);
      }

      // 3. If not file was uploaded (shouldnt be happening because of previous validation)
      if (courseDto.File == null || courseDto.File.Length == 0)
        return BadRequest("No file uploaded.");
      
      // 4. Recovering specific db row of the course and including its students
      var courseModel = await _context.Courses
        .Include(c => c.Students)  
        .FirstOrDefaultAsync(c => c.Id == id);

      // 5. If no course was found
      if (courseModel == null)
      {
        return NotFound();
      }

      // 6. Parsing file
      var fileName = courseModel.Id.ToString() + Path.GetExtension(courseDto.File.FileName);
      var filePath = Path.Combine(_imagePath, fileName);

      using (var stream = new FileStream(filePath, FileMode.Create))
      {
        await courseDto.File.CopyToAsync(stream);
      }

      // 7. Updating its attributes
      courseModel.Name = courseDto.Name;
      courseModel.Description = courseDto.Description;
      courseModel.Schedule = courseDto.Schedule;
      courseModel.Professor = courseDto.Professor;
      courseModel.ImageUrl = fileName;
      await _context.SaveChangesAsync();

      // 8. Mapping the course to DTO and also mapping its students
      var courseResponseDto = courseModel.ToDto();
      courseResponseDto.Students = courseModel.Students?.Select(s => s.ToBasicDto()).ToList();

      // 9. Response
      return Ok(courseResponseDto);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
      // 1. Recovering specific db row
      var courseModel = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);

      // 2. If no course was found
      if (courseModel == null)
      {
        return NotFound();
      }

      // 3. Deleting from db
      _context.Courses.Remove(courseModel);
      await _context.SaveChangesAsync();

      // 4. Response
      return NoContent();
    }

    // Special service: verify unique Course name
    [NonAction]
    private async Task<bool> IsCourseNameUniqueAsync(string name, int? excludeId = null)
    {
        // 1. Search courses with the same name
        var query = _context.Courses.Where(c => c.Name == name);
        
        // 2. We need to exclude the own course from the check (if it's an update)
        if (excludeId.HasValue)
        {
          query = query.Where(c => c.Id != excludeId.Value);
        }
        
        // 3. Executing the query
        return !await query.AnyAsync();
    }
  }
}