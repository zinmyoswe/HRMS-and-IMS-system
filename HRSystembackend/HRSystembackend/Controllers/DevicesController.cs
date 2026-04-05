using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HRSystembackend.Data;
using HRSystembackend.Models;
using HRSystembackend.DTOs;

namespace HRSystembackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly HRDbContext _db;

        public DevicesController(HRDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceDto>>> GetAll()
        {
            var items = await _db.Devices
                .AsNoTracking()
                .Select(d => new DeviceDto
                {
                    DeviceId = d.DeviceId,
                    Dept = d.Dept,
                    DeviceType = d.DeviceType,
                    Brand = d.Brand,
                    Model = d.Model,
                    FixedAssets = d.FixedAssets,
                    GreenLabel = d.GreenLabel,
                    DeviceName = d.DeviceName,
                    SerialNumber = d.SerialNumber,
                    MACAddress = d.MACAddress,
                    IPAddress = d.IPAddress,
                    Remark1 = d.Remark1,
                    Remark2 = d.Remark2,
                    CreatedAt = d.CreatedAt
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceDto>> Get(int id)
        {
            var d = await _db.Devices.AsNoTracking().FirstOrDefaultAsync(x => x.DeviceId == id);
            if (d == null) return NotFound();
            var dto = new DeviceDto
            {
                DeviceId = d.DeviceId,
                Dept = d.Dept,
                DeviceType = d.DeviceType,
                Brand = d.Brand,
                Model = d.Model,
                FixedAssets = d.FixedAssets,
                GreenLabel = d.GreenLabel,
                DeviceName = d.DeviceName,
                SerialNumber = d.SerialNumber,
                MACAddress = d.MACAddress,
                IPAddress = d.IPAddress,
                Remark1 = d.Remark1,
                Remark2 = d.Remark2,
                CreatedAt = d.CreatedAt
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<DeviceDto>> Create([FromBody] DeviceDto input)
        {
            var entity = new Device
            {
                Dept = input.Dept,
                DeviceType = input.DeviceType,
                Brand = input.Brand,
                Model = input.Model,
                FixedAssets = input.FixedAssets,
                GreenLabel = input.GreenLabel,
                DeviceName = input.DeviceName,
                SerialNumber = input.SerialNumber,
                MACAddress = input.MACAddress,
                IPAddress = input.IPAddress,
                Remark1 = input.Remark1,
                Remark2 = input.Remark2,
                CreatedAt = System.DateTime.UtcNow
            };

            _db.Devices.Add(entity);
            await _db.SaveChangesAsync();

            input.DeviceId = entity.DeviceId;
            input.CreatedAt = entity.CreatedAt;
            return CreatedAtAction(nameof(Get), new { id = entity.DeviceId }, input);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DeviceDto input)
        {
            var existing = await _db.Devices.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Dept = input.Dept;
            existing.DeviceType = input.DeviceType;
            existing.Brand = input.Brand;
            existing.Model = input.Model;
            existing.FixedAssets = input.FixedAssets;
            existing.GreenLabel = input.GreenLabel;
            existing.DeviceName = input.DeviceName;
            existing.SerialNumber = input.SerialNumber;
            existing.MACAddress = input.MACAddress;
            existing.IPAddress = input.IPAddress;
            existing.Remark1 = input.Remark1;
            existing.Remark2 = input.Remark2;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _db.Devices.FindAsync(id);
            if (existing == null) return NotFound();
            _db.Devices.Remove(existing);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
