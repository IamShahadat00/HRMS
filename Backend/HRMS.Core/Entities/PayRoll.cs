namespace HRMS.Core.Entities
{
    public class PayRoll
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime PayDate { get; set; }
        public decimal  GrossSalary { get; set; }
        public decimal TaxDeductions { get; set; }
        public decimal OtherDeductions { get; set; }
        public decimal NetSalary { get; set; }
        public string Status { get; set; } = "Generated";
    }
}
