using CORE.APP.Domain;

namespace APP.DataAccess.Entities
{
    public class Artist : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsBand { get; set; }
        public ICollection<Album> Albums { get; set; }
        public ICollection<Song> Songs { get; set; }
    }
}
