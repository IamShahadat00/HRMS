using HRMS.Core.Entities;
using HRMS.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeeController : ControllerBase 
{
    private readonly IEmployeeService _employeeService;
    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllEmployees([FromQuery] string? search = null)
    {
        var employees = await _employeeService.GetAllEmployeesAsync(search);
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        if (userRole == "Employee")
        {
            var employeeIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (employeeIdClaim == null)
            {
                return Forbid();
            }
        }
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        return employee == null ? NotFound(new {message = "Employee not found"}) : Ok(employee);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
    {
        var result = await _employeeService.AddEmployeeAsync(employee);
        if (result)
        {
            return Ok(new { message = "Employee added successfully" });
        }
        return BadRequest("Could not add employee");
    }

    [HttpPut("{id}")] 
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee employee)
    {
        var result = await _employeeService.UpdateEmployeeAsync(id, employee);
        if (result)
        {
            return Ok(new { message = "Updated successfully" });
        }
        return NotFound(new { message = "Employee not found" });
    }
}