using APP.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace APP.DataAccess.Context
{
    public class Db : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<PlaylistTrack> PlaylistTracks { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<TrackArtist> TrackArtists { get; set; }

        public Db(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<PlaylistTrack>()
                .HasKey(pt => new { pt.PlaylistId, pt.TrackId });

            modelBuilder.Entity<TrackArtist>()
                .HasKey(ta => new { ta.TrackId, ta.ArtistId });
        }
    }
}

