using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.DataAccess.Context;
using ConsoleApp.DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp.Infrastructure.Service
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetList();
        void Insert(Course course);
        void Update(Course course);
        void Delete(int? id);
    }

    public class CourseService : ICourseService
    {
        private readonly SchoolContext _context;

        public CourseService(SchoolContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Course>> GetList()
        {

            var list = await _context.Courses.AsNoTracking().ToListAsync();
            return list;
        }

        public void Insert(Course course)
        {
            if(course == null) return;

            try
            {
                _context.Courses.Add(course);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Update(Course course)
        {
            if(course == null) return;
            try
            {
                _context.Courses.Update(course);
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
            if(id == 0) return;

            var course = _context.Courses.FirstOrDefault(x => x.CourseID == id);
            if(course == null) return;

            try
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
