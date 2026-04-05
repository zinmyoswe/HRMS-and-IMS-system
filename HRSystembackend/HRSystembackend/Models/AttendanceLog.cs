using System;

namespace HRSystembackend.Models
{
    public class AttendanceLog
    {
        public int LogId { get; set; }
        public int AttendanceId { get; set; }
        public Attendance? Attendance { get; set; }
        public string LogType { get; set; } = null!; // IN / OUT
        public DateTime LogTime { get; set; }
        public decimal? Distance { get; set; }
    }
}
