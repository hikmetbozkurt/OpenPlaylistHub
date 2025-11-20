using APP.DataAccess.Context;
using APP.DataAccess.Entities;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using CORE.APP.Services.MVC;
using Microsoft.EntityFrameworkCore;

namespace APP.Business.Services
{
    public class GroupService : Service<Group>, IService<GroupRequest, GroupResponse>
    {
        private readonly Db _db;

        public GroupService(Db db) : base(db)
        {
            _db = db;
        }

        public List<GroupResponse> List()
        {
            return Query()
                .Select(g => new GroupResponse
                {
                    Id = g.Id,
                    Guid = g.Guid,
                    Name = g.Name,
                    Description = g.Description
                })
                .ToList();
        }

        public GroupResponse Item(int id)
        {
            return Query()
                .Where(g => g.Id == id)
                .Select(g => new GroupResponse
                {
                    Id = g.Id,
                    Guid = g.Guid,
                    Name = g.Name,
                    Description = g.Description
                })
                .SingleOrDefault();
        }

        public GroupRequest Edit(int id)
        {
            var group = Query().FirstOrDefault(g => g.Id == id);
            if (group == null)
            {
                return null;
            }

            return new GroupRequest
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description
            };
        }

        public CommandResponse Create(GroupRequest request)
        {
            if (_db.Groups.Any(g => g.Name == request.Name))
            {
                return Error("Group with the same name already exists.");
            }

            var entity = new Group
            {
                Name = request.Name,
                Description = request.Description
            };

            Create(entity);

            return Success("Group created successfully.", entity.Id);
        }

        public CommandResponse Update(GroupRequest request)
        {
            var entity = Query(false).FirstOrDefault(g => g.Id == request.Id);
            if (entity == null)
            {
                return Error("Group not found!");
            }

            if (_db.Groups.Any(g => g.Id != request.Id && g.Name == request.Name))
            {
                return Error("Group with the same name already exists.");
            }

            entity.Name = request.Name;
            entity.Description = request.Description;

            Update(entity);

            return Success("Group updated successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            if (_db.Users.Any(u => u.GroupId == id))
            {
                return Error("Group cannot be deleted while it is assigned to users.");
            }

            var entity = Query(false).FirstOrDefault(g => g.Id == id);
            if (entity == null)
            {
                return Error("Group not found!");
            }

            Delete(entity);

            return Success("Group deleted successfully.", id);
        }
    }
}
