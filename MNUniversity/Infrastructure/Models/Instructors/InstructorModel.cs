using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using AutoMapper;
using DataAccess.Entity;

namespace Infrastructure.Models.Instructors
{
	public class InstructorModel
	{
		[StringLength(50), Required]
		public string LastName { get; set; }
		[StringLength(50), Required]
		public string FirstMideName { get; set; }
		[Display(Name = "Hire Date"), DataType(DataType.Date), Required]
		public DateTime HireDate { get; set; }
		[StringLength(50), RegularExpression(@"^[A-Z]+[a-zA-Z0-0]""'\s-]*$")]
		public string FullName { get; set; }
	}
}
