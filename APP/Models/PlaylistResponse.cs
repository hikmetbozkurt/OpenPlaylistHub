using CORE.APP.Models;

namespace APP.Models
{
    public class PlaylistResponse : Response
    {

        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedDate { get; set; }
        public int OwnerUserId { get; set; }
        public string OwnerUserName { get; set; }

        public List<SongResponse> Songs { get; set; } = new List<SongResponse>();
        public List<string> MemberUserNames { get; set; } = new List<string>(); // For PlaylistMembers
    }
}

