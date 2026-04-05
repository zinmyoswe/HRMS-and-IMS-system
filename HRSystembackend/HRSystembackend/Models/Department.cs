using System.Collections.Generic;

namespace HRSystembackend.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;

        public ICollection<Staff>? Staffs { get; set; }
    }
}
