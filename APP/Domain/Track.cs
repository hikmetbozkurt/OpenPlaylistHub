namespace APP.Domain
{
    public class Track
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public int DurationSeconds { get; set; }
        public decimal Rating { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}

