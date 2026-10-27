using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace RunnerMeets.Models;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options)
    { }

    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<RunnerProfile> RunnerProfiles => Set<RunnerProfile>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Place> Places => Set<Place>();
    public DbSet<SupplyStation> SupplyStations => Set<SupplyStation>();
    public DbSet<EventToHosts> EventToHosts => Set<EventToHosts>();
    public DbSet<EventToRegistration> EventToRegistrations => Set<EventToRegistration>();
    public DbSet<EventToSupplyStation> EventToSupplyStations => Set<EventToSupplyStation>();
    public DbSet<EventTimeOptions> EventTimeOptions => Set<EventTimeOptions>();
    public DbSet<EventTimeVotes> EventTimeVotes => Set<EventTimeVotes>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.DisplayName).HasMaxLength(200);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Postcode).HasMaxLength(10);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("NOW()");

            entity.HasIndex(e => e.IdentityUserId).IsUnique();

            entity.HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<UserProfile>(e => e.IdentityUserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<RunnerProfile>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Bio).HasMaxLength(1000);
            entity.Property(e => e.ExperienceLevel).HasMaxLength(50);
            entity.Property(e => e.MinPaceSecondsPerKm);
            entity.Property(e => e.MaxPaceSecondsPerKm);
            entity.Property(e => e.TrainingFor).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("NOW()");
            entity.Property(e => e.PreferredDistanceKm).HasColumnType("decimal(5,2)");

            entity.HasOne(e => e.UserProfile)
                .WithOne()
                .HasForeignKey<RunnerProfile>(e => e.UserProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.UserProfileId).IsUnique();
        });

        builder.Entity<Place>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.FormattedAddress).HasMaxLength(500);
            entity.Property(e => e.PlaceType).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("NOW()");

            entity.HasIndex(e => e.GooglePlaceId);
        });

        builder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);

            entity.HasOne(e => e.Place)
                .WithMany()
                .HasForeignKey(e => e.PlaceId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.UserProfile)
                .WithMany()
                .HasForeignKey(e => e.CreatedByUserProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.StartDateTime);
            entity.Property(e => e.EndDateTime);
            entity.Property(e => e.RoutePolyline).HasMaxLength(2000);
            entity.Property(e => e.MaxParticipants);
            entity.Property(e => e.MinParticipants);
            entity.Property(e => e.MinPaceSecondsPerKm);
            entity.Property(e => e.MaxPaceSecondsPerKm);

            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Visibility).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.DistanceKm).HasColumnType("decimal(5,2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("NOW()");

            entity.HasIndex(e => e.PlaceId);
            entity.HasIndex(e => e.CreatedByUserProfileId);
            entity.HasIndex(e => e.StartDateTime);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.Visibility);
        });

        builder.Entity<SupplyStation>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.StationType).HasMaxLength(100);
            entity.Property(e => e.Notes).HasMaxLength(1000);

            entity.HasOne(e => e.Place)
                .WithMany()
                .HasForeignKey(e => e.PlaceId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.PlaceId);
        });

        builder.Entity<EventToHosts>(entity =>
        {
            entity.HasKey(e => new { e.EventId, e.UserProfileId });

            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Event)
                .WithMany()
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.UserProfile)
                .WithMany()
                .HasForeignKey(e => e.UserProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.UserProfileId);
        });

        builder.Entity<EventToRegistration>(entity =>
        {
            entity.HasKey(e => new { e.EventId, e.UserId });

            entity.Property(e => e.RegistrationStatus).HasMaxLength(50);
            entity.Property(e => e.JoinedAt).HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Event)
                .WithMany()
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.RegistrationStatus);
        });

        builder.Entity<EventToSupplyStation>(entity =>
        {
            entity.HasKey(e => new { e.EventId, e.SupplyStationId });

            entity.HasOne(e => e.Event)
                .WithMany()
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.SupplyStation)
                .WithMany()
                .HasForeignKey(e => e.SupplyStationId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.SupplyStationId);
        });

        builder.Entity<EventTimeOptions>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Event)
                .WithMany()
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.UserProfile)
                .WithMany()
                .HasForeignKey(e => e.CreatedById)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.EventId);
            entity.HasIndex(e => e.CreatedById);
            entity.HasIndex(e => e.StartTime);
        });

        builder.Entity<EventTimeVotes>(entity =>
        {
            entity.HasKey(e => new { e.EventTimeOptionId, e.UserProfileId });

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.EventTimeOptions)
                .WithMany()
                .HasForeignKey(e => e.EventTimeOptionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.UserProfile)
                .WithMany()
                .HasForeignKey(e => e.UserProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.UserProfileId);
        });
    }
}
