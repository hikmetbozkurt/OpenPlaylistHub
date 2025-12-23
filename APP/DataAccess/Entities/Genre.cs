using CORE.APP.Domain;

namespace APP.DataAccess.Entities
{
    public class Genre : Entity
    {
        public string Name { get; set; }
        public ICollection<SongGenre> SongGenres { get; set; }
    }
}

