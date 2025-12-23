using CORE.APP.Models;

namespace APP.Models
{
    public class UserRequest : Request
    {

        public string UserName { get; set; }
        // Email removed as per schema strictness
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public APP.DataAccess.Entities.Genders Gender { get; set; } // Use full name or using

        public bool IsActive { get; set; }
        public DateTime? BirthDate { get; set; }

    }
}

