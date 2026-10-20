namespace OnlineCourseEnrollment.Models
{
    // Represents a course available for enrollment.
    public class Course
    {
        // Stores the unique ID of the course.
        public int CourseId { get; set; }

        // Stores the name of the course.
        public string CourseName { get; set; } = string.Empty;

        // Stores the course duration.
        public string Duration { get; set; } = string.Empty;

        // Stores the original course fees.
        public decimal Fees { get; set; }

        // Stores the discount percentage.
        public decimal DiscountPercentage { get; set; }
    }
}