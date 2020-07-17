using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.DataAccess.Context;
using ConsoleApp.DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp.Infrastructure.Service
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetList();
    }

    public class DepartmentService : IDepartmentService
    {
        protected readonly SchoolContext _context;

        public DepartmentService(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetList()
        {
            var deparments = await _context.Departments.Include(x => x.Courses).AsNoTracking().ToListAsync();
            return deparments;
        }
    }
}
