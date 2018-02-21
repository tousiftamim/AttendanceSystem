using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceSystem.Models.ViewModel
{
    public class LeaveSummaryViewModel
    {
        public int EmployeeId { get; set; }
        public int ReamainingCasualLeave { get; set; }
        public int ReamainingEarnLeave { get; set; }
        public int ReamainingSickLeave { get; set; }

        public int CasualLeaveTakenInTimeFrame { get; set; }
        public int EarnLeaveTakenInTimeFrame { get; set; }
        public int SickLeaveTakenInTimeFrame { get; set; }
    }
}