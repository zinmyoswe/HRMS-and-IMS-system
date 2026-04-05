using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HRSystembackend.Data;
using HRSystembackend.Models;

namespace HRSystembackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly HRDbContext _db;
        public DepartmentsController(HRDbContext db) { _db = db; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetAll() => Ok(await _db.Departments.AsNoTracking().ToListAsync());

        [HttpPost]
        public async Task<ActionResult<Department>> Create(Department input)
        {
            _db.Departments.Add(input);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = input.DepartmentId }, input);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> Get(int id)
        {
            var d = await _db.Departments.FindAsync(id);
            if (d == null) return NotFound();
            return Ok(d);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Department input)
        {
            var d = await _db.Departments.FindAsync(id);
            if (d == null) return NotFound();
            d.DepartmentName = input.DepartmentName;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var d = await _db.Departments.FindAsync(id);
            if (d == null) return NotFound();
            _db.Departments.Remove(d);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
