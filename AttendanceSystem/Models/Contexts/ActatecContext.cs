using System.Data.Entity;
using AttendanceSystem.Models.Actatec;

namespace AttendanceSystem.Models.Contexts
{
    public class ActatecContext: DbContext
    {
        public ActatecContext()
            : base("ActatekConnection")
        {
        }

        public DbSet<EventLog> EventLogs { get; set; }

    }
}