using CORE.APP.Models;

namespace APP.Models
{
    public class ArtistResponse : Response
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsBand { get; set; }
    }
}

