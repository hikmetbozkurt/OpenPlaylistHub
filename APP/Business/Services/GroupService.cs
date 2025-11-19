using APP.DataAccess.Context;
using APP.DataAccess.Entities;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using CORE.APP.Services.MVC;
using Microsoft.EntityFrameworkCore;

namespace APP.Business.Services
{
    public class GroupService : Service, IService<GroupRequest, GroupResponse>
    {
        private readonly Db _db;

        public GroupService(Db db)
        {
            _db = db;
        }

        public List<GroupResponse> GetAll()
        {
            return _db.Groups
                .Select(g => new GroupResponse
                {
                    Id = g.Id,
                    Name = g.Name,
                    Description = g.Description
                })
                .ToList();
        }

        public GroupResponse GetById(int id)
        {
            var group = _db.Groups.FirstOrDefault(g => g.Id == id);

            if (group == null)
                return null;

            return new GroupResponse
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description
            };
        }

        public CommandResponse Create(GroupRequest request)
        {
            var group = new Group
            {
                Name = request.Name,
                Description = request.Description
            };

            _db.Groups.Add(group);
            _db.SaveChanges();

            return new CommandResponse();
        }

        public CommandResponse Update(GroupRequest request)
        {
            var group = _db.Groups.FirstOrDefault(g => g.Id == request.Id);
            if (group == null)
                return new CommandResponse();

            group.Name = request.Name;
            group.Description = request.Description;

            _db.SaveChanges();

            return new CommandResponse();
        }

        public CommandResponse Delete(int id)
        {
            var group = _db.Groups.FirstOrDefault(g => g.Id == id);
            if (group == null)
                return new CommandResponse();

            _db.Groups.Remove(group);
            _db.SaveChanges();

            return new CommandResponse();
        }
    }
}

