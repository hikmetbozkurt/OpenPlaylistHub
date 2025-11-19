using CORE.APP.Models;

namespace APP.Models
{
    public class PlaylistRequest : Request
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
    }
}

