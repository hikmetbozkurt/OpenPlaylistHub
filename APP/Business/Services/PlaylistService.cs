using APP.DataAccess.Context;
using APP.DataAccess.Entities;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using CORE.APP.Services.MVC;
using Microsoft.EntityFrameworkCore;

namespace APP.Business.Services
{
    public class PlaylistService : Service, IService<PlaylistRequest, PlaylistResponse>
    {
        private readonly Db _db;

        public PlaylistService(Db db)
        {
            _db = db;
        }

        public List<PlaylistResponse> GetAll()
        {
            return _db.Playlists
                .Include(p => p.User)
                .Include(p => p.PlaylistTracks)
                    .ThenInclude(pt => pt.Track)
                .Select(p => new PlaylistResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    IsPublic = p.IsPublic,
                    CreatedDate = p.CreatedDate,
                    UserId = p.UserId,
                    UserName = p.User.UserName,
                    Tracks = p.PlaylistTracks
                        .OrderBy(pt => pt.Order)
                        .Select(pt => new TrackResponse
                        {
                            Id = pt.Track.Id,
                            Title = pt.Track.Title,
                            Album = pt.Track.Album,
                            Duration = pt.Track.Duration,
                            Rating = pt.Track.Rating,
                            ReleaseDate = pt.Track.ReleaseDate,
                            IsFavorite = pt.Track.IsFavorite,
                            Genre = pt.Track.Genre
                        })
                        .ToList()
                })
                .ToList();
        }

        public PlaylistResponse GetById(int id)
        {
            var playlist = _db.Playlists
                .Include(p => p.User)
                .Include(p => p.PlaylistTracks)
                    .ThenInclude(pt => pt.Track)
                .FirstOrDefault(p => p.Id == id);

            if (playlist == null)
                return null;

            return new PlaylistResponse
            {
                Id = playlist.Id,
                Name = playlist.Name,
                Description = playlist.Description,
                IsPublic = playlist.IsPublic,
                CreatedDate = playlist.CreatedDate,
                UserId = playlist.UserId,
                UserName = playlist.User.UserName,
                Tracks = playlist.PlaylistTracks
                    .OrderBy(pt => pt.Order)
                    .Select(pt => new TrackResponse
                    {
                        Id = pt.Track.Id,
                        Title = pt.Track.Title,
                        Album = pt.Track.Album,
                        Duration = pt.Track.Duration,
                        Rating = pt.Track.Rating,
                        ReleaseDate = pt.Track.ReleaseDate,
                        IsFavorite = pt.Track.IsFavorite,
                        Genre = pt.Track.Genre
                    })
                    .ToList()
            };
        }

        public CommandResponse Create(PlaylistRequest request)
        {
            var playlist = new Playlist
            {
                Name = request.Name,
                Description = request.Description,
                IsPublic = request.IsPublic,
                CreatedDate = request.CreatedDate,
                UserId = request.UserId
            };

            _db.Playlists.Add(playlist);
            _db.SaveChanges();

            return new CommandResponse();
        }

        public CommandResponse Update(PlaylistRequest request)
        {
            var playlist = _db.Playlists.FirstOrDefault(p => p.Id == request.Id);
            if (playlist == null)
                return new CommandResponse();

            playlist.Name = request.Name;
            playlist.Description = request.Description;
            playlist.IsPublic = request.IsPublic;
            playlist.CreatedDate = request.CreatedDate;
            playlist.UserId = request.UserId;

            _db.SaveChanges();

            return new CommandResponse();
        }

        public CommandResponse Delete(int id)
        {
            var playlist = _db.Playlists
                .Include(p => p.PlaylistTracks)
                .FirstOrDefault(p => p.Id == id);
            if (playlist == null)
                return new CommandResponse();

            _db.PlaylistTracks.RemoveRange(playlist.PlaylistTracks);
            _db.Playlists.Remove(playlist);
            _db.SaveChanges();

            return new CommandResponse();
        }

        public CommandResponse AddTrackToPlaylist(int playlistId, int trackId)
        {
            var existing = _db.PlaylistTracks
                .FirstOrDefault(pt => pt.PlaylistId == playlistId && pt.TrackId == trackId);
            if (existing != null)
                return new CommandResponse();

            var maxOrder = _db.PlaylistTracks
                .Where(pt => pt.PlaylistId == playlistId)
                .Select(pt => pt.Order)
                .DefaultIfEmpty(0)
                .Max();

            var playlistTrack = new PlaylistTrack
            {
                PlaylistId = playlistId,
                TrackId = trackId,
                Order = maxOrder + 1
            };

            _db.PlaylistTracks.Add(playlistTrack);
            _db.SaveChanges();

            return new CommandResponse();
        }

        public CommandResponse RemoveTrackFromPlaylist(int playlistId, int trackId)
        {
            var playlistTrack = _db.PlaylistTracks
                .FirstOrDefault(pt => pt.PlaylistId == playlistId && pt.TrackId == trackId);
            if (playlistTrack == null)
                return new CommandResponse();

            _db.PlaylistTracks.Remove(playlistTrack);
            _db.SaveChanges();

            return new CommandResponse();
        }
    }
}

