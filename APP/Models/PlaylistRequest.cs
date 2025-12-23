using CORE.APP.Models;

namespace APP.Models
{
    public class PlaylistRequest : Request
    {

        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public int OwnerUserId { get; set; }
        public List<int> SongIds { get; set; } = new List<int>();
        public List<int> MemberUserIds { get; set; } = new List<int>();
    }
}

