using HRMS.Core.DTOs;
using HRMS.Core.Entities;

namespace HRMS.Core.Interfaces
{
    public interface ISalaryService
    {
            Task<SalaryDto?> GetSalaryByEmployeeIdAsync(int employeeId);
            Task<bool> SetSalaryAsync(Salary salary);          
            Task<bool> UpdateSalaryAsync(int employeeId, Salary salary);
            Task<IEnumerable<SalaryDto>> GetAllSalariesAsync(); 
    }
}
