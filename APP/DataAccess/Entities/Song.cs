using CORE.APP.Domain;

namespace APP.DataAccess.Entities
{
    public class Song : Entity
    {
        public string Title { get; set; }
        public int? DurationSeconds { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? TotalStreams { get; set; }
        public int? AlbumId { get; set; }
        public Album Album { get; set; } // ArtistId is needed too
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }
        public ICollection<PlaylistSong> PlaylistSongs { get; set; }
        public ICollection<SongGenre> SongGenres { get; set; }
        public ICollection<SongRating> SongRatings { get; set; }
    }
}

