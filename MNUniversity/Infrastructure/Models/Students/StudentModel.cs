using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DataAccess.Entity;
using Microsoft.VisualBasic.CompilerServices;

namespace Infrastructure.Models.Students
{
    public class StudentModel
    {
        public int ID { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(50)]
        public string FirstMidName { get; set; }
        [Display(Name = "Release Date"), DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }
        [StringLength(50)]
        public string FullName { get; set; }
    }
}
