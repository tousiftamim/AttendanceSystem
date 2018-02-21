using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using AttendanceSystem.Models;
using AttendanceSystem.Models.Contexts;
using iTextSharp.text.pdf.qrcode;

namespace AttendanceSystem.Repositories
{
    public class LeaveCountRepository
    {
        readonly AttendanceContext _dbContex = new AttendanceContext();

        public List<LeaveCount> GetAll()
        {
            return _dbContex.LeaveCounts.Include("Employee").ToList();
        }

        public LeaveCount Get(int id)
        {
            return _dbContex.LeaveCounts.Find(id);
        }

        public LeaveCount Create(LeaveCount leaveCount)
        {
            _dbContex.LeaveCounts.Add(leaveCount);
            _dbContex.SaveChanges();
            return leaveCount;
        }

        public IQueryable<LeaveCount> FindBy(System.Linq.Expressions.Expression<Func<LeaveCount, bool>> predicate)
        {
            IQueryable<LeaveCount> query = _dbContex.Set<LeaveCount>().Where(predicate);
            return query;
        }

        public List<LeaveCount> GetLeaveCount(IQueryable<Attendance> query)
        {
            var employeeGroup = query.GroupBy(_ => _.Employee.Id);

            var leaveCount = (from l in employeeGroup.ToList()
                join el in _dbContex.EmployeeLeaves on l.Key equals el.EmployeeId
                join lc in _dbContex.LeaveCounts on l.Key equals lc.EmployeeId
                group el by el.EmployeeId into g2
                let firstOrDefault = _dbContex.LeaveCounts.FirstOrDefault(lc => lc.EmployeeId == g2.Key)
                where firstOrDefault != null
                select new LeaveCount
                {
                    EmployeeId = g2.Key,
                    CasualLeave =  firstOrDefault.CasualLeave - g2.GroupBy(x => x.LeaveType).Where(x => x.Key == (int)LeaveTypeEnum.CasualLeave).Select(x => x.Sum(d => d.Duration)).FirstOrDefault(),
                    EarnLeave = firstOrDefault.EarnLeave - g2.GroupBy(x => x.LeaveType).Where(x => x.Key == (int)LeaveTypeEnum.EarnLeave).Select(x => x.Sum(d => d.Duration)).FirstOrDefault(),
                    SickLeave = firstOrDefault.SickLeave - g2.GroupBy(x => x.LeaveType).Where(x => x.Key == (int)LeaveTypeEnum.SickLeave).Select(x => x.Sum(d => d.Duration)).FirstOrDefault()
                }).ToList();

            return leaveCount;
        }
    }
}