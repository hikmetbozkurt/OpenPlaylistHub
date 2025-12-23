using APP.DataAccess.Context;
using APP.DataAccess.Entities;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using CORE.APP.Services.MVC;
using Microsoft.EntityFrameworkCore;

namespace APP.Business.Services
{
    public class SongService : Service<Song>, IService<SongRequest, SongResponse>
    {
        private readonly Db _db;

        public SongService(Db db) : base(db)
        {
            _db = db;
        }

        protected override IQueryable<Song> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking)
                .Include(s => s.Album)
                .Include(s => s.Artist)
                .Include(s => s.SongGenres)
                    .ThenInclude(sg => sg.Genre)
                .Include(s => s.SongRatings)
                .Include(s => s.PlaylistSongs);
        }

        public List<SongResponse> List()
        {
            return Query()
                .Select(s => new SongResponse
                {
                    Id = s.Id,
                    Guid = s.Guid,
                    Title = s.Title,
                    DurationSeconds = s.DurationSeconds,
                    TotalStreams = s.TotalStreams,
                    ReleaseDate = s.ReleaseDate,
                    AlbumId = s.AlbumId,
                    AlbumTitle = s.Album != null ? s.Album.Name : null,
                    ArtistId = s.ArtistId,
                    ArtistName = s.Artist != null ? s.Artist.FirstName + " " + s.Artist.LastName + (s.Artist.IsBand ? " (Band)" : "") : string.Empty,
                    AverageRating = s.SongRatings.Any() ? s.SongRatings.Average(sr => sr.Rating) : 0,
                    GenreIds = s.SongGenres.Select(sg => sg.GenreId).ToList(),
                    GenreNames = string.Join(", ", s.SongGenres.Select(sg => sg.Genre.Name))
                })
                .OrderBy(s => s.Title)
                .ToList();
        }

        public SongResponse Item(int id)
        {
            // Similar logic to List, but for single item. 
            // Reuse projection in a clean way or duplicate if simple.
            // Duplicating for explicit clarity in this context.
            return Query()
                .Where(s => s.Id == id)
                .Select(s => new SongResponse
                {
                    Id = s.Id,
                    Guid = s.Guid,
                    Title = s.Title,
                    DurationSeconds = s.DurationSeconds,
                    TotalStreams = s.TotalStreams,
                    ReleaseDate = s.ReleaseDate,
                    AlbumId = s.AlbumId,
                    AlbumTitle = s.Album != null ? s.Album.Name : null,
                    ArtistId = s.ArtistId,
                    ArtistName = s.Artist != null ? s.Artist.FirstName + " " + s.Artist.LastName + (s.Artist.IsBand ? " (Band)" : "") : string.Empty,
                    AverageRating = s.SongRatings.Any() ? s.SongRatings.Average(sr => sr.Rating) : 0,
                    GenreIds = s.SongGenres.Select(sg => sg.GenreId).ToList(),
                    GenreNames = string.Join(", ", s.SongGenres.Select(sg => sg.Genre.Name))
                })
                .SingleOrDefault();
        }

        public SongRequest Edit(int id)
        {
            var song = Query().FirstOrDefault(s => s.Id == id);
            if (song == null)
            {
                return null;
            }

            return new SongRequest
            {
                Id = song.Id,
                Title = song.Title,
                DurationSeconds = song.DurationSeconds,
                TotalStreams = song.TotalStreams,
                ReleaseDate = song.ReleaseDate,
                AlbumId = song.AlbumId,
                ArtistId = song.ArtistId,

            };
        }

        public CommandResponse Create(SongRequest request)
        {
            if (request.AlbumId.HasValue && !_db.Albums.Any(a => a.Id == request.AlbumId.Value))
            {
                return Error("Album not found!");
            }
            if (!_db.Artists.Any(a => a.Id == request.ArtistId))
            {
                return Error("Artist not found!");
            }

            var entity = new Song
            {
                Title = request.Title,
                DurationSeconds = request.DurationSeconds,
                TotalStreams = request.TotalStreams,
                ReleaseDate = request.ReleaseDate,
                AlbumId = request.AlbumId,
                ArtistId = request.ArtistId
            };

            Create(entity, false);


            
            SaveChanges();
            return Success("Song created successfully.", entity.Id);
        }

        public CommandResponse Update(SongRequest request)
        {
            var entity = Query(false).FirstOrDefault(s => s.Id == request.Id);
            if (entity == null)
            {
                return Error("Song not found!");
            }

            if (request.AlbumId.HasValue && !_db.Albums.Any(a => a.Id == request.AlbumId.Value))
            {
                return Error("Album not found!");
            }
             if (!_db.Artists.Any(a => a.Id == request.ArtistId))
            {
                return Error("Artist not found!");
            }

            entity.Title = request.Title;
            entity.DurationSeconds = request.DurationSeconds;
            entity.TotalStreams = request.TotalStreams;
            entity.ReleaseDate = request.ReleaseDate;
            entity.AlbumId = request.AlbumId;
            entity.ArtistId = request.ArtistId;



            Update(entity, false);
            SaveChanges();

            return Success("Song updated successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false)
                .Include(s => s.PlaylistSongs)
                .Include(s => s.SongGenres)
                .Include(s => s.SongRatings)
                .SingleOrDefault(s => s.Id == id);
            if (entity == null)
            {
                return Error("Song not found!");
            }

            if (entity.PlaylistSongs?.Any() == true)
            {
                _db.PlaylistSongs.RemoveRange(entity.PlaylistSongs);
            }

            if (entity.SongGenres?.Any() == true)
            {
                _db.SongGenres.RemoveRange(entity.SongGenres);
            }
            
             if (entity.SongRatings?.Any() == true)
            {
                _db.SongRatings.RemoveRange(entity.SongRatings);
            }

            Delete(entity);
            return Success("Song deleted successfully.", id);
        }
    }
}

