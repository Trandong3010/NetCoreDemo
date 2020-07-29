using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Context;
using DataAccess.Entity;
using Infrastructure.Models;
using Infrastructure.Models.Instructors;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service
{
    public interface IInstructorService
    {
	    Task<IEnumerable<InstructorModel>> GetList();
        Task<InstructorIndexData> GetById(int? id, int? courseId);
        void Update(int? id, string[] selectedCourses);
    }

    public class InstructorService : IInstructorService
    {
        protected readonly SchoolContext _context;
        protected readonly IMapper _mapper;

        public InstructorService(SchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<InstructorModel>> GetList()
        {
	        return _mapper.Map<IEnumerable<InstructorModel>>(await _context.Instructors.AsNoTracking().ToListAsync());
        }

        public async Task<InstructorIndexData> GetById(int? id, int? courseId)
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
                Instructor instructor = viewModel.Instructors.Single(x => x.ID == id);
                viewModel.Courses = instructor.CourseAssignments.Select(x => x.Course);
            }

            if (courseId != null)
            {
                var selectedCourse = viewModel.Courses.Single(x => x.CourseID == courseId);
                await _context.Entry(selectedCourse).Collection(x => x.Enrollments).LoadAsync();
                foreach (Enrollment enrollment in selectedCourse.Enrollments)
                {
                    await _context.Entry(enrollment).Reference(x => x.Student).LoadAsync();
                }

                viewModel.Enrollments = selectedCourse.Enrollments;
            }
            return viewModel;
        }

        public void Update(int? id, string[] selectedCourses)
        {
            if (id == null) return;

            var instructorToUpdate = _context.Instructors
                .Include(x => x.OfficeAssignment)
                .Include(x => x.CourseAssignments).ThenInclude(x => x.Course)
                .AsNoTracking().FirstOrDefault(m => m.ID == id);

            UpdateInstructorCourses(selectedCourses, instructorToUpdate);
            PopulateAssignedCourseData(instructorToUpdate);

        }


        public void PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourse = _context.Courses;
            var instructorCourses = new HashSet<int>(instructor.CourseAssignments.Select(c => c.CourseID));
            var viewModel = new List<AssignedCourseData>();
            foreach (var course in allCourse)
            {
                viewModel.Add(new AssignedCourseData
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Assigned = instructorCourses.Contains(course.CourseID)
                });
            }
        }

        private void UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
        {
            if (selectedCourses == null)
            {
                instructorToUpdate.CourseAssignments = new List<CourseAssignment>();
                return;
            }

            var selectedCourseHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>(instructorToUpdate.CourseAssignments.Select(c => c.CourseID));
            foreach (var course in _context.Courses)
            {
                if (selectedCourseHS.Contains(course.CourseID.ToString()))
                {
                    if (!instructorCourses.Contains(course.CourseID))
                    {
                        instructorToUpdate.CourseAssignments.Add(new CourseAssignment
                        {
                            InstructorID = instructorToUpdate.ID,
                            CourseID = course.CourseID
                        });
                    }
                }
                else
                {
                    if (instructorCourses.Contains(course.CourseID))
                    {
                        CourseAssignment courseToRemove =
                            instructorToUpdate.CourseAssignments.FirstOrDefault(x => x.CourseID == course.CourseID);
                        _context.Remove(courseToRemove);
                    }
                }
            }
        }
    }

}
