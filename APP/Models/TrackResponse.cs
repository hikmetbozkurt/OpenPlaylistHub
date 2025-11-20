using APP.DataAccess.Entities;
using CORE.APP.Models;

namespace APP.Models
{
    public class TrackResponse : Response
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal Rating { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsFavorite { get; set; }
        public Genre Genre { get; set; }
    }
}

