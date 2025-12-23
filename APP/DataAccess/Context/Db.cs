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
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<SongGenre> SongGenres { get; set; }
        public DbSet<PlaylistSong> PlaylistSongs { get; set; }
        public DbSet<PlaylistMember> PlaylistMembers { get; set; }
        public DbSet<SongRating> SongRatings { get; set; }

        public Db(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            // PlaylistTrack configuration removed

            // TrackArtist configuration removed

            modelBuilder.Entity<SongGenre>()
                .HasKey(sg => new { sg.SongId, sg.GenreId });

            // SongArtist configuration removed

            modelBuilder.Entity<PlaylistSong>()
                .HasKey(ps => new { ps.PlaylistId, ps.SongId });

            modelBuilder.Entity<PlaylistMember>()
                .HasKey(pm => new { pm.PlaylistId, pm.UserId });

            modelBuilder.Entity<SongRating>()
                .HasKey(sr => new { sr.SongId, sr.UserId });
        }
    }
}

