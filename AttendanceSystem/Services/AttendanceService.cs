using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AttendanceSystem.Models;
using AttendanceSystem.Models.Actatec;

namespace AttendanceSystem.Services
{
    public class AttendanceService
    {
        public IQueryable<Attendance> GetUserAttendance(List<EventLog> events, List<Employee> employees)
        {
            var eventGroupBy =
                (from e in events
                 group e by new
                 {
                     e.USERID,
                     Date = e.LOCAL_TIMESTAMP.Date
                 }
                     into userEvents
                     join employee in employees on Int32.Parse(userEvents.Key.USERID) equals employee.ReferenceId

                     let lastentry = userEvents.OrderByDescending(_ => _.LOCAL_TIMESTAMP)
                     .FirstOrDefault(_ => TimeSpan.Compare(_.LOCAL_TIMESTAMP.TimeOfDay,
                         DateTime.Parse("12/12/12 7:00").TimeOfDay) == 1)
                     where lastentry != null
                     let firstentry = userEvents.OrderBy(_ => _.LOCAL_TIMESTAMP)
                     .FirstOrDefault(_ => TimeSpan.Compare(_.LOCAL_TIMESTAMP.TimeOfDay,
                         DateTime.Parse("12/12/12 7:00").TimeOfDay) == 1)
                     where firstentry != null
                     select (new Attendance
                     {
                         UserId = Int32.Parse(userEvents.Key.USERID),
                         Date = userEvents.Key.Date,
                         FirstEntryTime = firstentry.LOCAL_TIMESTAMP,
                         LastExitTime = lastentry.LOCAL_TIMESTAMP,
                         Name = employee.Name,
                         Shift = employee.Shift,
                         Department = employee.Department,
                         Branch = employee.Branch,
                         Employee = employee
                     })).AsQueryable();

            return eventGroupBy;
        }


        public IQueryable<Attendance> GetUserAttendanceWithLeave(List<EventLog> events, List<Employee> employees)
        {
            var eventGroupBy =
                (from e in events
                 group e by new
                 {
                     e.USERID,
                     Date = e.LOCAL_TIMESTAMP.Date
                 }
                     into userEvents
                     join employee in employees on Int32.Parse(userEvents.Key.USERID) equals employee.ReferenceId

                     let lastentry = userEvents.OrderByDescending(_ => _.LOCAL_TIMESTAMP)
                     .FirstOrDefault(_ => TimeSpan.Compare(_.LOCAL_TIMESTAMP.TimeOfDay,
                         DateTime.Parse("12/12/12 7:00").TimeOfDay) == 1)
                     where lastentry != null
                     let firstentry = userEvents.OrderBy(_ => _.LOCAL_TIMESTAMP)
                     .FirstOrDefault(_ => TimeSpan.Compare(_.LOCAL_TIMESTAMP.TimeOfDay,
                         DateTime.Parse("12/12/12 7:00").TimeOfDay) == 1)
                     where firstentry != null
                     select (new Attendance
                     {
                         UserId = Int32.Parse(userEvents.Key.USERID),
                         Date = userEvents.Key.Date,
                         FirstEntryTime = firstentry.LOCAL_TIMESTAMP,
                         LastExitTime = lastentry.LOCAL_TIMESTAMP,
                         Name = employee.Name,
                         Shift = employee.Shift,
                         Department = employee.Department,
                         Branch = employee.Branch,
                         Employee = employee
                     })).AsQueryable();

            return eventGroupBy;
        }
        public List<Employee> GetAbsentUsers(List<EventLog> events, List<Employee> employees)
        {
            var presentUserIds = GetUserAttendance(events, employees).Select(_ => _.UserId).ToList();
            var absentList = employees.Where(_ => !presentUserIds.Contains(_.ReferenceId))
                .OrderBy(_ => _.Branch.Name).ThenBy(_=>_.Department.Name)
                .ThenBy(_=>_.Shift.Name).ThenBy(_=>_.Name).ToList();
            return absentList;
        }

        public List<Attendance> GetLateUsers(List<EventLog> events, List<Employee> employees)
        {
            var attendance = GetUserAttendance(events, employees).Where(_ => (TimeSpan.Compare(_.FirstEntryTime.TimeOfDay,
                TimeSpan.Parse(_.Shift.GraceEntryTime).Add(TimeSpan.FromMinutes(1))) == 1
                )).OrderBy(_ => _.Date).ThenBy(_ => _.Branch.Name).ThenBy(_ => _.Department.Name)
                .ThenBy(_ => _.Shift.Name).ThenBy(_ => _.Name).ToList();
            return attendance;
        }
    }
}