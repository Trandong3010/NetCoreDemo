using System;
using System.Collections.Generic;

namespace WebAppEntryBlank.Models
{
    public partial class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public int? Credits { get; set; }
        public int? DepartmentId { get; set; }
    }
}
