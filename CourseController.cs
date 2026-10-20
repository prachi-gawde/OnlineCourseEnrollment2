using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OnlineCourseEnrollment.Models;
using OnlineCourseEnrollment.Extensions;
using System.Text.Json;

namespace OnlineCourseEnrollment.Controllers
{
    // Handles course listing, details, enrollment, and users.
    public class CourseController : Controller
    {
        // Stores the memory cache service.
        private readonly IMemoryCache _cache;

        // Receives the memory cache through dependency injection.
        public CourseController(IMemoryCache cache)
        {
            _cache = cache;
        }

        // Returns the course list, using a five-minute cache.
        public IActionResult Index()
        {
            // Retrieve the course list from the cache if available.
            if (!_cache.TryGetValue("CourseList", out List<Course>? courses))
            {
                // Create sample course records.
                courses = new List<Course>
                {
                    new Course
                    {
                        CourseId = 1,
                        CourseName = "ASP.NET Core MVC",
                        Duration = "3 Months",
                        Fees = 30000,
                        DiscountPercentage = 10
                    },
                    new Course
                    {
                        CourseId = 2,
                        CourseName = "Machine Learning",
                        Duration = "4 Months",
                        Fees = 40000,
                        DiscountPercentage = 15
                    },
                    new Course
                    {
                        CourseId = 3,
                        CourseName = "Python Programming",
                        Duration = "2 Months",
                        Fees = 20000,
                        DiscountPercentage = 5
                    }
                };

                // Set the cache expiration to five minutes.
                var options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(5)
                };

                // Save the course list in memory cache.
                _cache.Set("CourseList", courses, options);
            }

            // Display the course list.
            return View(courses);
        }

        // Displays course details and enables response caching.
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        public IActionResult Details(int id)
        {
            // Retrieve the course list from the cache.
            if (!_cache.TryGetValue("CourseList", out List<Course>? courses))
            {
                // Populate the cache if it has expired.
                return RedirectToAction(nameof(Index));
            }

            // Find the requested course by ID.
            var course = courses.FirstOrDefault(c => c.CourseId == id);

            // Return a not-found response if the course does not exist.
            if (course == null)
            {
                return NotFound();
            }

            // Display the selected course.
            return View(course);
        }

        // Displays the enrollment form.
        public IActionResult Enroll(int id)
        {
            // Store the selected course ID in session.
            HttpContext.Session.SetInt32("SelectedCourse", id);

            // Retrieve the available courses.
            if (!_cache.TryGetValue("CourseList", out List<Course>? courses))
            {
                return RedirectToAction(nameof(Index));
            }

            // Find the selected course.
            var course = courses.FirstOrDefault(c => c.CourseId == id);

            // Check whether the course exists.
            if (course == null)
            {
                return NotFound();
            }

            // Display the enrollment form.
            return View(course);
        }

        // Processes the submitted enrollment form.
        [HttpPost]
        public IActionResult Enroll(int courseId, string studentName)
        {
            // Store the student name in a browser cookie.
            Response.Cookies.Append("StudentName", studentName);

            // Store the selected course in session.
            HttpContext.Session.SetInt32("SelectedCourse", courseId);

            // Redirect to Details, passing the course ID in the query string.
            return RedirectToAction("Details", new { id = courseId });
        }

        // Retrieves users asynchronously from JSONPlaceholder.
        public async Task<IActionResult> Users()
        {
            // Create an HTTP client for the API request.
            using var client = new HttpClient();

            // Send an asynchronous GET request.
            var response = await client.GetAsync(
                "https://jsonplaceholder.typicode.com/users");

            // Throw an exception if the request fails.
            response.EnsureSuccessStatusCode();

            // Read the JSON response.
            var json = await response.Content.ReadAsStringAsync();

            // Convert the JSON response into User objects.
            var users = JsonSerializer.Deserialize<List<User>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            // Pass the users to the view.
            return View(users ?? new List<User>());
        }
    }
}