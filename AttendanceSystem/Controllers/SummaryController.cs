using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AttendanceSystem.Models;
using AttendanceSystem.Models.Actatec;
using AttendanceSystem.Repositories;
using AttendanceSystem.Services;

namespace AttendanceSystem.Controllers
{
    [Authorize(Roles = "admin")]
    public class SummaryController : Controller
    {
        private readonly EventLogRepository _eventLogRepository = new EventLogRepository();
        private readonly EmployeeRepository _employeeRepository = new EmployeeRepository();
        private readonly AttendanceService _attendanceService = new AttendanceService();
        private readonly ShiftRepository _shiftRepository = new ShiftRepository();
        private readonly DeptRepository _deptRepository = new DeptRepository();
        private readonly BranchRepository _branchRepository = new BranchRepository();
        LeaveCountRepository _leaveCountRepository = new LeaveCountRepository();

        public ActionResult Index(string from, string to, int shiftId = 0, int deptId = 0, int branchId = 0)
        {
            var events = _eventLogRepository.Get();
            var employees = _employeeRepository.Get();
            var shifts = _shiftRepository.Get();
            var depts = _deptRepository.Get();
            var branches = _branchRepository.Get();
            ViewBag.Shifts = shifts;
            ViewBag.Departments = depts;
            ViewBag.Branches = branches;

            var query = _attendanceService.GetUserAttendance(events, employees);
            List<Attendance> result;
            query = shiftId != 0 ? query.Where(_ => _.Shift.Id == shiftId) : query;
            query = deptId != 0 ? query.Where(_ => _.Department.Id == deptId) : query;
            query = branchId != 0 ? query.Where(_ => _.Branch.Id == branchId) : query;
            DateTime fromDate, toDate;
            if (from != null && DateTime.TryParse(from, out fromDate)) query = query.Where(_ => _.Date >= fromDate);
            if (to != null && DateTime.TryParse(to, out toDate)) query = query.Where(_ => _.Date <= toDate);

            var filterResult = query.ToList();
            var numberOfLeavesTakenByEmployee = _leaveCountRepository.GetLeaveCount(query);
            var leaveCountDictionary = numberOfLeavesTakenByEmployee.ToDictionary(l => l.EmployeeId);
            var groupByUser = filterResult.GroupBy(_ => _.UserId);
            var attendanceSummaries = new List<AttendanceSummary>();
            foreach (var g in groupByUser)
            {
                var totalPresents = g.Count();
                var totalLates = g.Count(day => TimeSpan.Compare(day.FirstEntryTime.TimeOfDay,
                    TimeSpan.Parse(day.Shift.GraceEntryTime).Add(TimeSpan.FromMinutes(1))) == 1);
                totalPresents -= totalLates;
                var totalDays = totalPresents + totalLates;
                if (leaveCountDictionary.Count > 0 && leaveCountDictionary.ContainsKey(g.FirstOrDefault().Employee.Id))
                {
                    attendanceSummaries.Add(new AttendanceSummary
                    {
                        Name = g.FirstOrDefault().Name,
                        TotalDays = (int)totalDays,
                        TotalPresents = totalPresents,
                        TotalLates = totalLates,
                        ReamainingCasualLeave = leaveCountDictionary[g.FirstOrDefault().Employee.Id].CasualLeave,
                        ReamainingEarnLeave = leaveCountDictionary[g.FirstOrDefault().Employee.Id].EarnLeave,
                        ReamainingSickLeave = leaveCountDictionary[g.FirstOrDefault().Employee.Id].SickLeave
                    });
                }
                else
                {
                    attendanceSummaries.Add(new AttendanceSummary
                    {
                        Name = g.FirstOrDefault().Name,
                        TotalDays = (int)totalDays,
                        TotalPresents = totalPresents,
                        TotalLates = totalLates,
                        ReamainingCasualLeave = 0,
                        ReamainingEarnLeave = 0,
                        ReamainingSickLeave = 0
                    });
                }

            }

            return View(attendanceSummaries.OrderBy(_ => _.Name).ToList());
        }

    }
}
