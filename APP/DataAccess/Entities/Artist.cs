using CORE.APP.Domain;

namespace APP.DataAccess.Entities
{
    public class Artist : Entity
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
