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
    public class StaffsController : ControllerBase
    {
        private readonly HRDbContext _db;

        public StaffsController(HRDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaffDto>>> GetAll()
        {
            var items = await _db.Staffs
                .AsNoTracking()
                .Select(s => new StaffDto
                {
                    StaffId = s.StaffId,
                    StaffCode = s.StaffCode,
                    StaffName = s.StaffName,
                    Email = s.Email,
                    Phone = s.Phone,
                    Address = s.Address,
                    DateOfBirth = s.DateOfBirth,
                    JoinedDate = s.JoinedDate,
                    DepartmentId = s.DepartmentId,
                    PositionId = s.PositionId,
                    ManagerId = s.ManagerId,
                    IsActive = s.IsActive
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StaffDto>> Get(int id)
        {
            var s = await _db.Staffs.AsNoTracking().FirstOrDefaultAsync(x => x.StaffId == id);
            if (s == null) return NotFound();
            var dto = new StaffDto
            {
                StaffId = s.StaffId,
                StaffCode = s.StaffCode,
                StaffName = s.StaffName,
                Email = s.Email,
                Phone = s.Phone,
                Address = s.Address,
                DateOfBirth = s.DateOfBirth,
                JoinedDate = s.JoinedDate,
                DepartmentId = s.DepartmentId,
                PositionId = s.PositionId,
                ManagerId = s.ManagerId,
                IsActive = s.IsActive
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<StaffDto>> Create([FromBody] StaffDto input)
        {
            if (await _db.Staffs.AnyAsync(x => x.StaffCode == input.StaffCode)) return BadRequest("StaffCode already exists");
            if (await _db.Staffs.AnyAsync(x => x.Email == input.Email)) return BadRequest("Email already exists");

            var entity = new Staff
            {
                StaffCode = input.StaffCode,
                StaffName = input.StaffName,
                Email = input.Email,
                Phone = input.Phone,
                Address = input.Address,
                DateOfBirth = input.DateOfBirth,
                JoinedDate = input.JoinedDate,
                DepartmentId = input.DepartmentId,
                PositionId = input.PositionId,
                ManagerId = input.ManagerId,
                IsActive = input.IsActive,
                CreatedDate = System.DateTime.UtcNow,
                UpdatedDate = System.DateTime.UtcNow
            };

            _db.Staffs.Add(entity);
            await _db.SaveChangesAsync();

            input.StaffId = entity.StaffId;
            return CreatedAtAction(nameof(Get), new { id = entity.StaffId }, input);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] StaffDto input)
        {
            var existing = await _db.Staffs.FindAsync(id);
            if (existing == null) return NotFound();
            if (existing.StaffCode != input.StaffCode && await _db.Staffs.AnyAsync(x => x.StaffCode == input.StaffCode)) return BadRequest("StaffCode already exists");
            if (existing.Email != input.Email && await _db.Staffs.AnyAsync(x => x.Email == input.Email)) return BadRequest("Email already exists");

            existing.StaffCode = input.StaffCode;
            existing.StaffName = input.StaffName;
            existing.Email = input.Email;
            existing.Phone = input.Phone;
            existing.Address = input.Address;
            existing.DateOfBirth = input.DateOfBirth;
            existing.JoinedDate = input.JoinedDate;
            existing.DepartmentId = input.DepartmentId;
            existing.PositionId = input.PositionId;
            existing.ManagerId = input.ManagerId;
            existing.IsActive = input.IsActive;
            existing.UpdatedDate = System.DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _db.Staffs.FindAsync(id);
            if (existing == null) return NotFound();
            _db.Staffs.Remove(existing);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
