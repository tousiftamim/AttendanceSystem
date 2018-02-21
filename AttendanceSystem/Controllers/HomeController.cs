using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AttendanceSystem.Models;
using AttendanceSystem.Repositories;
using AttendanceSystem.Services;
using WebMatrix.WebData;

namespace AttendanceSystem.Controllers
{
    public class HomeController : Controller
    {
        readonly EventLogRepository _eventLogRepository = new EventLogRepository();
        readonly EmployeeRepository _employeeRepository = new EmployeeRepository();
        readonly AttendanceService _attendanceService = new AttendanceService();

       
        public ActionResult Index(string from, string to, int referenceId = 0)
        {
            var user = System.Web.HttpContext.Current.User.Identity.Name;
            
            //var refId = WebSecurity.CurrentUserId;
            //referenceId =  (user == null) ? referenceId : referenceId = refId;
            var events = _eventLogRepository.Get();
            var employees = _employeeRepository.Get();
            ViewBag.Employees = employees;
            referenceId = employees.Where(t => t.Name == user).Select(t => t.ReferenceId).FirstOrDefault();
            var query = _attendanceService.GetUserAttendance(events, employees);
            List<Attendance> result;
            query = referenceId != 0 ? query.Where(_ => _.UserId == referenceId) : query;
            DateTime fromDate, toDate;
            if (from != null && DateTime.TryParse(from, out fromDate))
            {
                query = query.Where(_ => _.Date >= fromDate) ;
            }
            else
            {
                query = query.Where(_ => _.Date == DateTime.Today);
            }

            if (to != null && DateTime.TryParse(to, out toDate))
            {
                query = query.Where(_ => _.Date <= toDate);
            }
            else
            {
                query = query.Where(_ => _.Date == DateTime.Today);
            }
        
            

            return View(query.ToList());
        }
        [Authorize(Roles = "admin")]
        public ActionResult TodaysEntry()
        {
            var events = _eventLogRepository.GetTodaysLog();
            var employees = _employeeRepository.Get();
            var todaysEntry = (from e in events
                               group e by new
                               {
                                   e.USERID
                               }
                                   into userEvents
                                   join employee in employees on Int32.Parse(userEvents.Key.USERID) equals employee.ReferenceId
                                   let firstOrDefault = userEvents.FirstOrDefault(_ => TimeSpan.Compare(_.LOCAL_TIMESTAMP.TimeOfDay,
                                            DateTime.Parse("12/12/12 7:00").TimeOfDay) == 1)
                                   where firstOrDefault != null
                                   select new Attendance
                                   {
                                       UserId = employee.Id,
                                       Name = employee.Name,
                                       Shift = employee.Shift,
                                       FirstEntryTime = firstOrDefault.LOCAL_TIMESTAMP
                                   }).OrderBy(_ => _.Name).ToList();

            return View(todaysEntry);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
