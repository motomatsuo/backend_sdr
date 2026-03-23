using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.Modelos
{
    public class ActivityLog
    {
        public int ActivityLogId { get; private set; }
        public DateTime Register { get; private set; }
        public string Log { get; private set; }

        public ActivityLog(int activityLogId, DateTime register, string log)
        {
            ActivityLogId = activityLogId;
            Register = register;
            Log = log;
        }
    }
}
