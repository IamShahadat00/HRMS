namespace HRMS.Core.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Position { get; set; }
        public string? Address { get; set; }
        public string? DeptName { get; set; }
        public int DepartmentId { get; set; }
        public string? AccountNumber { get; set; }
        public DateTime HireDate { get; set; }
        public bool isActive { get; set; } 

    }
}
