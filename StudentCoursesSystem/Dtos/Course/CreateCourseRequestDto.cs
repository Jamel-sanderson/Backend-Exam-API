using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace api.Dtos.Course
{
  public class CreateCourseRequestDto
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public string Schedule { get; set; }
    public string Professor { get; set; }
    public IFormFile? File { get; set; }
  }
}