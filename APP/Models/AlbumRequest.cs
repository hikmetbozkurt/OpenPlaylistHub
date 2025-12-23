using CORE.APP.Models;

namespace APP.Models
{
    public class AlbumRequest : Request
    {

        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ArtistId { get; set; }
    }
}

