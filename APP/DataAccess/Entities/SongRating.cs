using System;

namespace APP.DataAccess.Entities
{
    public class SongRating
    {
        public int SongId { get; set; }
        public Song Song { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public byte Rating { get; set; } // 1-5
        public DateTime RatedAt { get; set; }
    }
}
