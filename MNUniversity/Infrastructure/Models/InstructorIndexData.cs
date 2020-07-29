using System.Collections.Generic;
using DataAccess.Entity;

namespace Infrastructure.Models
{
    public class InstructorIndexData
    {
        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<DataAccess.Entity.Enrollment> Enrollments { get; set; }
    }
}
