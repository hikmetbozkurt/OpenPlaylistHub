using APP.DataAccess.Context;
using APP.DataAccess.Entities;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using CORE.APP.Services.MVC;
using Microsoft.EntityFrameworkCore;

namespace APP.Business.Services
{
    public class ArtistService : Service<Artist>, IService<ArtistRequest, ArtistResponse>
    {
        private readonly Db _db;

        public ArtistService(Db db) : base(db)
        {
            _db = db;
        }

        public List<ArtistResponse> List()
        {
            return Query()
                .Select(a => new ArtistResponse
                {
                    Id = a.Id,
                    Guid = a.Guid,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    IsBand = a.IsBand
                })
                .OrderBy(a => a.FirstName).ThenBy(a => a.LastName)
                .ToList();
        }

        public ArtistResponse Item(int id)
        {
            return Query()
                .Where(a => a.Id == id)
                .Select(a => new ArtistResponse
                {
                    Id = a.Id,
                    Guid = a.Guid,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    IsBand = a.IsBand
                })
                .SingleOrDefault();
        }

        public ArtistRequest Edit(int id)
        {
            var artist = Query().FirstOrDefault(a => a.Id == id);
            if (artist == null)
            {
                return null;
            }

            return new ArtistRequest
            {
                Id = artist.Id,
                FirstName = artist.FirstName,
                LastName = artist.LastName,
                IsBand = artist.IsBand
            };
        }

        public CommandResponse Create(ArtistRequest request)
        {
            var entity = new Artist
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                IsBand = request.IsBand
            };

            Create(entity);

            return Success("Artist created successfully.", entity.Id);
        }

        public CommandResponse Update(ArtistRequest request)
        {
            var entity = Query(false).FirstOrDefault(a => a.Id == request.Id);
            if (entity == null)
            {
                return Error("Artist not found!");
            }

            entity.FirstName = request.FirstName;
            entity.LastName = request.LastName;
            entity.IsBand = request.IsBand;

            Update(entity);

            return Success("Artist updated successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false)
                .Include(a => a.Songs) 
                .SingleOrDefault(a => a.Id == id);
            if (entity == null)
            {
                return Error("Artist not found!");
            }

            if (entity.Songs?.Any() == true)
            {
                return Error("Artist has songs! Delete songs first.");
            }

            Delete(entity);

            return Success("Artist deleted successfully.", id);
        }
    }
}

