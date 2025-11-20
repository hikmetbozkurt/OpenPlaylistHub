using CORE.APP.Domain;

namespace APP.DataAccess.Entities
{
    public class User : Entity
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime BirthDate { get; set; }
        public int? GroupId { get; set; }
        public Group Group { get; set; }
        public ICollection<Playlist> Playlists { get; set; }
    }
}
