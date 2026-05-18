using HRMS.Core.DTOs;
using HRMS.Core.Interfaces;
using HRMS.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static HRMS.Core.DTOs.PayRollDto;

namespace HRMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PayRollController : ControllerBase
    {
        private readonly IPayRollService _payrollService;

        public PayRollController(IPayRollService payrollService)
        {
            _payrollService = payrollService;
        }

        [HttpPost("generate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GeneratePayroll([FromBody] GeneratePayrollDto request)
        {
            if (request.Month < 1 || request.Month > 12)
                return BadRequest(new { message = "Invalid month selected." });

            try
            {
                var payrolls = await _payrollService.GenerateMonthlyPayrollAsync(request.Month, request.Year);
                return Ok(new
                {
                    message = $"Payroll generated successfully for {request.Month}/{request.Year}",
                    data = payrolls
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An internal error occurred", error = ex.Message });
            }
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetPayrollByEmployeeId(int employeeId)
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
            var payroll = await _payrollService.GetPayrollByEmployeeAsync(employeeId);
            if (payroll == null)
            {
                return NotFound(new { message = "No payroll found for this employee." });
            }
            return Ok(payroll);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayrollById(int id)
        {
            var payroll = await _payrollService.GetPayrollByIdAsync(id);
            if (payroll == null)
            {
                return NotFound(new { message = "Payroll record not found." });
            }
            return Ok(payroll);
        }
    }
}
