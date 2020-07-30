using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using DataAccess.Entity;
using Infrastructure.Common;
using Infrastructure.Models.Courses;
using Infrastructure.Models.Enrollment;
using Infrastructure.Models.Instructors;
using Infrastructure.Models.Students;
using Microsoft.EntityFrameworkCore.Internal;

namespace Infrastructure.Models
{
	public class AutoMapping : Profile
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

			CreateMap<Instructor, InstructorModelData>()
				.ForMember(x => x.LastName, opt => opt.MapFrom(y => y.LastName))
				.ForMember(x => x.FirstMidName, opt => opt.MapFrom(y => y.FirstMidName))
				.ForMember(x => x.HireDate, opt => opt.MapFrom(y => y.HireDate))
				.ForMember(x => x.Courses, opt => opt.MapFrom(y => y.CourseAssignments.Select(x => x.Course).ToList()))
			.ForMember(x => x.Enrollments, opt => opt.MapFrom(y => y.CourseAssignments.Select(x => x.Course.Enrollments).ToList()));
			//CreateMap<Course, InstructorModelData>()
			//	.ForMember(x => x.Courses, opt => opt.Ignore());
			//CreateMap<DataAccess.Entity.Enrollment, InstructorModelData>()
			//	.ForMember(x => x.Instructors, opt => opt.Ignore());
		}
	}
}
