using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Society_Management.Data;
using Student_Society_Management.Models;

namespace Student_Society_Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly SocietyDbContext _context;
        public EventsController(SocietyDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents() =>
            await _context.Events.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            return ev == null ? NotFound() : ev;
        }

        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent(Event ev)
        {
            _context.Events.Add(ev);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEvent), new { id = ev.Id }, ev);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, Event ev)
        {
            if (id != ev.Id) return BadRequest();
            _context.Entry(ev).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null) return NotFound();
            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
