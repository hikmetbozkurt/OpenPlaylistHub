using CORE.APP.Models;

namespace APP.Models
{
    public class PlaylistResponse : Response
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Guid { get; set; }
        public List<TrackResponse> Tracks { get; set; } = new List<TrackResponse>();
    }
}

