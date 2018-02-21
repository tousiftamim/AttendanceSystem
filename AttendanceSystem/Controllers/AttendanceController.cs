using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using AttendanceSystem.Models;
using AttendanceSystem.Models.Actatec;
using AttendanceSystem.Repositories;
using AttendanceSystem.Services;
using WebMatrix.WebData;

namespace AttendanceSystem.Controllers
{
    [Authorize(Roles="admin")]
    public class AttendanceController : Controller
    {
        readonly EventLogRepository _eventLogRepository = new EventLogRepository();
        readonly EmployeeRepository _employeeRepository = new EmployeeRepository();
        readonly ExportService _exportService = new ExportService();
        readonly AttendanceService _attendanceService = new AttendanceService();
        public ActionResult Index(string from, string to, int referenceId = 0)
        {
            var events = _eventLogRepository.Get();
            var employees = _employeeRepository.Get();
            ViewBag.Employees = employees;

            var query = _attendanceService.GetUserAttendance(events, employees);
            List<Attendance> result;
            query = referenceId != 0 ? query.Where(_ => _.UserId == referenceId) : query;
            DateTime fromDate, toDate;
            if (from != null && DateTime.TryParse(from, out fromDate)) query = query.Where(_ => _.Date >= fromDate);
            if (to != null && DateTime.TryParse(to, out toDate)) query = query.Where(_ => _.Date <= toDate);

            return View(query.ToList());
        }



        public string PrintFullAttendanceExcel(string from, string to, int referenceId = 0)
        {
            var events = _eventLogRepository.Get();
            var employees = _employeeRepository.Get();
            ViewBag.Employees = employees;

            var query = _attendanceService.GetUserAttendance(events, employees);
            List<Attendance> result;
            query = referenceId != 0 ? query.Where(_ => _.UserId == referenceId) : query;
            DateTime fromDate, toDate;
            if (from != null && DateTime.TryParse(from, out fromDate)) query = query.Where(_ => _.Date >= fromDate);
            if (to != null && DateTime.TryParse(to, out toDate)) query = query.Where(_ => _.Date <= toDate);

            var sw = new StringWriter();
            var htw = new HtmlTextWriter(sw);
            var grid = _exportService.ExportExcelAttendanceList(query.ToList());
            if (grid.HeaderRow != null)
                grid.HeaderRow.BackColor = Color.White;

            grid.RenderControl(htw);

            //style to format numbers to string
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);

            grid.HeaderStyle.BackColor = Color.White;
            Response.ClearContent();
            Response.Buffer = true;
            var fileName = "test.xls";
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return fileName;
        }

        public class AttendanceByUser
        {
            public int UserId { get; set; }
            public DateTime? Date { get; set; }
            public DateTime FirstEntryTime { get; set; }
            public DateTime LastExitTime { get; set; }
        }

    }
}
