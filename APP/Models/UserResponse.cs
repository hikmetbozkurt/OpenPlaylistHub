using CORE.APP.Models;

namespace APP.Models
{
    public class UserResponse : Response
    {


        public string UserName { get; set; }
        // Email removed
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; } // Display as string

        public bool IsActive { get; set; }
        public string IsActiveFormatted { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthDateFormatted { get; set; }

    }
}

