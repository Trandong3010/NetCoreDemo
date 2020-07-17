using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.DataAccess.Context;
using ConsoleApp.DataAccess.Entity;
using ConsoleApp.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp.Infrastructure.Service
{

    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetList();
        void Insert(Student student);
        void Update(Student student);
        void Delete(int? id);
        Student GetById(int id);

        Task<IEnumerable<Student>> GetListPaging(string sortOrder, string currentFilter, string searchString,
            int? pageNumber);
    }

    public class StudentService : IStudentService
    {
        protected readonly SchoolContext _context;
        public StudentService(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetList()
        {

            var list = await _context.Students.AsNoTracking().ToListAsync();
            return list;
        }

        public void Insert(Student student)
        {
            if(student == null) return;

            try
            {
                _context.Students.Add(student);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Update(Student student)
        {
            if (student == null) return;
            try
            {
                _context.Students.Update(student);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Delete(int? id)
        {
            if(id == null) return;

            var student = _context.Students.FirstOrDefault(x => x.ID == id);
            if(student == null) return;
            try
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Student GetById(int id)
        {
            var student = _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course).AsNoTracking()
                .FirstOrDefault(x => x.ID == id);
            return student;
        }

        public async Task<IEnumerable<Student>> GetListPaging(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var students = from s in _context.Students select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(x =>
                    x.LastName.Contains(searchString) || x.FirstMidName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(x => x.LastName);
                    break;
                case "Date":
                    students = students.OrderByDescending(x => x.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderBy(x => x.LastName);
                    break;
                default:
                    students = students.OrderBy(x => x.LastName);
                    break;
            }

            int pageSize = 3;
            return await PaginatedList<Student>.CreateAsync(students.AsNoTracking(), pageNumber ?? 1, pageSize);
        }
    }
}
