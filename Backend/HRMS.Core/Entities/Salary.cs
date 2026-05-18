using HRMS.Core.Entities;

namespace HRMS.Core.Entities
{
    public class Salary
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public decimal BaseSalary { get; set; }
        public decimal MedicalAllowance { get; set; }
        public decimal ConveyanceAllowance { get; set; }
        public decimal OtherBonuses { get; set; }
        public decimal Deductions { get; set; }
    }
}
