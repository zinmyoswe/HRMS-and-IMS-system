using System;
using System.Collections.Generic;

namespace HRSystembackend.Models
{
    public class Staff
    {
        public int StaffId { get; set; }
        public string StaffCode { get; set; } = null!;
        public string StaffName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime JoinedDate { get; set; }
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public int? PositionId { get; set; }
        public Position? Position { get; set; }
        public int? ManagerId { get; set; }
        public Staff? Manager { get; set; }
        public ICollection<Staff>? Subordinates { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        // Navigation
        public ICollection<StaffRole>? StaffRoles { get; set; }
        public ICollection<Attendance>? Attendances { get; set; }
        public ICollection<SalaryStructure>? SalaryStructures { get; set; }
        public ICollection<Payroll>? Payrolls { get; set; }
        public ICollection<StaffAllowance>? StaffAllowances { get; set; }
        // Monitor navigation removed (monitor-related files deleted)
    }
}
