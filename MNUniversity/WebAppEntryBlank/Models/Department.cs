using System;
using System.Collections.Generic;

namespace WebAppEntryBlank.Models
{
    public partial class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public decimal? Budget { get; set; }
        public DateTime? StartDate { get; set; }
        public int? InstructorId { get; set; }
    }
}
