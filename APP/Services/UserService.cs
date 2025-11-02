using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using CORE.APP.Services.MVC;
using Microsoft.EntityFrameworkCore;

namespace APP.Services
{
    public class UserService : Service, IService<UserRequest, UserResponse>
    {
        private readonly Db _db;

        public UserService(Db db)
        {
            _db = db;
        }

        public List<UserResponse> GetAll()
        {
            return _db.Users
                .Include(u => u.Group)
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    BirthDate = u.BirthDate,
                    GroupId = u.GroupId,
                    GroupName = u.Group != null ? u.Group.Name : null
                })
                .ToList();
        }

        public UserResponse GetById(int id)
        {
            var user = _db.Users
                .Include(u => u.Group)
                .FirstOrDefault(u => u.Id == id);

            if (user == null)
                return null;

            return new UserResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsActive = user.IsActive,
                BirthDate = user.BirthDate,
                GroupId = user.GroupId,
                GroupName = user.Group != null ? user.Group.Name : null
            };
        }

        public CommandResponse Create(UserRequest request)
        {
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                Password = request.Password,
                IsActive = request.IsActive,
                BirthDate = request.BirthDate,
                GroupId = request.GroupId
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            return new CommandResponse();
        }

        public CommandResponse Update(UserRequest request)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == request.Id);
            if (user == null)
                return new CommandResponse();

            user.UserName = request.UserName;
            user.Email = request.Email;
            if (!string.IsNullOrEmpty(request.Password))
            {
                user.Password = request.Password;
            }
            user.IsActive = request.IsActive;
            user.BirthDate = request.BirthDate;
            user.GroupId = request.GroupId;

            _db.SaveChanges();

            return new CommandResponse();
        }

        public CommandResponse Delete(int id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return new CommandResponse();

            _db.Users.Remove(user);
            _db.SaveChanges();

            return new CommandResponse();
        }
    }
}

