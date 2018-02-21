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
    public class BranchController : Controller
    {
        private readonly BranchRepository _branchRepository = new BranchRepository();
        public ActionResult Index()
        {
            List<Branch> branches = _branchRepository.Get();
            return View(branches);
        }

        public ActionResult Details(int id)
        {
            Branch branch = _branchRepository.Get(id);
            return View(branch);
        }

        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Create(Branch b)
        {
            try
            {
                Branch branch = _branchRepository.Create(b);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            Branch branch = _branchRepository.Get(id);
            return View(branch);
        }

        
        [HttpPost]
        public ActionResult Edit(Branch branch)
        {
            try
            {
                _branchRepository.Edit(branch);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Delete(int id)
        {
            var isDeleted =_branchRepository.Delete(id);
            if (!isDeleted)
            {
                ViewBag.Message = "Could not delete branch. Employees are assined to it.";
                List<Branch> branches = _branchRepository.Get();
                return View("Index",branches);
            }
            return RedirectToAction("Index");
        }


    }
}
