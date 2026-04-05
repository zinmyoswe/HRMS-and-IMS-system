using System;
using System.Collections.Generic;

namespace HRSystembackend.DTOs
{
    public class StaffDto
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
        public int? PositionId { get; set; }
        public int? ManagerId { get; set; }
        public bool IsActive { get; set; }
    }
}
