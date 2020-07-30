using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DataAccess.Entity;

namespace Infrastructure.Models.Courses
{
	public class CourseModel
	{
		public int CourseID { get; set; }
		[StringLength(50), Required]
		public string Title { get; set; }
		[Required]
		public int? Credits { get; set; }
		[Required]
		public int DepartmentID { get; set; }
	}
}
