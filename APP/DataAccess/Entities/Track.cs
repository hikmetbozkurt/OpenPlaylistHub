using CORE.APP.Domain;

namespace APP.DataAccess.Entities
{
    public enum Genre
    {
        Pop,
        Rock,
        Jazz,
        Classical,
        Electronic,
        HipHop,
        Country,
        Blues
    }

    public class Track : Entity
    {
        public string Title { get; set; }
        public string Album { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal Rating { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsFavorite { get; set; }
        public Genre Genre { get; set; }
        public ICollection<PlaylistTrack> PlaylistTracks { get; set; }
    }
}
