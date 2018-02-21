using System.Collections.Generic;
using System.Data;
using System.Linq;
using AttendanceSystem.Models;
using AttendanceSystem.Models.Contexts;

namespace AttendanceSystem.Repositories
{
    public class ShiftRepository
    {
        readonly AttendanceContext _dbContex = new AttendanceContext();

        public List<Shift> Get()
        {
            return _dbContex.Shifts.OrderBy(_=>_.Name).ToList();
        }

        public Shift Get(int id)
        {
            return _dbContex.Shifts.Find(id);
        }

        public Shift Create(Shift shift)
        {
            _dbContex.Shifts.Add(shift);
            _dbContex.SaveChanges();
            return shift;
        }

        public bool Delete(int id)
        {
            var employees = _dbContex.Employees.Select(_ => _.ShiftId == id).ToList();
            if (employees.Count > 0) return false;
            var shift = _dbContex.Shifts.Find(id);
            if (shift == null) return false;
            _dbContex.Shifts.Remove(shift);
            _dbContex.SaveChanges();
            return true;
        }

        public Shift Edit(Shift shift)
        {
            _dbContex.Entry(shift).State = EntityState.Modified;
            _dbContex.SaveChanges();
            return shift;
        }
    }
}