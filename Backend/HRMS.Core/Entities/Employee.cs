using System.ComponentModel.DataAnnotations;

namespace HRMS.Core.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Position { get; set; }
        public string? Address { get; set; }
        public string? AccountNumber { get; set; }
        public bool isActive { get; set; } = true;
        public DateTime Hiredate { get; set; } = DateTime.Now;
        public int DepartmentId { get; set; }
        public Department? Department { get; set; } = null!;
        public Salary? Salary { get; set; }
        public ICollection<PayRoll> PayRolls { get; set; } = new List<PayRoll>();
        public User? User { get; set; }
    }
}
