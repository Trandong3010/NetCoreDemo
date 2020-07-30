using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Infrastructure.Models.Departments
{
	public class DepartmentModel
	{
		public int DepartmentID { get; set; }
		[StringLength(50), Required]
		public string Name { get; set; }
		[Range(1, 100)]
		[DataType(DataType.Currency)]
		[Column(TypeName = "decimal(18, 2)")]
		[Required]
		public decimal Budget { get; set; }
		[Display(Name = "Start Date")]
		[DataType(DataType.Date)]
		[Required]
		public DateTime StartDate { get; set; }
		[Required]
		public int InstructorID { get; set; }
	}
}
