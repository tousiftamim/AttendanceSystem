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
    public class ShiftController : Controller
    {
        private readonly ShiftRepository _shiftRepository = new ShiftRepository();
        public ActionResult Index()
        {
            List<Shift> shifts = _shiftRepository.Get();
            return View(shifts);
        }

        public ActionResult Details(int id)
        {
            Shift shift = _shiftRepository.Get(id);
            return View(shift);
        }

        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Create(Shift s)
        {
            try
            {
                Shift shift = _shiftRepository.Create(s);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            Shift shift = _shiftRepository.Get(id);
            return View(shift);
        }

        
        [HttpPost]
        public ActionResult Edit(Shift shift)
        {
            try
            {
                _shiftRepository.Edit(shift);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Delete(int id)
        {
            var isDeleted =_shiftRepository.Delete(id);
            if (!isDeleted)
            {
                ViewBag.Message = "Could not delete shift. Employees are assined to it.";
                List<Shift> shifts = _shiftRepository.Get();
                return View("Index",shifts);
            }
            return RedirectToAction("Index");
        }


    }
}
