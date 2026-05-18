using HRMS.Core.DTOs;
using HRMS.Core.Entities;
using HRMS.Core.Interfaces;
using HRMS.Data;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;
        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddEmployeeAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp == null)
            {
                return false;
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.EmployeeId == id);
            if (user != null)
            { 
                _context.Users.Remove(user);
            }

            _context.Employees.Remove(emp);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync(string? search = null, bool? isActive = null, int? departmentId = null)
        {
            var query = _context.Employees.Include(e => e.Department).AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                var s = search.ToLower();
                query = query.Where(e =>
                ((e.FirstName != null && e.FirstName.ToLower().Contains(s)) ||
                 (e.LastName != null && e.LastName.ToLower().Contains(s)) ||
                 (e.Email != null && e.Email.ToLower().Contains(s)) ||
                 (e.Position != null && e.Position.ToLower().Contains(s))));
            }

            if (isActive.HasValue)
            {
                query = query.Where(e => e.isActive == isActive.Value);
            }

            if (departmentId.HasValue)
            {
                query = query.Where(e => e.DepartmentId == departmentId.Value);
            }
            var employees = await query.ToListAsync();
            // Inside the .Select(e => new EmployeeDto { ... }) block:
            return employees.Select(e => new EmployeeDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                Position = e.Position,
                DeptName = e.Department?.DeptName,
                DepartmentId = e.DepartmentId,
                AccountNumber = e.AccountNumber,
                HireDate = e.Hiredate,
                Address = e.Address,
                isActive = e.isActive
            });

        }

        public async Task<EmployeeDto> GetEmployeeByIdAsync(int id)
        {
            var e = await _context.Employees.Include(e => e.Department).FirstOrDefaultAsync(e => e.Id == id);
            if (e == null)
            {
                return null;
            }
            return new EmployeeDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                Position = e.Position,
                DeptName = e.Department?.DeptName,
                DepartmentId = e.DepartmentId,
                AccountNumber = e.AccountNumber,
                Address = e.Address,
                isActive = e.isActive
            };
        }
        public async Task<bool> UpdateEmployeeAsync(int id, Employee employee)
        {
            var existing = await _context.Employees.FindAsync(id);
            if (existing == null) return false;

            existing.FirstName = employee.FirstName;
            existing.LastName = employee.LastName;
            existing.Email = employee.Email;
            existing.Phone = employee.Phone;
            existing.Position = employee.Position;
            existing.Address = employee.Address;
            existing.DepartmentId = employee.DepartmentId;
            existing.AccountNumber = employee.AccountNumber;
            existing.isActive = employee.isActive;

            _context.Employees.Update(existing);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
