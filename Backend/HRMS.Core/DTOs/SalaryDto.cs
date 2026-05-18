namespace HRMS.Core.DTOs
{
    public class SalaryDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public decimal BaseSalary { get; set; }
        public decimal MedicalAllowance { get; set; }
        public decimal ConveyanceAllowance { get; set; }
        public decimal OtherBonuses { get; set; }
        public decimal Deductions { get; set; }
        public decimal GrossSalary { get; set; }
    }
}
