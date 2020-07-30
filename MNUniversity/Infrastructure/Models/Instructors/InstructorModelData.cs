using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DataAccess.Entity;

namespace Infrastructure.Models.Instructors
{
	public class InstructorModelData
	{
		public int ID { get; set; }
		public string LastName { get; set; }
		public string FirstMidName { get; set; }
		public DateTime HireDate { get; set; }
		public IEnumerable<Course> Courses { get; set; }
		public IEnumerable<DataAccess.Entity.Enrollment> Enrollments { get; set; }
	}
}
