using HRMS.Core.DTOs;
using HRMS.Core.Entities;
using HRMS.Core.Interfaces;
using HRMS.Data;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Services.Implementations
{
    public class SalaryService : ISalaryService
    {
        private readonly AppDbContext _context;
        public SalaryService(AppDbContext context)
        {
            _context = context;
        }

        // 1. Get All Salaries (Used by the Management Table)
        public async Task<IEnumerable<SalaryDto>> GetAllSalariesAsync()
        {
            var salaries = await _context.Salaries
                .Include(s => s.Employee)
                .AsNoTracking() // Improves performance for read-only lists
                .ToListAsync();

            return salaries.Select(s => MapToDto(s));
        }

        // 2. Get Salary by Employee ID
        public async Task<SalaryDto?> GetSalaryByEmployeeIdAsync(int employeeId)
        {
            var s = await _context.Salaries
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(s => s.EmployeeId == employeeId);

            return s == null ? null : MapToDto(s);
        }

        // 3. Set or Update Salary (Upsert logic)
        public async Task<bool> SetSalaryAsync(Salary salary)
        {
            var existing = await _context.Salaries
                .FirstOrDefaultAsync(s => s.EmployeeId == salary.EmployeeId);

            if (existing != null)
            {
                existing.BaseSalary = salary.BaseSalary;
                existing.MedicalAllowance = salary.MedicalAllowance;
                existing.ConveyanceAllowance = salary.ConveyanceAllowance;
                existing.OtherBonuses = salary.OtherBonuses;
                existing.Deductions = salary.Deductions;
                _context.Salaries.Update(existing);
            }
            else
            {
                await _context.Salaries.AddAsync(salary);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        // 4. Manual Update Method
        public async Task<bool> UpdateSalaryAsync(int employeeId, Salary salary)
        {
            var existingSalary = await _context.Salaries
                .FirstOrDefaultAsync(s => s.EmployeeId == employeeId);

            if (existingSalary == null) return false;

            existingSalary.BaseSalary = salary.BaseSalary;
            existingSalary.MedicalAllowance = salary.MedicalAllowance;
            existingSalary.ConveyanceAllowance = salary.ConveyanceAllowance;
            existingSalary.OtherBonuses = salary.OtherBonuses;
            existingSalary.Deductions = salary.Deductions;

            _context.Salaries.Update(existingSalary);
            return await _context.SaveChangesAsync() > 0;
        }

        // HELPER METHOD: To keep math consistent everywhere
        private SalaryDto MapToDto(Salary s)
        {
            return new SalaryDto
            {
                Id = s.Id,
                EmployeeId = s.EmployeeId,
                EmployeeName = s.Employee != null ? $"{s.Employee.FirstName} {s.Employee.LastName}" : "Unknown",
                BaseSalary = s.BaseSalary,
                MedicalAllowance = s.MedicalAllowance,
                ConveyanceAllowance = s.ConveyanceAllowance,
                OtherBonuses = s.OtherBonuses,
                Deductions = s.Deductions,
                GrossSalary = s.BaseSalary + s.MedicalAllowance + s.ConveyanceAllowance + s.OtherBonuses
            };
        }
    }
}