namespace Student_Society_Management.Models
{
    public class Member
    {
        public int Id { get; set; } 
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string StudentNumber { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; }
        public int? UserId { get; set; }
    }
}
