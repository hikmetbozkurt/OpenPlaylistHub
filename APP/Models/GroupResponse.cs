using CORE.APP.Models;

namespace APP.Models
{
    public class GroupResponse : Response
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Guid { get; set; }
        public string Description { get; set; }
    }
}

