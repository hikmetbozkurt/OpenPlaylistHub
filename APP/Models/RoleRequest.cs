using CORE.APP.Models;

namespace APP.Models
{
    public class RoleRequest : Request
    {

        public string Name { get; set; }
        public string Description { get; set; }
    }
}

