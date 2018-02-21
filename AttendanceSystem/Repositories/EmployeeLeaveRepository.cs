using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AttendanceSystem.Models;
using AttendanceSystem.Models.Contexts;

namespace AttendanceSystem.Repositories
{
    public class EmployeeLeaveRepository 
    {
        readonly AttendanceContext _dbContex = new AttendanceContext();

        public List<EmployeeLeave> GetAll()
        {
            return _dbContex.EmployeeLeaves.ToList();
        }

        public EmployeeLeave Get(int id)
        {
            return _dbContex.EmployeeLeaves.Find(id);
        }

        public EmployeeLeave Create(EmployeeLeave employeeLeave)
        {
            _dbContex.EmployeeLeaves.Add(employeeLeave);
            _dbContex.SaveChanges();
            return employeeLeave;
        }

        public IQueryable<EmployeeLeave> FindBy(System.Linq.Expressions.Expression<Func<EmployeeLeave, bool>> predicate)
        {
            IQueryable<EmployeeLeave> query = _dbContex.Set<EmployeeLeave>().Where(predicate);
            return query;
        }
    }
}