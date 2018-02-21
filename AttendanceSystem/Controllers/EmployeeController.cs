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
    public class EmployeeController : Controller
    {
        private readonly EmployeeRepository _employeeRepository = new EmployeeRepository();
        private readonly ShiftRepository _shiftRepository = new ShiftRepository();
        private readonly DeptRepository _deptRepository = new DeptRepository();
        private readonly BranchRepository _branchRepository = new BranchRepository();
        //
        // GET: /Employee/

        public ActionResult Index()
        {
            List<Employee> employees = _employeeRepository.Get();
            return View(employees);
        }

        //
        // GET: /Employee/Details/5

        public ActionResult Details(int id)
        {
            Employee employee = _employeeRepository.Get(id);
            return View(employee);
        }

        //
        // GET: /Employee/Create

        public ActionResult Create()
        {
            var shifts = _shiftRepository.Get();
            ViewBag.Dept = _deptRepository.Get();
            ViewBag.Branch = _branchRepository.Get();
            return View(shifts);
        }

        //
        // POST: /Employee/Create

        [HttpPost]
        public ActionResult Create(Employee e)
        {
            try
            {
                Employee employee = _employeeRepository.Create(e);
                return RedirectToAction("Index");
            }
            catch
            {
                var shifts = _shiftRepository.Get();
                ViewBag.Dept = _deptRepository.Get();
                ViewBag.Branch = _branchRepository.Get();
                return View(shifts);
            }
        }

        //
        // GET: /Employee/Edit/5

        public ActionResult Edit(int id)
        {
            Employee employee = _employeeRepository.Get(id);
            ViewBag.Shifts = _shiftRepository.Get();
            ViewBag.Dept = _deptRepository.Get();
            ViewBag.Branch = _branchRepository.Get();
            return View(employee);
        }

        //
        // POST: /Employee/Edit/5

        [HttpPost]
        public ActionResult Edit(Employee employee)
        {
            try
            {
                _employeeRepository.Edit(employee);
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Shifts = _shiftRepository.Get();
                ViewBag.Dept = _deptRepository.Get();
                ViewBag.Branch = _branchRepository.Get();
                return View(employee);
            }
        }

        //
        // GET: /Employee/Delete/5

        public ActionResult Delete(int id)
        {
            _employeeRepository.Delete(id);
            return RedirectToAction("Index");
        }

        //
        // POST: /Employee/Delete/5

    }
}
