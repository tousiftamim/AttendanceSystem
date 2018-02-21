using System.Collections.Generic;
using System.Data;
using System.Linq;
using AttendanceSystem.Models;
using AttendanceSystem.Models.Contexts;

namespace AttendanceSystem.Repositories
{
    public class EmployeeRepository
    {
        readonly AttendanceContext _dbContex = new AttendanceContext();

        public List<Employee> Get()
        {
            return _dbContex.Employees.OrderBy(_=>_.Branch.Name)
                .ThenBy(_=>_.Department.Name).ThenBy(_=>_.Shift.Name)
                .ThenBy(_=>_.Name).ToList();
        }

        public Employee Get(int id)
        {
            return _dbContex.Employees.Find(id);
        }

        public Employee Create(Employee employee)
        {
            _dbContex.Employees.Add(employee);
            _dbContex.SaveChanges();
            return employee;
        }

        public bool Delete(int id)
        {
            var employee = _dbContex.Employees.Find(id);
            if (employee == null) return false;
            _dbContex.Employees.Remove(employee);
            _dbContex.SaveChanges();
            return true;
        }

        public Employee Edit(Employee employee)
        {
            _dbContex.Entry(employee).State = EntityState.Modified;
            _dbContex.SaveChanges();
            return employee;
        }
    }
}