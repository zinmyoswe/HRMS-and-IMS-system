using System;

namespace HRSystembackend.Models
{
    public class SalaryStructure
    {
        public int SalaryStructureId { get; set; }
        public int StaffId { get; set; }
        public Staff? Staff { get; set; }

        public decimal BasicSalary { get; set; }
        public decimal PhoneAllowance { get; set; }
        public decimal MealAllowance { get; set; }
        public decimal HouseAllowance { get; set; }
        public decimal TaxRate { get; set; } // percent values like 3 or 5
        public decimal SocialSecurity { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
