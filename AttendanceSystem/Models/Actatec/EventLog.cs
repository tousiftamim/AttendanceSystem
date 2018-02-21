using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AttendanceSystem.Models.Actatec
{
    [Table("access_event_logs")]
    public class EventLog
    {
        public Int64 id { get; set; }
        public string USERID { get; set; }

        public string ACCESSMETHOD { get; set; }
        public DateTime LOCAL_TIMESTAMP { get; set; }
    }
}