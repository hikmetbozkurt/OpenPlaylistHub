using CORE.APP.Models;

namespace APP.Models
{
    public class GroupRequest : Request
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

