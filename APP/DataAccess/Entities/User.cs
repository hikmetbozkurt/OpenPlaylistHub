using CORE.APP.Domain;

namespace APP.DataAccess.Entities
{
    public class User : Entity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Genders Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsActive { get; set; }


        public ICollection<Playlist> Playlists { get; set; }

        public ICollection<PlaylistMember> PlaylistMembers { get; set; }
        public ICollection<SongRating> SongRatings { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
