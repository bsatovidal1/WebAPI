using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Project1_BrunoVidal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Project1_BrunoVidal.Data
{
    public class ArtContext : DbContext
    {
        //To give access to IHttpContextAccessor for Audit Data with IAuditable
        private readonly IHttpContextAccessor _httpContextAccessor;

        //Property to hold the UserName value
        public string UserName
        {
            get; private set;
        }

        public ArtContext(DbContextOptions<ArtContext> options, IHttpContextAccessor httpContextAccessor)
                : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            UserName = _httpContextAccessor.HttpContext?.User.Identity.Name;
            //UserName = (UserName == null) ? "Unknown" : UserName;
            UserName = UserName ?? "Unknown";
        }
        public ArtContext(DbContextOptions<ArtContext> options)
            : base(options)
        {
            UserName = "SeedData";
        }

        public DbSet<ArtType> ArtTypes { get; set; }
        public DbSet<Artwork> Artworks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("AW");

            modelBuilder.Entity<ArtType>()
                .HasMany(a => a.Artworks)
                .WithOne(a => a.ArtType)
                .OnDelete(DeleteBehavior.Restrict);

            //Add a unique combination to the Fluent API for Name, Completed and ArtTypeID in the Artwork class.
            modelBuilder.Entity<Artwork>()
                .HasIndex(p => new { p.Name, p.Completed, p.ArtTypeID })
                .IsUnique();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is IAuditable trackable)
                {
                    var now = DateTime.UtcNow;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.UpdatedOn = now;
                            trackable.UpdatedBy = UserName;
                            break;

                        case EntityState.Added:
                            trackable.CreatedOn = now;
                            trackable.CreatedBy = UserName;
                            trackable.UpdatedOn = now;
                            trackable.UpdatedBy = UserName;
                            break;
                    }
                }
            }
        }
    }
}
