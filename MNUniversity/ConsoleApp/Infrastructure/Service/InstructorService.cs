using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.DataAccess.Context;
using ConsoleApp.DataAccess.Entity;
using ConsoleApp.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp.Infrastructure.Service
{
    public interface IInstructorService
    {
        Task<InstructorIndexData> GetById(int? id, int? CourseID);
    }

    public class InstructorService : IInstructorService
    {
        protected readonly SchoolContext _context;

        public InstructorService(SchoolContext context)
        {
            _context = context;
        }

        public async Task<InstructorIndexData> GetById(int? id, int? CourseID)
        {
            var viewModel = new InstructorIndexData();
            viewModel.Instructors = await _context.Instructors
                .Include(x => x.OfficeAssignment)
                .Include(x => x.CourseAssignments)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.Enrollments)
                .ThenInclude(x => x.Student)
                .Include(x => x.CourseAssignments)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.Department).AsNoTracking().ToListAsync();

            if (id != null)
            {
                Instructor instructor = viewModel.Instructors.Where(x => x.ID == id).Single();
                viewModel.Courses = instructor.CourseAssignments.Select(x => x.Course);
            }

            if (CourseID != null)
            {
                var selectedCourse = viewModel.Courses.Where(x => x.CourseID == CourseID).Single();
                await _context.Entry(selectedCourse).Collection(x => x.Enrollments).LoadAsync();
                foreach (Enrollment enrollment in selectedCourse.Enrollments)
                {
                    await _context.Entry(enrollment).Reference(x => x.Student).LoadAsync();
                }

                viewModel.Enrollments = selectedCourse.Enrollments;
            }
            return viewModel;
        }
    }

}
