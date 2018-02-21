using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AttendanceSystem.Models
{
    public class Attendance
    {
        public int UserId { get; set; }
        public string Name { get; set; }

        public Employee Employee { get; set; }
        public DateTime JoiningDate { get; set; }
        public Shift Shift { get; set; }
        public Department Department { get; set; }
        public Branch Branch { get; set; }
        public DateTime Date { get; set; }
        public DateTime FirstEntryTime { get; set; }
        public DateTime LastExitTime { get; set; }
    }
}