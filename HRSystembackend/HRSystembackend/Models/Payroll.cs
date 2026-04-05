namespace HRSystembackend.Models
{
    public class Payroll
    {
        public int PayrollId { get; set; }
        public int StaffId { get; set; }
        public Staff? Staff { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }

        public int WorkingDays { get; set; }
        public int PresentDays { get; set; }
        public int AbsentDays { get; set; }
        public int LeaveDays { get; set; }

        public decimal TotalAllowance { get; set; }
        public decimal TotalDeduction { get; set; }

        public decimal GrossSalary { get; set; }
        public decimal NetSalary { get; set; }
    }
}
