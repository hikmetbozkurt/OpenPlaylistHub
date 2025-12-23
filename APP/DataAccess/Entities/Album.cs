using CORE.APP.Domain;

namespace APP.DataAccess.Entities
{
    public class Album : Entity
    {
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }
        public ICollection<Song> Songs { get; set; }
    }
}

