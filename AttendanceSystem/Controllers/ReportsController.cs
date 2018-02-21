using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using AttendanceSystem.Models;
using AttendanceSystem.Repositories;
using AttendanceSystem.Services;

namespace AttendanceSystem.Controllers
{
    [Authorize(Roles = "admin")]
    public class ReportsController : Controller
    {
        readonly EventLogRepository _eventLogRepository = new EventLogRepository();
        readonly EmployeeRepository _employeeRepository = new EmployeeRepository();
        readonly AttendanceService _attendanceService = new AttendanceService();
        readonly ExportService _exportService = new ExportService();
        public ActionResult Index()
        {
            var employees = _employeeRepository.Get();
            ViewBag.Employees = employees;
            return View();
        }

        public string ExportEmployee()
        {
            GridView grid = null;
            var fileName = "";
            {
                var query = _employeeRepository.Get();
                grid = _exportService.ExportExcelAbsentList(query);
                fileName = "Employee List.xls";
            }


            if (grid != null)
            {
                var sw = new StringWriter();
                var htw = new HtmlTextWriter(sw);


                if (grid.HeaderRow != null)
                    grid.HeaderRow.BackColor = Color.White;

                grid.RenderControl(htw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                grid.HeaderStyle.BackColor = Color.White;
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                Response.ContentType = "application/vnd.ms-excel";
                Response.Charset = "";
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return fileName;
        }


        public string Export(DateTime date)
        {
            GridView grid = null;
            var fileName = "";
            if (Request.Form["exportabsentlist"] != null)
            {
                var query = ExportAbsentList(date);
                grid = _exportService.ExportExcelAbsentList(query);
                fileName = "Absence List " + date.ToShortDateString() + ".xls";
            }
            else if (Request.Form["exportattendancelist"] != null)
            {
                var query = ExportAttendanceList(date);
                grid = _exportService.ExportExcelAttendanceList(query);
                fileName = "Attendance List " + date.ToShortDateString() + ".xls";
            }


            if (grid != null)
            {
                var sw = new StringWriter();
                var htw = new HtmlTextWriter(sw);


                if (grid.HeaderRow != null)
                    grid.HeaderRow.BackColor = Color.White;

                grid.RenderControl(htw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                grid.HeaderStyle.BackColor = Color.White;
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                Response.ContentType = "application/vnd.ms-excel";
                Response.Charset = "";
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return fileName;
        }


        public string Export2(DateTime from, DateTime to)
        {
            GridView grid = null;
            var fileName = "";
            if (Request.Form["exportlatelist"] != null)
            {
                var query = ExportLateList(from, to);
                grid = _exportService.ExportExcelLateList(query);
                fileName = "Late List from " + from.ToShortDateString() + " to " + to.ToShortDateString() + ".xls";
            }
            else if (Request.Form["attendancesummary"] != null)
            {
                var query = ExportAttendanceSummary(from, to);
                grid = _exportService.ExportExcelAttendanceSummary(query);
                fileName = "Attendance Summary from " + from.ToShortDateString() + " to " + to.ToShortDateString() + ".xls";
            }

            if (grid != null)
            {
                var sw = new StringWriter();
                var htw = new HtmlTextWriter(sw);


                if (grid.HeaderRow != null)
                    grid.HeaderRow.BackColor = Color.White;

                grid.RenderControl(htw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                grid.HeaderStyle.BackColor = Color.White;
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                Response.ContentType = "application/vnd.ms-excel";
                Response.Charset = "";
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return fileName;
        }


        public string Export3(DateTime from, DateTime to, int id)
        {
            GridView grid = null;
            var fileName = "";
            Employee employee = _employeeRepository.Get(id);
            if (Request.Form["exployeeattendancelist"] != null)
            {
                var query = ExportEmployeeAttendanceList(from, to, id);
                grid = _exportService.ExportEmployeeAttendanceList(query);
                fileName = "Employee " + employee.Name + " Attendance List from "
                    + from.ToShortDateString() + " to " + to.ToShortDateString() + ".xls";
            }
            if (grid != null)
            {
                var sw = new StringWriter();
                var htw = new HtmlTextWriter(sw);

                htw.Write("Employee Id: " + employee.Id);
                htw.Write("<br>");
                htw.Write("Employee Name: " + employee.Name);
                htw.Write("<br>");
                htw.Write("Employee Designation: " + employee.Designation);
                htw.Write("<br>");
                htw.Write("Employee Branch: " + employee.Branch.Name);
                htw.Write("<br>");
                htw.Write("Employee Department: " + employee.Department.Name);
                htw.Write("<br>");
                htw.Write("Employee Shift: " + employee.Shift.Name);
                htw.Write("<br>");
                htw.Write("Shift Grace Entry Time: " + employee.Shift.GraceEntryTime);
                htw.Write("<br>");
                htw.Write("Shift Exit Time: " + employee.Shift.ExpectedExitTime);
                htw.Write("<br>");
                if (grid.HeaderRow != null)
                    grid.HeaderRow.BackColor = Color.White;

                grid.RenderControl(htw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                grid.HeaderStyle.BackColor = Color.White;
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                Response.ContentType = "application/vnd.ms-excel";
                Response.Charset = "";
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return fileName;
        }

        public List<AttendanceSummary> ExportAttendanceSummary(DateTime from, DateTime to)
        {
            var events = _eventLogRepository.Get(from, to);
            var employees = _employeeRepository.Get();
            var query = _attendanceService.GetUserAttendance(events, employees).ToList();
            var groupByUser = query.GroupBy(_ => _.Name);
            var attendanceSummaries = new List<AttendanceSummary>();
            foreach (var g in groupByUser)
            {
                var totalPresents = g.Count();
                var totalLates = g.Count(day => TimeSpan.Compare(day.FirstEntryTime.TimeOfDay,
                    TimeSpan.Parse(day.Shift.GraceEntryTime).Add(TimeSpan.FromMinutes(1))) == 1);
                totalPresents -= totalLates;
                var totalDays = totalPresents + totalLates;
                attendanceSummaries.Add(new AttendanceSummary
                {
                    UserId = g.FirstOrDefault().Employee.ReferenceId,
                    Shift = g.FirstOrDefault().Shift,
                    Department = g.FirstOrDefault().Department,
                    Branch = g.FirstOrDefault().Branch,
                    Name = g.Key,
                    TotalDays = (int)totalDays,
                    TotalPresents = totalPresents,
                    TotalLates = totalLates
                });
            }
            return attendanceSummaries.OrderBy(_ => _.Branch.Name)
                .ThenBy(_ => _.Department.Name).ThenBy(_ => _.Shift.Name)
                .ThenBy(_ => _.Name).ToList();
        }
        public List<Attendance> ExportEmployeeAttendanceList(DateTime from, DateTime to, int id)
        {
            var events = _eventLogRepository.Get(from, to);
            var employees = new List<Employee>();
            employees.Add(_employeeRepository.Get(id));
            var query = _attendanceService.GetUserAttendance(events, employees).ToList();
            return query;
        }
        public List<Attendance> ExportLateList(DateTime from, DateTime to)
        {
            var events = _eventLogRepository.Get(from, to);
            var employees = _employeeRepository.Get();

            var query = _attendanceService.GetLateUsers(events, employees);
            return query;
        }
        public List<Employee> ExportAbsentList(DateTime date)
        {
            var events = _eventLogRepository.GetLogByDate(date);
            var employees = _employeeRepository.Get();

            var query = _attendanceService.GetAbsentUsers(events, employees);
            return query;
        }

        public List<Attendance> ExportAttendanceList(DateTime date)
        {
            var events = _eventLogRepository.GetLogByDate(date);
            var employees = _employeeRepository.Get();

            var query = _attendanceService.GetUserAttendance(events, employees)
                .OrderBy(_ => _.Branch.Name).ThenBy(_ => _.Department.Name)
                .ThenBy(_ => _.Shift.Name).ThenBy(_ => _.Name).ToList();

            return query;
        }
    }
}
