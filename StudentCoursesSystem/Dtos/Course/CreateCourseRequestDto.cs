using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Course
{
  public class CreateCourseRequestDto
  {
    [Required(ErrorMessage = CourseValidationConstants.NameRequiredErrorMessage)]
    [StringLength(CourseValidationConstants.NameMaxLength, 
      MinimumLength = CourseValidationConstants.NameMinLength,
      ErrorMessage = CourseValidationConstants.NameLengthErrorMessage)]
    public string Name { get; set; }

    [Required(ErrorMessage = CourseValidationConstants.DescriptionRequiredMessage)]
    [StringLength(CourseValidationConstants.DescriptionMaxLength, 
      MinimumLength = CourseValidationConstants.DescriptionMinLength,
      ErrorMessage = CourseValidationConstants.DescriptionLengthErrorMessage)]
    public string Description { get; set; }

    [Required(ErrorMessage = CourseValidationConstants.ScheduleRequiredMessage)]
    public string Schedule { get; set; }

    [Required(ErrorMessage = CourseValidationConstants.ProfessorRequiredMessage)]
    [StringLength(CourseValidationConstants.ProfessorMaxLength,
      MinimumLength = CourseValidationConstants.ProfessorMinLength)]
    [RegularExpression(CourseValidationConstants.ProfessorRegex,
      ErrorMessage = CourseValidationConstants.ProfessorErrorMessage)]
    public string Professor { get; set; }

    [Required(ErrorMessage = CourseValidationConstants.FileRequiredMessage)]
    public IFormFile File { get; set; }
  }
}