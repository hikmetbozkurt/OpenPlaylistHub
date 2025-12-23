using APP.DataAccess.Context;
using APP.DataAccess.Entities;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using CORE.APP.Services.MVC;
using Microsoft.EntityFrameworkCore;

namespace APP.Business.Services
{
    public class TrackService : Service<Track>, IService<TrackRequest, TrackResponse>
    {
        private readonly Db _db;

        public TrackService(Db db) : base(db)
        {
            _db = db;
        }

        public List<TrackResponse> List()
        {
            return Query()
                .OrderBy(t => t.Title)
                .ToList()
                .Select(t => new TrackResponse
                {
                    Id = t.Id,
                    Guid = t.Guid,
                    Title = t.Title,
                    Album = t.Album,
                    Duration = t.Duration,
                    Rating = t.Rating,
                    RatingFormatted = t.Rating.ToString("N1"),
                    ReleaseDate = t.ReleaseDate,
                    ReleaseDateFormatted = t.ReleaseDate.ToShortDateString(),
                    IsFavorite = t.IsFavorite,
                    Genre = t.Genre
                })
                .ToList();
        }

        public TrackResponse Item(int id)
        {
            var entity = Query().SingleOrDefault(t => t.Id == id);
            if (entity == null)
            {
                return null;
            }

            return new TrackResponse
            {
                Id = entity.Id,
                Guid = entity.Guid,
                Title = entity.Title,
                Album = entity.Album,
                Duration = entity.Duration,
                Rating = entity.Rating,
                RatingFormatted = entity.Rating.ToString("N1"),
                ReleaseDate = entity.ReleaseDate,
                ReleaseDateFormatted = entity.ReleaseDate.ToShortDateString(),
                IsFavorite = entity.IsFavorite,
                Genre = entity.Genre
            };
        }

        public TrackRequest Edit(int id)
        {
            var track = Query().FirstOrDefault(t => t.Id == id);
            if (track == null)
            {
                return null;
            }

            return new TrackRequest
            {
                Id = track.Id,
                Title = track.Title,
                Album = track.Album,
                Duration = track.Duration,
                Rating = track.Rating,
                ReleaseDate = track.ReleaseDate,
                IsFavorite = track.IsFavorite,
                Genre = track.Genre
            };
        }

        public CommandResponse Create(TrackRequest request)
        {
            var entity = new Track
            {
                Title = request.Title,
                Album = request.Album,
                Duration = request.Duration,
                Rating = request.Rating,
                ReleaseDate = request.ReleaseDate,
                IsFavorite = request.IsFavorite,
                Genre = request.Genre
            };

            Create(entity);

            return Success("Track created successfully.", entity.Id);
        }

        public CommandResponse Update(TrackRequest request)
        {
            var entity = Query(false).FirstOrDefault(t => t.Id == request.Id);
            if (entity == null)
            {
                return Error("Track not found!");
            }

            entity.Title = request.Title;
            entity.Album = request.Album;
            entity.Duration = request.Duration;
            entity.Rating = request.Rating;
            entity.ReleaseDate = request.ReleaseDate;
            entity.IsFavorite = request.IsFavorite;
            entity.Genre = request.Genre;

            Update(entity);

            return Success("Track updated successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false)
                .Include(t => t.PlaylistTracks)
                .SingleOrDefault(t => t.Id == id);
            if (entity == null)
            {
                return Error("Track not found!");
            }

            if (entity.PlaylistTracks?.Any() == true)
            {
                _db.PlaylistTracks.RemoveRange(entity.PlaylistTracks);
            }

            Delete(entity);

            return Success("Track deleted successfully.", id);
        }
    }
}
