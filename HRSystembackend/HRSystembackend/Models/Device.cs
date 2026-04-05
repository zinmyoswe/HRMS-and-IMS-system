using System;

namespace HRSystembackend.Models
{
    public class Device
    {
        public int DeviceId { get; set; }
        public string? Dept { get; set; }
        public string? DeviceType { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? FixedAssets { get; set; }
        public string? GreenLabel { get; set; }
        public string? DeviceName { get; set; }
        public string? SerialNumber { get; set; }
        public string? MACAddress { get; set; }
        public string? IPAddress { get; set; }
        public string? Remark1 { get; set; }
        public string? Remark2 { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
