using System;
using System.Collections.Generic;

namespace HRSystembackend.Models
{
    public class Attendance
    {
        public int AttendanceId { get; set; }
        public int StaffId { get; set; }
        public Staff? Staff { get; set; }
        public DateTime WorkDate { get; set; }
        public int? SiteId { get; set; }
        public Site? Site { get; set; }

        public ICollection<AttendanceLog>? Logs { get; set; }
    }
}
