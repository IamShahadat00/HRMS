using HRMS.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PayRoll> PayRolls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Salary>()
                .HasOne(s => s.Employee)
                .WithOne(e => e.Salary)
                .HasForeignKey<Salary>(s => s.EmployeeId);

            modelBuilder.Entity<PayRoll>()
                .HasOne(p => p.Employee)
                .WithMany(e => e.PayRolls)
                .HasForeignKey(p => p.EmployeeId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Employee)
                .WithOne(e => e.User)
                .HasForeignKey<User>(u => u.EmployeeId)
                .IsRequired(false);

            modelBuilder.Entity<Salary>(e => {
                e.Property(s => s.BaseSalary).HasColumnType("decimal(18,2)");
                e.Property(s => s.MedicalAllowance).HasColumnType("decimal(18,2)");
                e.Property(s => s.ConveyanceAllowance).HasColumnType("decimal(18,2)");
                e.Property(s => s.OtherBonuses).HasColumnType("decimal(18,2)");
                e.Property(s => s.Deductions).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<PayRoll>(e => {
                e.Property(p => p.GrossSalary).HasColumnType("decimal(18,2)");
                e.Property(p => p.TaxDeductions).HasColumnType("decimal(18,2)");
                e.Property(p => p.OtherDeductions).HasColumnType("decimal(18,2)");
                e.Property(p => p.NetSalary).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique();

            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, DeptName = "Human Resources" },
                new Department { Id = 2, DeptName = "Information Technology" },
                new Department { Id = 3, DeptName = "Finance" },
                new Department { Id = 4, DeptName = "Sales" },
                new Department { Id = 5, DeptName = "Operations" }
            );
        }
    }
}
