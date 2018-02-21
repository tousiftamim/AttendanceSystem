using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AttendanceSystem.Models
{
    [Table("Branch")]
    public class Branch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullAddress { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }   
    }
}