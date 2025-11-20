using APP.DataAccess.Context;
using APP.DataAccess.Entities;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using CORE.APP.Services.MVC;
using Microsoft.EntityFrameworkCore;

namespace APP.Business.Services
{
    public class RoleService : Service<Role>, IService<RoleRequest, RoleResponse>
    {
        private readonly Db _db;

        public RoleService(Db db) : base(db)
        {
            _db = db;
        }

        public List<RoleResponse> List()
        {
            return Query()
                .Select(r => new RoleResponse
                {
                    Id = r.Id,
                    Guid = r.Guid,
                    Name = r.Name,
                    Description = r.Description
                })
                .ToList();
        }

        public RoleResponse Item(int id)
        {
            return Query()
                .Where(r => r.Id == id)
                .Select(r => new RoleResponse
                {
                    Id = r.Id,
                    Guid = r.Guid,
                    Name = r.Name,
                    Description = r.Description
                })
                .SingleOrDefault();
        }

        public RoleRequest Edit(int id)
        {
            var role = Query().FirstOrDefault(r => r.Id == id);
            if (role == null)
            {
                return null;
            }

            return new RoleRequest
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };
        }

        public CommandResponse Create(RoleRequest request)
        {
            if (_db.Roles.Any(r => r.Name == request.Name))
            {
                return Error("Role with the same name already exists.");
            }

            var entity = new Role
            {
                Name = request.Name,
                Description = request.Description
            };

            Create(entity);

            return Success("Role created successfully.", entity.Id);
        }

        public CommandResponse Update(RoleRequest request)
        {
            var entity = Query(false).FirstOrDefault(r => r.Id == request.Id);
            if (entity == null)
            {
                return Error("Role not found!");
            }

            if (_db.Roles.Any(r => r.Id != request.Id && r.Name == request.Name))
            {
                return Error("Role with the same name already exists.");
            }

            entity.Name = request.Name;
            entity.Description = request.Description;

            Update(entity);

            return Success("Role updated successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false).FirstOrDefault(r => r.Id == id);
            if (entity == null)
            {
                return Error("Role not found!");
            }

            if (_db.UserRoles.Any(ur => ur.RoleId == id))
            {
                return Error("Role cannot be deleted while it is assigned to users.");
            }

            Delete(entity);

            return Success("Role deleted successfully.", id);
        }
    }
}
