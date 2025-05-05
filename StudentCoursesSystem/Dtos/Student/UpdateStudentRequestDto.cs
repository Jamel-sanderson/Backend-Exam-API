using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Student
{
  public class UpdateStudentRequestDto
  {
    
    [Required(ErrorMessage = StudentValidationConstants.NameRequiredMessage)]
    [StringLength(StudentValidationConstants.NameMaxLength, 
      MinimumLength = StudentValidationConstants.NameMinLength, 
      ErrorMessage = StudentValidationConstants.NameLengthErrorMessage)]
    [RegularExpression(StudentValidationConstants.NameRegex, 
      ErrorMessage = StudentValidationConstants.NameRegexErrorMessage)]
    public string Name { get; set; }

    [Required(ErrorMessage = StudentValidationConstants.EmailRequiredMessage)]
    [EmailAddress(ErrorMessage = StudentValidationConstants.EmailErrorMessage)]
    public string Email { get; set; }

    [Required(ErrorMessage = StudentValidationConstants.PhoneRequiredMessage)]
    [StringLength(StudentValidationConstants.PhoneMaxLength, 
      MinimumLength = StudentValidationConstants.PhoneMinLength, 
      ErrorMessage = StudentValidationConstants.PhoneLengthErrorMessage)]
    [RegularExpression(StudentValidationConstants.PhoneRegex, 
      ErrorMessage = StudentValidationConstants.PhoneRegexErrorMessage)]
    public string Phone { get; set; }

    [Required(ErrorMessage = StudentValidationConstants.CourseIdRequiredMessage)]
    [Range(1, int.MaxValue, ErrorMessage = StudentValidationConstants.CourseIdNumberErrorMessage)]
    public int CourseId { get; set; }

  }
}