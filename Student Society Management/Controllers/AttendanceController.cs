using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Society_Management.Data;
using Student_Society_Management.Models;

namespace Student_Society_Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly SocietyDbContext _context;
        public AttendanceController(SocietyDbContext context) => _context = context;

        [HttpGet("event/{eventId}")]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetForEvent(int eventId) =>
            await _context.Attendances.Where(a => a.EventId == eventId).ToListAsync();

        [HttpPost]
        public async Task<ActionResult<Attendance>> MarkAttendance(Attendance attendance)
        {
            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
            return Ok(attendance);
        }

        public class SetAttendanceRequest
        {
            public int EventId { get; set; }
            public int MemberId { get; set; }
            public bool Attended { get; set; }
        }

        [HttpPost("mine")]
        public async Task<IActionResult> SetMyAttendance(SetAttendanceRequest request)
        {
            var existing = await _context.Attendances
                .FirstOrDefaultAsync(a => a.EventId == request.EventId && a.MemberId == request.MemberId);

            if (existing == null)
            {
                _context.Attendances.Add(new Attendance
                {
                    EventId = request.EventId,
                    MemberId = request.MemberId,
                    Attended = request.Attended
                });
            }
            else
            {
                existing.Attended = request.Attended;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
