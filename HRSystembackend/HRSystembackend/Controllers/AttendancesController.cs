using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HRSystembackend.Data;
using HRSystembackend.Models;

namespace HRSystembackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendancesController : ControllerBase
    {
        private readonly HRDbContext _db;
        public AttendancesController(HRDbContext db) { _db = db; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAll() => Ok(await _db.Attendances.Include(a=>a.Logs).AsNoTracking().ToListAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Attendance>> Get(int id)
        {
            var a = await _db.Attendances.Include(x=>x.Logs).FirstOrDefaultAsync(x => x.AttendanceId == id);
            if (a == null) return NotFound();
            return Ok(a);
        }

        [HttpPost]
        public async Task<ActionResult<Attendance>> Create(Attendance input)
        {
            _db.Attendances.Add(input);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = input.AttendanceId }, input);
        }

        [HttpPost("{attendanceId}/logs")]
        public async Task<ActionResult<AttendanceLog>> AddLog(int attendanceId, [FromBody] AttendanceLog log)
        {
            var att = await _db.Attendances.FindAsync(attendanceId);
            if (att == null) return NotFound();
            log.AttendanceId = attendanceId;
            _db.AttendanceLogs.Add(log);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = attendanceId }, log);
        }
    }
}
