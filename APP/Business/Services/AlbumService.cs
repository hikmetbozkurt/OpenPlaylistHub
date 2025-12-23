using APP.DataAccess.Context;
using APP.DataAccess.Entities;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using CORE.APP.Services.MVC;
using Microsoft.EntityFrameworkCore;

namespace APP.Business.Services
{
    public class AlbumService : Service<Album>, IService<AlbumRequest, AlbumResponse>
    {
        private readonly Db _db;

        public AlbumService(Db db) : base(db)
        {
            _db = db;
        }

        protected override IQueryable<Album> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).Include(a => a.Artist);
        }

        public List<AlbumResponse> List()
        {
            return Query()
                .Select(a => new AlbumResponse
                {
                    Id = a.Id,
                    Guid = a.Guid,
                    Name = a.Name,
                    ReleaseDate = a.ReleaseDate,
                    ArtistId = a.ArtistId,
                    ArtistName = a.Artist != null ? (a.Artist.IsBand ? a.Artist.LastName : a.Artist.FirstName + " " + a.Artist.LastName) : null
                })
                .OrderBy(a => a.Name)
                .ToList();
        }

        public AlbumResponse Item(int id)
        {
            return Query()
                .Where(a => a.Id == id)
                .Select(a => new AlbumResponse
                {
                    Id = a.Id,
                    Guid = a.Guid,
                    Name = a.Name,
                    ReleaseDate = a.ReleaseDate,
                    ArtistId = a.ArtistId,
                    ArtistName = a.Artist != null ? (a.Artist.IsBand ? a.Artist.LastName : a.Artist.FirstName + " " + a.Artist.LastName) : null
                })
                .SingleOrDefault();
        }

        public AlbumRequest Edit(int id)
        {
            var album = Query().FirstOrDefault(a => a.Id == id);
            if (album == null)
            {
                return null;
            }

            return new AlbumRequest
            {
                Id = album.Id,
                Name = album.Name,
                ReleaseDate = album.ReleaseDate,
                ArtistId = album.ArtistId
            };
        }

        public CommandResponse Create(AlbumRequest request)
        {
            if (!_db.Artists.Any(a => a.Id == request.ArtistId))
            {
                return Error("Artist not found!");
            }

            var entity = new Album
            {
                Name = request.Name,
                ReleaseDate = request.ReleaseDate,
                ArtistId = request.ArtistId
            };

            Create(entity);

            return Success("Album created successfully.", entity.Id);
        }

        public CommandResponse Update(AlbumRequest request)
        {
            var entity = Query(false).FirstOrDefault(a => a.Id == request.Id);
            if (entity == null)
            {
                return Error("Album not found!");
            }

            if (!_db.Artists.Any(a => a.Id == request.ArtistId))
            {
                return Error("Artist not found!");
            }

            entity.Name = request.Name;
            entity.ReleaseDate = request.ReleaseDate;
            entity.ArtistId = request.ArtistId;

            Update(entity);

            return Success("Album updated successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false)
                .Include(a => a.Songs)
                .SingleOrDefault(a => a.Id == id);
            if (entity == null)
            {
                return Error("Album not found!");
            }

            if (entity.Songs?.Any() == true)
            {
                foreach (var song in entity.Songs)
                {
                    song.AlbumId = null;
                }
            }

            Delete(entity);

            return Success("Album deleted successfully.", id);
        }
    }
}

