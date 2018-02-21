using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceSystem.Models
{
    [Table("Employee")]
    public class Employee
    {
        public int Id { get; set; }
        public int ShiftId { get; set; }
        public int DeptId { get; set; }
        public int BranchId { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public DateTime JoiningDate { get; set; }
        public int ReferenceId { get; set; }
        public string ContactAddress { get; set; }
        public string FathersName { get; set; }
        public DateTime DateofBirth { get; set; }
        public string Section { get; set; }
        public string Grade { get; set; }
        public string Category { get; set; }
        public string GrossSalary { get; set; }

        [ForeignKey("ShiftId")]
        public virtual Shift Shift { get; set; }

        [ForeignKey("DeptId")]
        public virtual Department Department { get; set; }

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }
    }
}