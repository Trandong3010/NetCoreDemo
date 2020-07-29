using AutoMapper;
using DataAccess.Entity;
using Infrastructure.Common;
using Infrastructure.Models.Courses;
using Infrastructure.Models.Enrollment;
using Infrastructure.Models.Instructors;
using Infrastructure.Models.Students;

namespace Infrastructure.Models
{
	public class AutoMapping: Profile
	{
		public AutoMapping()
		{
			CreateMap<Course, CourseModel>();
			CreateMap<CourseModel, Course>();

			CreateMap<Instructor, InstructorModel>();
			CreateMap<InstructorModel, Instructor>();

			CreateMap<Student, StudentModel>();
			CreateMap<StudentModel, Student>();

			CreateMap<DataAccess.Entity.Enrollment, EnrollmentModel>();
			CreateMap<EnrollmentModel, DataAccess.Entity.Enrollment>();
		}
	}
}
