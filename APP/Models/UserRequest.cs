using CORE.APP.Models;

namespace APP.Models
{
    public class UserRequest : Request
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime BirthDate { get; set; }
        public int? GroupId { get; set; }
    }
}

