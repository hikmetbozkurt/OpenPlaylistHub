using CORE.APP.Domain;

namespace APP.DataAccess.Entities
{
    public class Playlist : Entity
    {
        public string Name { get; set; }
        public int OwnerUserId { get; set; }
        public User OwnerUser { get; set; } // Added navigation property
        public bool IsPublic { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<PlaylistSong> PlaylistSongs { get; set; }
        public ICollection<PlaylistMember> PlaylistMembers { get; set; }
    }
}
