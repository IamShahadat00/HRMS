
namespace HRMS.Core.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string? DeptName { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
