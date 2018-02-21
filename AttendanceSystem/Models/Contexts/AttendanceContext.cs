using System.Data.Entity;

namespace AttendanceSystem.Models.Contexts
{
    public class AttendanceContext: DbContext
    {
        public AttendanceContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        //public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Branch> Branches { get; set; }

        public DbSet<LeaveCount> LeaveCounts { get; set; }
        public DbSet<EmployeeLeave> EmployeeLeaves { get; set; }

    }
}