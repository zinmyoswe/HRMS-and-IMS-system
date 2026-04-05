using System;

namespace HRSystembackend.Models
{
    public class StaffAllowance
    {
        public int StaffAllowanceId { get; set; }
        public int StaffId { get; set; }
        public Staff? Staff { get; set; }
        public int AllowanceTypeId { get; set; }
        public AllowanceType? AllowanceType { get; set; }
        public decimal Amount { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
