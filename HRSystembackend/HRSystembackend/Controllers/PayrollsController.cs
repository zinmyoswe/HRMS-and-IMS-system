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
    public class PayrollsController : ControllerBase
    {
        private readonly HRDbContext _db;
        public PayrollsController(HRDbContext db) { _db = db; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payroll>>> GetAll() => Ok(await _db.Payrolls.AsNoTracking().ToListAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Payroll>> Get(int id)
        {
            var p = await _db.Payrolls.FindAsync(id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        [HttpPost]
        public async Task<ActionResult<Payroll>> Create(Payroll input)
        {
            _db.Payrolls.Add(input);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = input.PayrollId }, input);
        }
    }
}
