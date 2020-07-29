using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Context;
using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service
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
            var deparments = await _context.Departments.AsNoTracking().Include(x => x.Courses).ToListAsync();
            return deparments;
        }
    }
}
