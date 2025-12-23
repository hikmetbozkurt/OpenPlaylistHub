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
                .Include(p => p.OwnerUser)
                .Include(p => p.PlaylistSongs)
                    .ThenInclude(ps => ps.Song)
                    .ThenInclude(s => s.Artist); // Include Artist for song details
        }

        public List<PlaylistResponse> List()
        {
            return Query()
                .Select(p => new PlaylistResponse
                {
                    Id = p.Id,
                    Guid = p.Guid,
                    Name = p.Name,
                    IsPublic = p.IsPublic,
                    CreatedDate = p.CreatedDate,
                    OwnerUserId = p.OwnerUserId,
                    OwnerUserName = p.OwnerUser != null ? p.OwnerUser.UserName : string.Empty,
                    Songs = p.PlaylistSongs.OrderBy(ps => ps.OrderNo).Select(ps => new SongResponse
                    {
                        Id = ps.Song.Id,
                        Guid = ps.Song.Guid,
                        Title = ps.Song.Title,
                        DurationSeconds = ps.Song.DurationSeconds,
                        // Basic song info for listing
                        ArtistName = ps.Song.Artist != null ? ps.Song.Artist.FirstName + " " + ps.Song.Artist.LastName : ""
                    }).ToList()
                })
                .OrderByDescending(p => p.CreatedDate)
                .ToList();
        }

        public PlaylistResponse Item(int id)
        {
            // Similar projection but maybe more details
             return Query()
                .Where(p => p.Id == id)
                .Select(p => new PlaylistResponse
                {
                    Id = p.Id,
                    Guid = p.Guid,
                    Name = p.Name,
                    IsPublic = p.IsPublic,
                    CreatedDate = p.CreatedDate,
                    OwnerUserId = p.OwnerUserId,
                    OwnerUserName = p.OwnerUser != null ? p.OwnerUser.UserName : string.Empty,
                    Songs = p.PlaylistSongs.OrderBy(ps => ps.OrderNo).Select(ps => new SongResponse
                    {
                        Id = ps.Song.Id,
                        Guid = ps.Song.Guid,
                        Title = ps.Song.Title,
                        DurationSeconds = ps.Song.DurationSeconds,
                        TotalStreams = ps.Song.TotalStreams,
                        ReleaseDate = ps.Song.ReleaseDate,
                        AlbumId = ps.Song.AlbumId,
                        AlbumTitle = ps.Song.Album != null ? ps.Song.Album.Name : null,
                        ArtistId = ps.Song.ArtistId,
                        ArtistName = ps.Song.Artist != null ? ps.Song.Artist.FirstName + " " + ps.Song.Artist.LastName : "",
                        // Note: AverageRating and GenreNames might require more includes or separate logic if needed here
                    }).ToList()
                })
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
                IsPublic = playlist.IsPublic,
                OwnerUserId = playlist.OwnerUserId,
                SongIds = playlist.PlaylistSongs.Select(ps => ps.SongId).ToList()
            };
        }

        public CommandResponse Create(PlaylistRequest request)
        {
            if (!_db.Users.Any(u => u.Id == request.OwnerUserId))
            {
                return Error("Playlist owner was not found.");
            }

            var entity = new Playlist
            {
                Name = request.Name,
                IsPublic = request.IsPublic,
                CreatedDate = DateTime.UtcNow,
                OwnerUserId = request.OwnerUserId
            };

            Create(entity);

             if (request.SongIds != null && request.SongIds.Any())
            {
                short order = 1;
                foreach (var songId in request.SongIds)
                {
                     if (_db.Songs.Any(s => s.Id == songId))
                     {
                         _db.PlaylistSongs.Add(new PlaylistSong
                         {
                             PlaylistId = entity.Id,
                             SongId = songId,
                             OrderNo = order++
                         });
                     }
                }
                SaveChanges();
            }

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
            entity.IsPublic = request.IsPublic;
            entity.OwnerUserId = request.OwnerUserId;

            // Handle Song updates (Full replacement or additive? Ususally full replacement if list provided)
            // Existing logic didn't handle update of tracks inside Update method, only Edit provided Ids.
            // But typical edit flow might want to update list.
            
            var existingSongs = _db.PlaylistSongs.Where(ps => ps.PlaylistId == entity.Id).ToList();
            _db.PlaylistSongs.RemoveRange(existingSongs);

            if (request.SongIds != null && request.SongIds.Any())
            {
                short order = 1;
                foreach (var songId in request.SongIds)
                {
                    _db.PlaylistSongs.Add(new PlaylistSong
                    {
                        PlaylistId = entity.Id,
                        SongId = songId,
                        OrderNo = order++
                    });
                }
            }

            Update(entity);
            SaveChanges();

            return Success("Playlist updated successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false)
                .Include(p => p.PlaylistSongs)
                .SingleOrDefault(p => p.Id == id);
            if (entity == null)
            {
                return Error("Playlist not found!");
            }

            if (entity.PlaylistSongs?.Any() == true)
            {
                _db.PlaylistSongs.RemoveRange(entity.PlaylistSongs);
            }

            Delete(entity);

            return Success("Playlist deleted successfully.", id);
        }

        public CommandResponse AddSongToPlaylist(int playlistId, int songId)
        {
            var playlist = Query(false)
                .Include(p => p.PlaylistSongs)
                .SingleOrDefault(p => p.Id == playlistId);
            if (playlist == null)
            {
                return Error("Playlist not found!");
            }

            if (_db.PlaylistSongs.Any(ps => ps.PlaylistId == playlistId && ps.SongId == songId))
            {
                return Error("Song already exists in the playlist.");
            }

            var order = (short)((playlist.PlaylistSongs
                .Select(ps => ps.OrderNo)
                .Max() ?? 0) + 1);

            var playlistSong = new PlaylistSong
            {
                PlaylistId = playlistId,
                SongId = songId,
                OrderNo = order
            };

            _db.PlaylistSongs.Add(playlistSong);
            _db.SaveChanges();

            return Success("Song added to playlist.", playlistId);
        }

        public CommandResponse RemoveSongFromPlaylist(int playlistId, int songId)
        {
            var playlistSong = _db.PlaylistSongs
                .FirstOrDefault(ps => ps.PlaylistId == playlistId && ps.SongId == songId);
            if (playlistSong == null)
            {
                return Error("Song not found in playlist!");
            }

            _db.PlaylistSongs.Remove(playlistSong);
            _db.SaveChanges();

            return Success("Song removed from playlist.", playlistId);
        }
    }
}
