using APP.DataAccess.Context;
using APP.DataAccess.Entities;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using CORE.APP.Services.MVC;
using Microsoft.EntityFrameworkCore;

namespace APP.Business.Services
{
    public class GenreService : Service<Genre>, IService<GenreRequest, GenreResponse>
    {
        private readonly Db _db;

        public GenreService(Db db) : base(db)
        {
            _db = db;
        }

        public List<GenreResponse> List()
        {
            return Query()
                .Select(g => new GenreResponse
                {
                    Id = g.Id,
                    Guid = g.Guid,
                    Name = g.Name
                })
                .OrderBy(g => g.Name)
                .ToList();
        }

        public GenreResponse Item(int id)
        {
            return Query()
                .Where(g => g.Id == id)
                .Select(g => new GenreResponse
                {
                    Id = g.Id,
                    Guid = g.Guid,
                    Name = g.Name
                })
                .SingleOrDefault();
        }

        public GenreRequest Edit(int id)
        {
            var genre = Query().FirstOrDefault(g => g.Id == id);
            if (genre == null)
            {
                return null;
            }

            return new GenreRequest
            {
                Id = genre.Id,
                Name = genre.Name
            };
        }

        public CommandResponse Create(GenreRequest request)
        {
            if (_db.Genres.Any(g => g.Name == request.Name))
            {
                return Error("Genre with the same name already exists.");
            }

            var entity = new Genre
            {
                Name = request.Name
            };

            Create(entity);

            return Success("Genre created successfully.", entity.Id);
        }

        public CommandResponse Update(GenreRequest request)
        {
            var entity = Query(false).FirstOrDefault(g => g.Id == request.Id);
            if (entity == null)
            {
                return Error("Genre not found!");
            }

            if (_db.Genres.Any(g => g.Id != request.Id && g.Name == request.Name))
            {
                return Error("Genre with the same name already exists.");
            }

            entity.Name = request.Name;

            Update(entity);

            return Success("Genre updated successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false)
                .Include(g => g.SongGenres)
                .SingleOrDefault(g => g.Id == id);
            if (entity == null)
            {
                return Error("Genre not found!");
            }

            if (entity.SongGenres?.Any() == true)
            {
                _db.SongGenres.RemoveRange(entity.SongGenres);
            }

            Delete(entity);

            return Success("Genre deleted successfully.", id);
        }
    }
}

