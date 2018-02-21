using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace AttendanceSystem.Models
{

    [Table("EmployeeLeave")]
    public class EmployeeLeave
    {
        public int Id { get; set; }
        public DateTime LeaveStartDate { get; set; }
        public DateTime LeaveEndDate { get; set; }
        public int Duration { get; set; }
        public string Reason { get; set; }


        public int LeaveType { get; set; }
        public int EmployeeId { get; set; }

    }
}