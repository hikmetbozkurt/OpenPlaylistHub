using APP.DataAccess.Context;
using APP.DataAccess.Entities;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using CORE.APP.Services.MVC;
using Microsoft.EntityFrameworkCore;

namespace APP.Business.Services
{
    public class UserService : Service<User>, IService<UserRequest, UserResponse>
    {
        private readonly Db _db;

        public UserService(Db db) : base(db)
        {
            _db = db;
        }

        protected override IQueryable<User> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).Include(u => u.Group);
        }

        public List<UserResponse> List()
        {
            return Query()
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    Guid = u.Guid,
                    UserName = u.UserName,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    BirthDate = u.BirthDate,
                    GroupId = u.GroupId,
                    GroupName = u.Group != null ? u.Group.Name : null
                })
                .ToList();
        }

        public UserResponse Item(int id)
        {
            return Query()
                .Where(u => u.Id == id)
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    Guid = u.Guid,
                    UserName = u.UserName,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    BirthDate = u.BirthDate,
                    GroupId = u.GroupId,
                    GroupName = u.Group != null ? u.Group.Name : null
                })
                .SingleOrDefault();
        }

        public UserRequest Edit(int id)
        {
            var user = Query().FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return null;
            }

            return new UserRequest
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Password = string.Empty,
                IsActive = user.IsActive,
                BirthDate = user.BirthDate,
                GroupId = user.GroupId
            };
        }

        public CommandResponse Create(UserRequest request)
        {
            if (_db.Users.Any(u => u.Email == request.Email))
            {
                return Error("User with the same e-mail already exists.");
            }

            var entity = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                Password = request.Password,
                IsActive = request.IsActive,
                BirthDate = request.BirthDate,
                GroupId = request.GroupId
            };

            Create(entity);

            return Success("User created successfully.", entity.Id);
        }

        public CommandResponse Update(UserRequest request)
        {
            var entity = Query(false).FirstOrDefault(u => u.Id == request.Id);
            if (entity == null)
            {
                return Error("User not found!");
            }

            if (_db.Users.Any(u => u.Id != request.Id && u.Email == request.Email))
            {
                return Error("User with the same e-mail already exists.");
            }

            entity.UserName = request.UserName;
            entity.Email = request.Email;
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                entity.Password = request.Password;
            }
            entity.IsActive = request.IsActive;
            entity.BirthDate = request.BirthDate;
            entity.GroupId = request.GroupId;

            Update(entity);

            return Success("User updated successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false).Include(u => u.Playlists).SingleOrDefault(u => u.Id == id);
            if (entity == null)
            {
                return Error("User not found!");
            }

            if (entity.Playlists?.Any() == true)
            {
                _db.Playlists.RemoveRange(entity.Playlists);
            }

            Delete(entity);

            return Success("User deleted successfully.", id);
        }
    }
}
