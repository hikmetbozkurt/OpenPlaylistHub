using CORE.APP.Models;

namespace APP.Models
{
    public class SongRequest : Request
    {

        public string Title { get; set; }
        public int? DurationSeconds { get; set; }
        public int? TotalStreams { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public int? AlbumId { get; set; }
        public int ArtistId { get; set; }

    }
}

