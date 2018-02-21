using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AttendanceSystem.Models
{
    [Table("LeaveCount")]
    public class LeaveCount
    {
        public int Id { get; set; }
        public int EarnLeave { get; set; }
        public int SickLeave { get; set; }
        public int CasualLeave { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}