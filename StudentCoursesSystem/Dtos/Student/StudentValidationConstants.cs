namespace api.Dtos.Student
{
    public static class StudentValidationConstants
    {
        // Name
        public const int NameMinLength = 2;
        public const int NameMaxLength = 255;
        public const string NameRegex = @"^[\p{L}\s]+$";
        public const string NameRequiredMessage = "Name is required";
        public const string NameRegexErrorMessage = "Name can only contain letters and spaces";
        public const string NameLengthErrorMessage = "Name must be between {2} and {1} characters";
        public const string NameUniqueErrorMessage = "Student name must be unique";

        // Email
        public const string EmailRequiredMessage = "Email is required";
        public const string EmailErrorMessage = "Please enter a valid email address";
        
        // Phone
        public const int PhoneMinLength = 7;
        public const int PhoneMaxLength = 15;
        public const string PhoneRegex = @"^[0-9]+$";
        public const string PhoneRequiredMessage = "Phone is required";
        public const string PhoneRegexErrorMessage = "Phone number can only contain digits";
        public const string PhoneLengthErrorMessage = "Phone number must be between {2} and {1} digits";

        // CourseId
        public const string CourseIdRequiredMessage = "Course ID is required";
        public const string CourseIdNumberErrorMessage = "Course ID must be a valid number";
    }
}