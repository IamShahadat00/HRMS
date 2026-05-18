namespace HRMS.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? userName { get; set; }
        public string? PassWord { get; set; }
        public string? Role { get; set; }
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
