using OnlineCourseEnrollment.Models;

namespace OnlineCourseEnrollment.Extensions
{
    // Contains extension methods related to Course.
    public static class CourseExtensions
    {
        // Calculates the final course fee after applying the discount.
        public static decimal CalculateFinalFees(this Course course)
        {
            // Calculate the discount amount.
            decimal discountAmount =
                course.Fees * course.DiscountPercentage / 100;

            // Return the fees after subtracting the discount.
            return course.Fees - discountAmount;
        }
    }
}