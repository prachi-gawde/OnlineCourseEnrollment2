namespace OnlineCourseEnrollment.Models
{
    // Represents user information received from the API.
    public class User
    {
        // Stores the user name.
        public string Name { get; set; } = string.Empty;

        // Stores the user email.
        public string Email { get; set; } = string.Empty;

        // Stores company information.
        public Company Company { get; set; } = new Company();
    }

    // Represents company information.
    public class Company
    {
        // Stores company name.
        public string Name { get; set; } = string.Empty;
    }
}