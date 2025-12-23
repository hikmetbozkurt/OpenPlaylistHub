namespace APP.DataAccess.Entities
{
    public class PlaylistSong
    {
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
        public int SongId { get; set; }
        public Song Song { get; set; }
        public short? OrderNo { get; set; }
    }
}

