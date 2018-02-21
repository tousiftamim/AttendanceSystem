using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Linq;
using AttendanceSystem.Models;
using AttendanceSystem.Models.Actatec;
using AttendanceSystem.Models.Contexts;

namespace AttendanceSystem.Repositories
{
    public class EventLogRepository
    {
        private readonly ActatecContext _actatecContext = new ActatecContext();

        public List<EventLog> Get()
        {
            return _actatecContext.EventLogs.Where(_ => _.ACCESSMETHOD == "FP").ToList();
        }

        public List<EventLog> GetTodaysLog()
        {
            var today = DateTime.Today;
            return _actatecContext.EventLogs.Where(_ => _.ACCESSMETHOD == "FP"
                && EntityFunctions.TruncateTime(_.LOCAL_TIMESTAMP) == today.Date).ToList();
        }

        public List<EventLog> GetLogByDate(DateTime date)
        {
            return _actatecContext.EventLogs.Where(_ => _.ACCESSMETHOD == "FP"
                && EntityFunctions.TruncateTime(_.LOCAL_TIMESTAMP) == date.Date).ToList();
        }

        public List<EventLog> Get(DateTime from, DateTime to)
        {
            return _actatecContext.EventLogs.Where(_ => _.ACCESSMETHOD == "FP" &&
                EntityFunctions.TruncateTime(_.LOCAL_TIMESTAMP) >= from && 
                EntityFunctions.TruncateTime(_.LOCAL_TIMESTAMP) <= to).ToList();
        }
    }
}