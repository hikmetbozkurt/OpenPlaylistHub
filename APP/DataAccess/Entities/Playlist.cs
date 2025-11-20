using CORE.APP.Domain;

namespace APP.DataAccess.Entities
{
    public class Playlist : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<PlaylistTrack> PlaylistTracks { get; set; }
    }
}
