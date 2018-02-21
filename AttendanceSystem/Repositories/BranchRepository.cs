using System.Collections.Generic;
using System.Data;
using System.Linq;
using AttendanceSystem.Models;
using AttendanceSystem.Models.Contexts;

namespace AttendanceSystem.Repositories
{
    public class BranchRepository
    {
        readonly AttendanceContext _dbContex = new AttendanceContext();

        public List<Branch> Get()
        {
            return _dbContex.Branches.OrderBy(_=>_.Name).ToList();
        }

        public Branch Get(int id)
        {
            return _dbContex.Branches.Find(id);
        }

        public Branch Create(Branch branch)
        {
            _dbContex.Branches.Add(branch);
            _dbContex.SaveChanges();
            return branch;
        }

        public bool Delete(int id)
        {
            var employees = _dbContex.Employees.Select(_ => _.BranchId == id).ToList();
            if (employees.Count > 0) return false;
            var branch = _dbContex.Branches.Find(id);
            if (branch == null) return false;
            _dbContex.Branches.Remove(branch);
            _dbContex.SaveChanges();
            return true;
        }

        public Branch Edit(Branch branch)
        {
            _dbContex.Entry(branch).State = EntityState.Modified;
            _dbContex.SaveChanges();
            return branch;
        }
    }
}