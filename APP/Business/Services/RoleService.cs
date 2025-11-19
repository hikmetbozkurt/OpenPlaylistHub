using APP.DataAccess.Context;
using APP.DataAccess.Entities;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using CORE.APP.Services.MVC;
using Microsoft.EntityFrameworkCore;

namespace APP.Business.Services
{
    public class RoleService : Service, IService<RoleRequest, RoleResponse>
    {
        private readonly Db _db;

        public RoleService(Db db)
        {
            _db = db;
        }

        public List<RoleResponse> GetAll()
        {
            return _db.Roles
                .Select(r => new RoleResponse
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description
                })
                .ToList();
        }

        public RoleResponse GetById(int id)
        {
            var role = _db.Roles.FirstOrDefault(r => r.Id == id);

            if (role == null)
                return null;

            return new RoleResponse
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };
        }

        public CommandResponse Create(RoleRequest request)
        {
            var role = new Role
            {
                Name = request.Name,
                Description = request.Description
            };

            _db.Roles.Add(role);
            _db.SaveChanges();

            return new CommandResponse();
        }

        public CommandResponse Update(RoleRequest request)
        {
            var role = _db.Roles.FirstOrDefault(r => r.Id == request.Id);
            if (role == null)
                return new CommandResponse();

            role.Name = request.Name;
            role.Description = request.Description;

            _db.SaveChanges();

            return new CommandResponse();
        }

        public CommandResponse Delete(int id)
        {
            var role = _db.Roles.FirstOrDefault(r => r.Id == id);
            if (role == null)
                return new CommandResponse();

            _db.Roles.Remove(role);
            _db.SaveChanges();

            return new CommandResponse();
        }
    }
}

