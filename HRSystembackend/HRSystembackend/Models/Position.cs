using System.Collections.Generic;

namespace HRSystembackend.Models
{
    public class Position
    {
        public int PositionId { get; set; }
        public string PositionName { get; set; } = null!;

        public ICollection<Staff>? Staffs { get; set; }
    }
}
