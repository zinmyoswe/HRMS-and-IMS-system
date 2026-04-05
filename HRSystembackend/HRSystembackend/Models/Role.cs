using System.Collections.Generic;

namespace HRSystembackend.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;

        public ICollection<StaffRole>? StaffRoles { get; set; }
    }
}
