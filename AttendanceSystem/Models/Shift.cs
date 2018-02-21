using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AttendanceSystem.Models
{
    [Table("Shift")]
    public class Shift
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ExpectedEntryTime { get; set; }
        public string GraceEntryTime { get; set; }
        public string ExpectedExitTime { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }   
    }
}