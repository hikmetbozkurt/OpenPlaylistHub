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
                    Title = g.Title
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
                    Title = g.Title
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
                Title = group.Title
            };
        }

        public CommandResponse Create(GroupRequest request)
        {
            if (_db.Groups.Any(g => g.Title == request.Title))
            {
                return Error("Group with the same title already exists.");
            }

            var entity = new Group
            {
                Title = request.Title
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

            if (_db.Groups.Any(g => g.Id != request.Id && g.Title == request.Title))
            {
                return Error("Group with the same title already exists.");
            }

            entity.Title = request.Title;

            Update(entity);

            return Success("Group updated successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {


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
