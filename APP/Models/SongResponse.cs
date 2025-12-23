using CORE.APP.Models;

namespace APP.Models
{
    public class SongResponse : Response
    {


        public string Title { get; set; }
        public int? DurationSeconds { get; set; }
        public string DurationFormatted => DurationSeconds.HasValue ? TimeSpan.FromSeconds(DurationSeconds.Value).ToString(@"mm\:ss") : string.Empty;
        public int? TotalStreams { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public int? AlbumId { get; set; }
        public string AlbumTitle { get; set; } // Renamed from Name

        public int ArtistId { get; set; }
        public string ArtistName { get; set; } // Full name constructed

        public List<int> GenreIds { get; set; } // Maybe IDs are enough for list, or Names
        public string GenreNames { get; set; } // Comma separated for display
        
        public double AverageRating { get; set; } // Calculated from SongRatings
    }
}

