using HRMS.Core.DTOs;
using HRMS.Core.Entities;
using HRMS.Core.Interfaces;
using HRMS.Data;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Services.Implementations
{
    public class PayrollService : IPayRollService
    {
        private readonly AppDbContext _context;
        public PayrollService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PayRollDto>> GenerateMonthlyPayrollAsync(int month, int year)
        {
            var exist = await _context.PayRolls.AnyAsync(p => p.Month == month && p.Year == year);
            if (exist) throw new InvalidOperationException($"Payroll for {month}/{year} already exists.");

            var employees = await _context.Employees
                .Include(e => e.Salary)
                .Include(e => e.Department)
                .Where(e => e.isActive && e.Salary != null)
                .ToListAsync();

            if (!employees.Any())
            {
                throw new InvalidOperationException("No active employees with salary configurations were found.");
            }

            var payrolls = new List<PayRoll>();

            foreach (var employee in employees)
            {
                var sal = employee.Salary!; 

                decimal gross = sal.BaseSalary + sal.MedicalAllowance + sal.ConveyanceAllowance + sal.OtherBonuses;
                decimal monthlyTax = CalCulateTax(gross);
                decimal net = gross - monthlyTax - sal.Deductions;

                payrolls.Add(new PayRoll
                {
                    EmployeeId = employee.Id,
                    Month = month,
                    Year = year,
                    GrossSalary = gross,
                    TaxDeductions = monthlyTax,
                    OtherDeductions = sal.Deductions,
                    NetSalary = net,
                    PayDate = DateTime.Now,
                    Status = "Generated"
                });
            }

            _context.PayRolls.AddRange(payrolls);
            await _context.SaveChangesAsync();

            return await GetPayrollByMonthYearAsync(month, year);
        }

        public async Task<IEnumerable<PayRollDto>> GetPayrollByEmployeeAsync(int employeeId)
        {
            return await _context.PayRolls.Include(p => p.Employee).ThenInclude(e => e.Department)
                .Where(p => p.EmployeeId == employeeId).OrderBy(p => p.Month).ThenBy(p => p.Year)
                .Select(p => new PayRollDto
                {
                    Id = p.Id,
                    EmployeeId = p.EmployeeId,
                    EmployeeName = $"{p.Employee.FirstName} {p.Employee.LastName}",
                    DeptName = p.Employee.Department.DeptName,
                    Month = p.Month,
                    Year = p.Year,
                    PayDate = p.PayDate,
                    GrossSalary = p.GrossSalary,
                    TaxDeduction = p.TaxDeductions,
                    NetSalary = p.NetSalary,
                    Status = p.Status,
                    OtherDeductions = p.OtherDeductions
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<PayRollDto>> GetPayrollByMonthYearAsync(int month, int year)
        {
            return await _context.PayRolls.Include(p => p.Employee).ThenInclude(e => e.Department)
                .Where(p => p.Month == month && p.Year == year).OrderBy(p => p.Employee.LastName)
                .Select(p => new PayRollDto
                {
                    Id= p.Id,
                    EmployeeId= p.EmployeeId,
                    EmployeeName= $"{p.Employee.FirstName} {p.Employee.LastName}",
                    DeptName= p.Employee.Department.DeptName,
                    Month = p.Month,
                    Year = p.Year,
                    PayDate = p.PayDate,
                    GrossSalary = p.GrossSalary,
                    TaxDeduction = p.TaxDeductions,
                    NetSalary = p.NetSalary,
                    Status = p.Status,
                    OtherDeductions = p.OtherDeductions,

                })
                .ToListAsync();
        }

        public async Task<PayRollDto?> GetPayrollByIdAsync(int id)
        {
            var p = await _context.PayRolls.Include(p => p.Employee).ThenInclude(e => e.Department)
                .FirstOrDefaultAsync(p => p.Id == id);
            return p == null ? null : new PayRollDto
            {
                Id = p.Id,
                EmployeeId = p.EmployeeId,
                EmployeeName = $"{p.Employee.FirstName} {p.Employee.LastName}",
                DeptName = p.Employee.Department.DeptName,
                Month = p.Month,
                Year = p.Year,
                PayDate = p.PayDate,
                GrossSalary = p.GrossSalary,
                TaxDeduction = p.TaxDeductions,
                NetSalary = p.NetSalary,
                Status = p.Status,
                OtherDeductions = p.OtherDeductions
            };
        }

        private static decimal CalCulateTax(decimal monthlyGross)
        {
            decimal annualGross = monthlyGross * 12;
            decimal tax = 0;

            if (annualGross <= 350000)
            {
                tax = 0;
            }
            else if (annualGross <= 450000)
            {
                tax = (annualGross - 350000) * 0.05m;
            }
            else if (annualGross <= 750000)
            {
                tax = 5000 + (annualGross - 450000) * 0.10m;
            }
            else
            {
                tax = 35000 + (annualGross - 750000) * 0.15m;
            }
            return Math.Round(tax / 12, 2); 
        }

    }
}