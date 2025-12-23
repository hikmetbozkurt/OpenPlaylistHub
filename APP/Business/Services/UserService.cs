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
            return base.Query(isNoTracking);
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
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Gender = u.Gender.ToString(),
                    IsActive = u.IsActive,
                    IsActiveFormatted = u.IsActive ? "Active" : "Passive",
                    BirthDate = u.BirthDate,
                    BirthDateFormatted = u.BirthDate.HasValue ? u.BirthDate.Value.ToShortDateString() : string.Empty
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
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Gender = entity.Gender.ToString(),
                IsActive = entity.IsActive,
                IsActiveFormatted = entity.IsActive ? "Active" : "Passive",
                BirthDate = entity.BirthDate,
                BirthDateFormatted = entity.BirthDate.HasValue ? entity.BirthDate.Value.ToShortDateString() : string.Empty
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
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                Password = string.Empty,
                IsActive = user.IsActive,
                BirthDate = user.BirthDate
            };
        }

        public CommandResponse Create(UserRequest request)
        {
            // Email check removed
            if (_db.Users.Any(u => u.UserName == request.UserName))
            {
                return Error("User with the same user name already exists.");
            }

            var entity = new User
            {
                UserName = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = request.Gender,
                // Email removed
                Password = request.Password, // Should be hashed in real app
                IsActive = request.IsActive,
                RegistrationDate = DateTime.Now,
                BirthDate = request.BirthDate
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

            if (_db.Users.Any(u => u.Id != request.Id && u.UserName == request.UserName))
            {
                return Error("User with the same user name already exists.");
            }

            entity.UserName = request.UserName;
            entity.FirstName = request.FirstName;
            entity.LastName = request.LastName;
            entity.Gender = request.Gender;
            
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                entity.Password = request.Password;
            }
            entity.IsActive = request.IsActive;
            entity.BirthDate = request.BirthDate;

            Update(entity);

            return Success("User updated successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            // Include related entities to check or cascade
            // User now has PlaylistMembers, SongRatings, UserRoles
            var entity = Query(false)
                .Include(u => u.Playlists) // Owned playlists
                .Include(u => u.PlaylistMembers)
                .Include(u => u.SongRatings)
                .Include(u => u.UserRoles)
                .SingleOrDefault(u => u.Id == id);

            if (entity == null)
            {
                return Error("User not found!");
            }

            // Explicitly remove related data if not cascade delete
            if (entity.Playlists?.Any() == true)
            {
                _db.Playlists.RemoveRange(entity.Playlists);
            }
             if (entity.PlaylistMembers?.Any() == true)
            {
                _db.PlaylistMembers.RemoveRange(entity.PlaylistMembers);
            }
             if (entity.SongRatings?.Any() == true)
            {
                _db.SongRatings.RemoveRange(entity.SongRatings);
            }
             if (entity.UserRoles?.Any() == true)
            {
                _db.UserRoles.RemoveRange(entity.UserRoles);
            }

            Delete(entity);

            return Success("User deleted successfully.", id);
        }

        public UserResponse Login(string userName, string password)
        {
            // Login with UserName only, no Email
            var userEntity = Query().SingleOrDefault(u => u.UserName == userName && u.Password == password && u.IsActive);
            if (userEntity == null)
            {
                return null;
            }

            return new UserResponse
            {
                Id = userEntity.Id,
                Guid = userEntity.Guid,
                UserName = userEntity.UserName,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Gender = userEntity.Gender.ToString(),
                IsActive = userEntity.IsActive,
                IsActiveFormatted = userEntity.IsActive ? "Active" : "Passive",
                BirthDate = userEntity.BirthDate,
                BirthDateFormatted = userEntity.BirthDate.HasValue ? userEntity.BirthDate.Value.ToShortDateString() : string.Empty
            };
        }
    }
}
