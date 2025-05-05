namespace api.Dtos.Course
{
    public static class CourseValidationConstants
    {
        // Name
        public const int NameMinLength = 10;
        public const int NameMaxLength = 63;
        public const string NameRequiredErrorMessage = "Course name is required";
        public const string NameLengthErrorMessage = "Course name must be between {2} and {1} characters";
        public const string NameUniqueErrorMessage = "Course name must be unique";

        // Description
        public const int DescriptionMinLength = 10;
        public const int DescriptionMaxLength = 500;
        public const string DescriptionRequiredMessage = "Description is required";
        public const string DescriptionLengthErrorMessage = "Description must be between {2} and {1} characters";
        
        // Schedule
        public const string ScheduleRequiredMessage = "Schedule is required";
        
        // Professor
        public const int ProfessorMinLength = 3;
        public const int ProfessorMaxLength = 255;
        public const string ProfessorRegex = @"^[\p{L}\s]+$";
        public const string ProfessorRequiredMessage = "Professor name is required";
        public const string ProfessorErrorMessage = "Professor name can only contain letters, spaces and dots";
        public const string ProfessorRegexErrorMessage = "Professor name can only contain letters and spaces";

        // File 
        public const string FileRequiredMessage = "Image file is required";
    }
}