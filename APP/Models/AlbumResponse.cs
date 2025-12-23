using CORE.APP.Models;

namespace APP.Models
{
    public class AlbumResponse : Response
    {

        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ArtistId { get; set; }
        public string ArtistName { get; set; }
    }
}

