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
                .ToList()
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    Guid = u.Guid,
                    UserName = u.UserName,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    IsActiveFormatted = u.IsActive ? "Active" : "Passive",
                    BirthDate = u.BirthDate,
                    BirthDateFormatted = u.BirthDate.ToShortDateString(),
                    GroupId = u.GroupId,
                    GroupName = u.Group?.Name
                })
                .ToList();
        }

        public UserResponse Item(int id)
        {
            var entity = Query().SingleOrDefault(u => u.Id == id);
            if (entity == null)
                return null;

            return new UserResponse
            {
                Id = entity.Id,
                Guid = entity.Guid,
                UserName = entity.UserName,
                Email = entity.Email,
                IsActive = entity.IsActive,
                IsActiveFormatted = entity.IsActive ? "Active" : "Passive",
                BirthDate = entity.BirthDate,
                BirthDateFormatted = entity.BirthDate.ToShortDateString(),
                GroupId = entity.GroupId,
                GroupName = entity.Group?.Name
            };
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
            if (_db.Users.Any(u => u.Email == request.Email || u.UserName == request.UserName))
            {
                return Error("User with the same e-mail or user name already exists.");
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

            if (_db.Users.Any(u => u.Id != request.Id && (u.Email == request.Email || u.UserName == request.UserName)))
            {
                return Error("User with the same e-mail or user name already exists.");
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

        public UserResponse Login(string userName, string password)
        {
            var userEntity = Query().SingleOrDefault(u => (u.UserName == userName || u.Email == userName) && u.Password == password && u.IsActive);
            if (userEntity == null)
            {
                return null;
            }

            return new UserResponse
            {
                Id = userEntity.Id,
                Guid = userEntity.Guid,
                UserName = userEntity.UserName,
                Email = userEntity.Email,
                IsActive = userEntity.IsActive,
                IsActiveFormatted = userEntity.IsActive ? "Active" : "Passive",
                BirthDate = userEntity.BirthDate,
                BirthDateFormatted = userEntity.BirthDate.ToShortDateString(),
                GroupId = userEntity.GroupId,
                GroupName = userEntity.Group?.Name,
                // Add Role info if needed for Claims
                // Assuming Group acts as Role or separate logic? User entity has Group. Group has name.
                // We'll use Group Name as Role claim.
            };
        }
    }
}
