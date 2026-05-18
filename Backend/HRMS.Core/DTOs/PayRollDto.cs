namespace HRMS.Core.DTOs
{
    public class PayRollDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? DeptName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime PayDate { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal TaxDeduction { get; set; }
        public decimal OtherDeductions { get; set; }
        public decimal NetSalary { get; set; }
        public string? Status { get; set; }

        public class GeneratePayrollDto
        {
            public int Month { get; set; }
            public int Year { get; set; }
        }
    }
}
