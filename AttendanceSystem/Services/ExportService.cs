using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using AttendanceSystem.Models;
using WebMatrix.WebData;

namespace AttendanceSystem.Services
{
    public class ExportService
    {
        public GridView ExportExcelAttendanceList(List<Attendance> attendances)
        {
            var products = new System.Data.DataTable("attendance");
            products.Columns.Add("Id", typeof(string));
            products.Columns.Add("Reference Id", typeof(string));
            products.Columns.Add("Name", typeof(string));
            products.Columns.Add("Designation", typeof(string));
            products.Columns.Add("Shift Name", typeof(string));
            products.Columns.Add("Department Name", typeof(string));
            products.Columns.Add("Branch Name", typeof(string));
            products.Columns.Add("Entry Time", typeof(string));
            products.Columns.Add("Exit Time", typeof(string));
            products.Columns.Add("Working Hour", typeof(string));
            products.Columns.Add("Status", typeof(string));

            foreach (var att in attendances)
            {
                products.Rows.Add(att.Employee.Id, att.Employee.ReferenceId, att.Name, att.Employee.Designation,
                    att.Shift.Name, att.Department.Name, att.Branch.Name,
                    att.FirstEntryTime.ToLongTimeString(), att.LastExitTime.ToLongTimeString(),
                    att.LastExitTime.Subtract(att.FirstEntryTime),
                    TimeSpan.Compare(att.FirstEntryTime.TimeOfDay, TimeSpan.Parse(att.Shift.GraceEntryTime).Add(TimeSpan.FromMinutes(1))) == 1 ? "Late Entry" : "On Time");

            }

            var grid = new GridView
            {
                DataSource = products
            };
            grid.DataBind();
            return grid;
        }
        public GridView ExportEmployeeAttendanceList(List<Attendance> attendances)
        {
            var products = new System.Data.DataTable("attendance");
            products.Columns.Add("Log Date", typeof(string));
            products.Columns.Add("Entry Time", typeof(string));
            products.Columns.Add("Exit Time", typeof(string));
            products.Columns.Add("Working Hour", typeof(string));
            products.Columns.Add("Entry Status", typeof(string));
            products.Columns.Add("Exit Status", typeof(string));

            foreach (var att in attendances)
            {
                products.Rows.Add(att.Date.ToShortDateString(),
                    att.FirstEntryTime.ToLongTimeString(), att.LastExitTime.ToLongTimeString(),
                    att.LastExitTime.Subtract(att.FirstEntryTime),
                    TimeSpan.Compare(att.FirstEntryTime.TimeOfDay,
                    TimeSpan.Parse(att.Shift.GraceEntryTime).Add(TimeSpan.FromMinutes(1))) == 1
                    ? "Late Entry" : (TimeSpan.Compare(att.FirstEntryTime.TimeOfDay,
                    TimeSpan.Parse(att.Shift.ExpectedEntryTime).Subtract(TimeSpan.FromMinutes(30))) == -1 ?
                    "Early Entry" : "On Time"), TimeSpan.Compare(att.LastExitTime.TimeOfDay,
                    TimeSpan.Parse(att.Shift.ExpectedExitTime).Add(TimeSpan.FromHours(12)).Add(TimeSpan.FromMinutes(30))) == 1?
                    "Late Out" : TimeSpan.Compare(att.LastExitTime.TimeOfDay,
                    TimeSpan.Parse(att.Shift.ExpectedExitTime).Add(TimeSpan.FromHours(12)).Subtract(TimeSpan.FromMinutes(30))) == -1?
                    "Early Out": "On Time");
            }

            var grid = new GridView
            {
                DataSource = products
            };
            grid.DataBind();
            return grid;
        }

        public GridView ExportExcelAbsentList(List<Employee> employees)
        {
            var products = new System.Data.DataTable("employees");
            products.Columns.Add("Id", typeof(string));
            products.Columns.Add("Name", typeof(string));
            products.Columns.Add("Joining Date", typeof(string));
            products.Columns.Add("Designation", typeof(string));
            products.Columns.Add("Referance Id", typeof(string));
            products.Columns.Add("Shift Name", typeof(string));
            products.Columns.Add("Department Name", typeof(string));
            products.Columns.Add("Branch Name", typeof(string));
            products.Columns.Add("Contact Address", typeof(string));
            products.Columns.Add("Father's Name", typeof(string));
            products.Columns.Add("Date of Birth", typeof(string));
            products.Columns.Add("Section", typeof(string));
            products.Columns.Add("Grade", typeof(string));
            products.Columns.Add("Category", typeof(string));
            products.Columns.Add("GrossSalary", typeof(string));

            foreach (var em in employees)
            {
                products.Rows.Add(em.Id, em.Name, em.JoiningDate.ToShortDateString(), em.Designation,
                    em.ReferenceId, em.Shift.Name, em.Department.Name, em.Branch.Name,
                    em.ContactAddress, em.FathersName, em.DateofBirth.ToShortDateString(), em.Section,
                    em.Grade, em.Category, em.GrossSalary);
            }

            var grid = new GridView
            {
                DataSource = products
            };
            grid.DataBind();
            return grid;
        }

        public GridView ExportExcelAttendanceSummary(List<AttendanceSummary> attendanceSummaries)
        {
            var products = new System.Data.DataTable("employees");
            products.Columns.Add("Id", typeof(string));
            products.Columns.Add("Name", typeof(string));
            products.Columns.Add("Shift", typeof(string));
            products.Columns.Add("Department", typeof(string));
            products.Columns.Add("Branch", typeof(string));
            products.Columns.Add("Total Days", typeof(string));
            products.Columns.Add("Total Presents", typeof(string));
            products.Columns.Add("Total Lates", typeof(string));

            foreach (var atts in attendanceSummaries)
            {
                products.Rows.Add(atts.UserId, atts.Name, atts.Shift.Name,
                    atts.Department.Name, atts.Branch.Name, atts.TotalDays,
                    atts.TotalPresents, atts.TotalLates);
            }

            var grid = new GridView
            {
                DataSource = products
            };
            grid.DataBind();
            return grid;
        }
        public GridView ExportExcelLateList(List<Attendance> attendances)
        {
            var products = new System.Data.DataTable("employees");
            products.Columns.Add("Id", typeof(string));
            products.Columns.Add("Name", typeof(string));
            products.Columns.Add("Designation", typeof(string));
            products.Columns.Add("Shift Name", typeof(string));
            products.Columns.Add("Department Name", typeof(string));
            products.Columns.Add("Branch Name", typeof(string));
            products.Columns.Add("Attendance Date", typeof(string));
            products.Columns.Add("Entry Time", typeof(string));
            products.Columns.Add("Last Expected Entry", typeof(string));

            foreach (var att in attendances)
            {
                products.Rows.Add(att.Employee.Id, att.Employee.Name, att.Employee.Designation,
                    att.Shift.Name, att.Department.Name, att.Branch.Name,
                    att.Date.ToShortDateString(), att.FirstEntryTime.TimeOfDay,
                    att.Shift.GraceEntryTime);
            }

            var grid = new GridView
            {
                DataSource = products
            };
            grid.DataBind();
            return grid;
        }
    }
}