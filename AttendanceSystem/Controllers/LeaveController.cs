using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttendanceSystem.Models;
using AttendanceSystem.Repositories;

namespace AttendanceSystem.Controllers
{
    [Authorize(Roles = "admin")]
    public class LeaveController : Controller
    {
        readonly EmployeeRepository _employeeRepository = new EmployeeRepository();
        private readonly LeaveCountRepository _leaveCountRepository = new LeaveCountRepository();
        readonly EmployeeLeaveRepository _employeeLeaveRepository = new EmployeeLeaveRepository();

        public ActionResult Index()
        {
            return View(_leaveCountRepository.GetAll());
        }
        public ActionResult CreateEmplyeeOverAllLeave()
        {
            ViewBag.EmployeeId = new SelectList(_employeeRepository.Get(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult CreateEmplyeeOverAllLeave(LeaveCount leaveCount)
        {
            var item = _leaveCountRepository.FindBy(count => count.EmployeeId == leaveCount.EmployeeId).FirstOrDefault();
            if (item == null)
            {
                _leaveCountRepository.Create(leaveCount);
                return RedirectToAction("Index");
            }
            ViewBag.ErrorMessage = "Already Exists";
            return View("ErrorMessage");

        }

        public ActionResult AddLeaveInfo()
        {
            ViewBag.EmployeeId = new SelectList(_employeeRepository.Get(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult AddLeaveInfo(EmployeeLeave leaveInfo)
        {
            _employeeLeaveRepository.Create(leaveInfo);
            ViewBag.EmployeeId = new SelectList(_employeeRepository.Get(), "Id", "Name");
            return View();
        }

        public ActionResult LeaveInfo(int employeeId = 0)
        {
            ViewBag.EmployeeId = new SelectList(_employeeRepository.Get(), "Id", "Name");
            var leaveStats = _employeeLeaveRepository.FindBy(leave => leave.EmployeeId == employeeId).ToList();
            return View(leaveStats);
        }

    }
}