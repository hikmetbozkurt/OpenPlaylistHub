using System;
using APP.DataAccess.Context;
using APP.DataAccess.Entities;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using CORE.APP.Services.MVC;
using Microsoft.EntityFrameworkCore;

namespace APP.Business.Services
{
    public class PlaylistService : Service<Playlist>, IService<PlaylistRequest, PlaylistResponse>
    {
        private readonly Db _db;

        public PlaylistService(Db db) : base(db)
        {
            _db = db;
        }

        protected override IQueryable<Playlist> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking)
                .Include(p => p.User)
                .Include(p => p.PlaylistTracks)
                    .ThenInclude(pt => pt.Track);
        }

        public List<PlaylistResponse> List()
        {
            return Query()
                .Select(ProjectToResponse)
                .OrderByDescending(p => p.CreatedDate)
                .ToList();
        }

        public PlaylistResponse Item(int id)
        {
            return Query()
                .Where(p => p.Id == id)
                .Select(ProjectToResponse)
                .SingleOrDefault();
        }

        public PlaylistRequest Edit(int id)
        {
            var playlist = Query().FirstOrDefault(p => p.Id == id);
            if (playlist == null)
            {
                return null;
            }

            return new PlaylistRequest
            {
                Id = playlist.Id,
                Name = playlist.Name,
                Description = playlist.Description,
                IsPublic = playlist.IsPublic,
                CreatedDate = playlist.CreatedDate,
                UserId = playlist.UserId
            };
        }

        public CommandResponse Create(PlaylistRequest request)
        {
            if (!_db.Users.Any(u => u.Id == request.UserId))
            {
                return Error("Playlist owner was not found.");
            }

            var entity = new Playlist
            {
                Name = request.Name,
                Description = request.Description,
                IsPublic = request.IsPublic,
                CreatedDate = request.CreatedDate == default ? DateTime.UtcNow : request.CreatedDate,
                UserId = request.UserId
            };

            Create(entity);

            return Success("Playlist created successfully.", entity.Id);
        }

        public CommandResponse Update(PlaylistRequest request)
        {
            var entity = Query(false).FirstOrDefault(p => p.Id == request.Id);
            if (entity == null)
            {
                return Error("Playlist not found!");
            }

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.IsPublic = request.IsPublic;
            entity.CreatedDate = request.CreatedDate;
            entity.UserId = request.UserId;

            Update(entity);

            return Success("Playlist updated successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false)
                .Include(p => p.PlaylistTracks)
                .SingleOrDefault(p => p.Id == id);
            if (entity == null)
            {
                return Error("Playlist not found!");
            }

            if (entity.PlaylistTracks?.Any() == true)
            {
                _db.PlaylistTracks.RemoveRange(entity.PlaylistTracks);
            }

            Delete(entity);

            return Success("Playlist deleted successfully.", id);
        }

        public CommandResponse AddTrackToPlaylist(int playlistId, int trackId)
        {
            var playlist = Query(false)
                .Include(p => p.PlaylistTracks)
                .SingleOrDefault(p => p.Id == playlistId);
            if (playlist == null)
            {
                return Error("Playlist not found!");
            }

            if (_db.PlaylistTracks.Any(pt => pt.PlaylistId == playlistId && pt.TrackId == trackId))
            {
                return Error("Track already exists in the playlist.");
            }

            var order = playlist.PlaylistTracks
                .Select(pt => pt.Order)
                .DefaultIfEmpty(0)
                .Max() + 1;

            var playlistTrack = new PlaylistTrack
            {
                PlaylistId = playlistId,
                TrackId = trackId,
                Order = order
            };

            _db.PlaylistTracks.Add(playlistTrack);
            _db.SaveChanges();

            return Success("Track added to playlist.", playlistId);
        }

        public CommandResponse RemoveTrackFromPlaylist(int playlistId, int trackId)
        {
            var playlistTrack = _db.PlaylistTracks
                .FirstOrDefault(pt => pt.PlaylistId == playlistId && pt.TrackId == trackId);
            if (playlistTrack == null)
            {
                return Error("Track relationship not found!");
            }

            _db.PlaylistTracks.Remove(playlistTrack);
            _db.SaveChanges();

            return Success("Track removed from playlist.", playlistId);
        }

        private static PlaylistResponse ProjectToResponse(Playlist playlist)
        {
            return new PlaylistResponse
            {
                Id = playlist.Id,
                Guid = playlist.Guid,
                Name = playlist.Name,
                Description = playlist.Description,
                IsPublic = playlist.IsPublic,
                CreatedDate = playlist.CreatedDate,
                UserId = playlist.UserId,
                UserName = playlist.User?.UserName,
                Tracks = playlist.PlaylistTracks
                    .OrderBy(pt => pt.Order)
                    .Select(pt => new TrackResponse
                    {
                        Id = pt.Track.Id,
                        Guid = pt.Track.Guid,
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
    }
}
