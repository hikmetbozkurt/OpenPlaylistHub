using CORE.APP.Models;

namespace APP.Models
{
    public class UserResponse : Response
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime BirthDate { get; set; }
        public int? GroupId { get; set; }
        public string GroupName { get; set; }
    }
}

