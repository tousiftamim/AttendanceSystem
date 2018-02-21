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
    public class DeptController : Controller
    {
        private readonly DeptRepository _deptRepository = new DeptRepository();
        public ActionResult Index()
        {
            List<Department> departments = _deptRepository.Get();
            return View(departments);
        }

        public ActionResult Details(int id)
        {
            Department department = _deptRepository.Get(id);
            return View(department);
        }

        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Create(Department d)
        {
            try
            {
                Department department = _deptRepository.Create(d);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            Department department = _deptRepository.Get(id);
            return View(department);
        }

        
        [HttpPost]
        public ActionResult Edit(Department department)
        {
            try
            {
                _deptRepository.Edit(department);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Delete(int id)
        {
            var isDeleted =_deptRepository.Delete(id);
            if (!isDeleted)
            {
                ViewBag.Message = "Could not delete department. Employees are assined to it.";
                List<Department> departments = _deptRepository.Get();
                return View("Index",departments);
            }
            return RedirectToAction("Index");
        }


    }
}
