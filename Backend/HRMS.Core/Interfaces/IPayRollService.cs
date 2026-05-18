using HRMS.Core.DTOs;

namespace HRMS.Core.Interfaces
{
    public interface IPayRollService 
    {
        Task<IEnumerable<PayRollDto>> GenerateMonthlyPayrollAsync(int month, int year);
        Task<IEnumerable<PayRollDto>> GetPayrollByEmployeeAsync(int employeeId);
        Task<IEnumerable<PayRollDto>> GetPayrollByMonthYearAsync(int month, int year);
        Task<PayRollDto?> GetPayrollByIdAsync(int id);
    }
}
