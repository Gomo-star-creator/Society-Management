
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Society_Management.Data;
using Student_Society_Management.Models;

namespace Student_Society_Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly SocietyDbContext _context;
        public MembersController(SocietyDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers() =>
            await _context.Members.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            return member == null ? NotFound() : member;
        }

        [HttpPost]
        public async Task<ActionResult<Member>> CreateMember(Member member)
        {
            _context.Members.Add(member);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMember), new { id = member.Id }, member);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(int id, Member member)
        {
            if (id != member.Id) return BadRequest();
            _context.Entry(member).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null) return NotFound();
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
