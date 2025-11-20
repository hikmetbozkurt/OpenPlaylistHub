using CORE.APP.Domain;

namespace APP.DataAccess.Entities
{
    public class Group : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
