using System.Collections.Generic;

namespace HRSystembackend.Models
{
    public class Site
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; } = null!;
        public string SiteType { get; set; } = null!; // Office, Customer, Factory
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public ICollection<Attendance>? Attendances { get; set; }
    }
}
