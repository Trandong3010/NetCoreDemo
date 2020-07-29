using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using DataAccess.Entity;

namespace Infrastructure.Models.Instructors
{
	public class InstructorModel
	{
		public string LastName { get; set; }
		public string FirstMideName { get; set; }
		public DateTime HireDate { get; set; }
		public string FullName { get; set; }
	}
}
