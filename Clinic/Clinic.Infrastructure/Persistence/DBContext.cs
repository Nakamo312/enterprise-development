using Clinic.Domain.Models;
using Clinic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Persistence;

/// <summary>
/// Entity Framework database context for the Clinic application.
/// Represents a session with the database and provides access to entity sets.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the AppDbContext class.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    /// <summary>
    /// Gets or sets the Patients entity set.
    /// </summary>
    public DbSet<Patient> Patients { get; set; } = default!;

    /// <summary>
    /// Gets or sets the Doctors entity set.
    /// </summary>
    public DbSet<Doctor> Doctors { get; set; } = default!;

    /// <summary>
    /// Gets or sets the Specializations entity set.
    /// </summary>
    public DbSet<Specialization> Specializations { get; set; } = default!;

    /// <summary>
    /// Gets or sets the Appointments entity set.
    /// </summary>
    public DbSet<Appointment> Appointments { get; set; } = default!;

    /// <summary>
    /// Configures the model that was discovered by convention from the entity types
    /// exposed in DbSet properties on the derived context.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.PassportNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.DateOfBirth).IsRequired();
            entity.Property(e => e.Address).IsRequired().HasMaxLength(255);
            entity.Property(e => e.ContactPhone).HasMaxLength(20);
            entity.Property(e => e.BloodGroup).IsRequired().HasConversion<string>();
            entity.Property(e => e.Gender).IsRequired().HasConversion<string>();
            entity.Property(e => e.RhFactor).IsRequired().HasConversion<string>();
            entity.HasData(DataSeed.Patients);
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.PassportNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.YearOfBirth).IsRequired();
            entity.Property(e => e.Experience);
            entity.Property(e => e.SpecializationId).IsRequired();
            entity.HasOne<Specialization>()
                .WithMany()
                .HasForeignKey(d => d.SpecializationId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasData(DataSeed.Doctors);
        });

        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.HasData(DataSeed.Specializations);
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.DateTime).IsRequired();
            entity.Property(e => e.RoomNumber).IsRequired();
            entity.Property(e => e.IsRepeated).IsRequired();
            entity.HasOne<Patient>()
                  .WithMany()
                  .HasForeignKey(e => e.PatientId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<Doctor>()
                  .WithMany()
                  .HasForeignKey(e => e.DoctorId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.PatientId).IsRequired();
            entity.Property(e => e.DoctorId).IsRequired();
            entity.HasData(DataSeed.Appointments);
        });
    }
}