using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceSystem.Models.Actatec
{
    public class EmployeeAttendance
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public List<Attendance> Attendances { get; set; } 
    }
}