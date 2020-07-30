using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DataAccess.Entity;

namespace Infrastructure.Models.Enrollment
{
	public class EnrollmentModel
	{
		[Required]
		public int CourseID { get; set; }
		[Required]
		public int StudentID { get; set; }
		[Required]
		public Grade Grade { get; set; }
	}
}
