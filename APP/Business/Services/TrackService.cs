using APP.DataAccess.Context;
using APP.DataAccess.Entities;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using CORE.APP.Services.MVC;
using Microsoft.EntityFrameworkCore;

namespace APP.Business.Services
{
    public class TrackService : Service, IService<TrackRequest, TrackResponse>
    {
        private readonly Db _db;

        public TrackService(Db db)
        {
            _db = db;
        }

        public List<TrackResponse> GetAll()
        {
            return _db.Tracks
                .Select(t => new TrackResponse
                {
                    Id = t.Id,
                    Title = t.Title,
                    Album = t.Album,
                    Duration = t.Duration,
                    Rating = t.Rating,
                    ReleaseDate = t.ReleaseDate,
                    IsFavorite = t.IsFavorite,
                    Genre = t.Genre
                })
                .ToList();
        }

        public TrackResponse GetById(int id)
        {
            var track = _db.Tracks.FirstOrDefault(t => t.Id == id);

            if (track == null)
                return null;

            return new TrackResponse
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
            var track = new Track
            {
                Title = request.Title,
                Album = request.Album,
                Duration = request.Duration,
                Rating = request.Rating,
                ReleaseDate = request.ReleaseDate,
                IsFavorite = request.IsFavorite,
                Genre = request.Genre
            };

            _db.Tracks.Add(track);
            _db.SaveChanges();

            return new CommandResponse();
        }

        public CommandResponse Update(TrackRequest request)
        {
            var track = _db.Tracks.FirstOrDefault(t => t.Id == request.Id);
            if (track == null)
                return new CommandResponse();

            track.Title = request.Title;
            track.Album = request.Album;
            track.Duration = request.Duration;
            track.Rating = request.Rating;
            track.ReleaseDate = request.ReleaseDate;
            track.IsFavorite = request.IsFavorite;
            track.Genre = request.Genre;

            _db.SaveChanges();

            return new CommandResponse();
        }

        public CommandResponse Delete(int id)
        {
            var track = _db.Tracks
                .Include(t => t.PlaylistTracks)
                .FirstOrDefault(t => t.Id == id);
            if (track == null)
                return new CommandResponse();

            _db.PlaylistTracks.RemoveRange(track.PlaylistTracks);
            _db.Tracks.Remove(track);
            _db.SaveChanges();

            return new CommandResponse();
        }
    }
}

