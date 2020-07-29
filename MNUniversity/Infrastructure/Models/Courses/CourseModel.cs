using AutoMapper;
using DataAccess.Entity;

namespace Infrastructure.Models.Courses
{
	public class CourseModel
	{
		public int CourseID { get; set; }
		public string Title { get; set; }
		public int Credits { get; set; }
		public int DepartmentID { get; set; }
	}
}
