namespace Student_Society_Management.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int MemberId { get; set; }
        public bool Attended { get; set; }
    }
}
