namespace HRSystembackend.Models
{
    public class AllowanceType
    {
        public int AllowanceTypeId { get; set; }
        public string Name { get; set; } = null!;

        public System.Collections.Generic.ICollection<StaffAllowance>? StaffAllowances { get; set; }
    }
}
