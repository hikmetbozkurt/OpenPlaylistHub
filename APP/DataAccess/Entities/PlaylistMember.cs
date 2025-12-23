namespace APP.DataAccess.Entities
{
    public class PlaylistMember
    {
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public bool IsEditor { get; set; }
    }
}
