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
    public class SitesController : ControllerBase
    {
        private readonly HRDbContext _db;
        public SitesController(HRDbContext db) { _db = db; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Site>>> GetAll() => Ok(await _db.Sites.AsNoTracking().ToListAsync());

        [HttpPost]
        public async Task<ActionResult<Site>> Create(Site input)
        {
            _db.Sites.Add(input);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = input.SiteId }, input);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Site>> Get(int id)
        {
            var s = await _db.Sites.FindAsync(id);
            if (s == null) return NotFound();
            return Ok(s);
        }
    }
}
