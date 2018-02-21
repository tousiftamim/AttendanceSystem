using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceSystem.Models
{
    public class AttendanceSummary
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public Shift Shift { get; set; }
        public Department Department { get; set; }
        public Branch Branch { get; set; }
        public int TotalDays { get; set; }
        public int TotalPresents { get; set; }
        public int TotalLates { get; set; }
        public int ReamainingCasualLeave { get; set; }
        public int ReamainingEarnLeave { get; set; }
        public int ReamainingSickLeave { get; set; }

       
    }
}