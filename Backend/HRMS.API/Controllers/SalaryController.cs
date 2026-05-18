using HRMS.Core.Entities;
using HRMS.Core.Interfaces;
using HRMS.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SalaryController : ControllerBase 
    {
        private readonly ISalaryService _salaryService;
        public SalaryController(ISalaryService salaryService)
        {
            _salaryService = salaryService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllSalaries()
        {
            var result = await _salaryService.GetAllSalariesAsync();
            return Ok(result);
        }


        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetSalaryByEmployeeId(int employeeId)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole == "Employee")
            {
                var employeeIdClaim = User.FindFirst("EmployeeId")?.Value;
                if (employeeIdClaim == null || employeeIdClaim != employeeId.ToString())
                {
                    return Forbid();
                }
            }
            var salary = await _salaryService.GetSalaryByEmployeeIdAsync(employeeId);
            if(salary == null)
            {
                return NotFound(new {message = "No Salary configured for this employee"});
            } 
            return Ok(salary);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetSalary([FromBody] Salary salary)
        {
            var result = await _salaryService.SetSalaryAsync(salary);
            if (result)
            {
                return Ok(new { message = "Salary added successfully" }); // Return Object
            }
            return BadRequest(new { message = "Could not add salary" });
        }

        [HttpPut("employee/{employeeId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSalary(int employeeId, [FromBody] Salary salary)
        {
            var result = await _salaryService.UpdateSalaryAsync(employeeId, salary);
            if (result)
            {
                return Ok(new { message = "Updated successfully" });
            }
            return BadRequest("Could not update salary");
        }
    }
}
