using System.Collections.Generic;
using System.Data;
using System.Linq;
using AttendanceSystem.Models;
using AttendanceSystem.Models.Contexts;

namespace AttendanceSystem.Repositories
{
    public class DeptRepository
    {
        readonly AttendanceContext _dbContex = new AttendanceContext();

        public List<Department> Get()
        {
            return _dbContex.Departments.OrderBy(_=>_.Name).ToList();
        }

        public Department Get(int id)
        {
            return _dbContex.Departments.Find(id);
        }

        public Department Create(Department department)
        {
            _dbContex.Departments.Add(department);
            _dbContex.SaveChanges();
            return department;
        }

        public bool Delete(int id)
        {
            var employees = _dbContex.Employees.Select(_ => _.DeptId == id).ToList();
            if (employees.Count > 0) return false;
            var dept = _dbContex.Departments.Find(id);
            if (dept == null) return false;
            _dbContex.Departments.Remove(dept);
            _dbContex.SaveChanges();
            return true;
        }

        public Department Edit(Department department)
        {
            _dbContex.Entry(department).State = EntityState.Modified;
            _dbContex.SaveChanges();
            return department;
        }
    }
}