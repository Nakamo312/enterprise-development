using Clinic.Domain.Data;
using Clinic.Infrastructure.Data.Interfaces;
using Clinic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Clinic.Infrastructure.Data;

/// <summary>
/// Entity Framework data seeder that uses predefined data from DataSeed class.
/// Provides methods for seeding and clearing database data.
/// </summary>
public class EfDataSeeder(AppDbContext context, ILogger<EfDataSeeder> logger, DataSeed data) : IDataSeeder
{
    /// <summary>
    /// Seeds the database with initial test or development data.
    /// </summary>
    public async Task SeedAsync()
    {
        logger.LogInformation("Starting database seeding...");

        try
        {
            await SeedSpecializationsAsync();
            await SeedDoctorsAsync();
            await SeedPatientsAsync();
            await SeedAppointmentsAsync();

            logger.LogInformation("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during database seeding");
            throw;
        }
    }

    /// <summary>
    /// Clears all data from the database.
    /// Removes all entities in the correct order to respect foreign key constraints.
    /// </summary>
    public async Task ClearAsync()
    {
        logger.LogInformation("Clearing all database data...");

        try
        {
            await ClearAppointmentsAsync();
            await ClearPatientsAsync();
            await ClearDoctorsAsync();
            await ClearSpecializationsAsync();

            logger.LogInformation("Database clearing completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during database clearing");
            throw;
        }
    }

    private async Task SeedSpecializationsAsync()
    {
        if (await context.Specializations.AnyAsync())
        {
            logger.LogInformation("Specializations already exist, skipping seeding");
            return;
        }

        logger.LogInformation("Seeding {Count} specializations", data.Specializations.Count);

        foreach (var specialization in data.Specializations)
        {
            var existing = await context.Specializations.FindAsync(specialization.Id);
            if (existing == null)
            {
                context.Specializations.Add(specialization);
            }
        }

        await context.SaveChangesAsync();
        logger.LogInformation("Specializations seeded successfully");
    }

    private async Task SeedDoctorsAsync()
    {
        if (await context.Doctors.AnyAsync())
        {
            logger.LogInformation("Doctors already exist, skipping seeding");
            return;
        }

        logger.LogInformation("Seeding {Count} doctors", data.Doctors.Count);

        foreach (var doctor in data.Doctors)
        {
            var existing = await context.Doctors.FindAsync(doctor.Id);
            if (existing == null)
            {
                var specialization = await context.Specializations.FindAsync(doctor.SpecializationId);
                if (specialization == null)
                {
                    logger.LogWarning("Specialization with ID {SpecializationId} not found for doctor {DoctorId}",
                        doctor.SpecializationId, doctor.Id);
                    continue;
                }

                context.Doctors.Add(doctor);
            }
        }

        await context.SaveChangesAsync();
        logger.LogInformation("Doctors seeded successfully");
    }

    private async Task SeedPatientsAsync()
    {
        if (await context.Patients.AnyAsync())
        {
            logger.LogInformation("Patients already exist, skipping seeding");
            return;
        }

        logger.LogInformation("Seeding {Count} patients", data.Patients.Count);

        foreach (var patient in data.Patients)
        {
            var existing = await context.Patients.FindAsync(patient.Id);
            if (existing == null)
            {
                context.Patients.Add(patient);
            }
        }

        await context.SaveChangesAsync();
        logger.LogInformation("Patients seeded successfully");
    }

    private async Task SeedAppointmentsAsync()
    {
        if (await context.Appointments.AnyAsync())
        {
            logger.LogInformation("Appointments already exist, skipping seeding");
            return;
        }

        logger.LogInformation("Seeding {Count} appointments", data.Appointments.Count);

        foreach (var appointment in data.Appointments)
        {
            var existing = await context.Appointments.FindAsync(appointment.Id);
            if (existing == null)
            {
                var patient = await context.Patients.FindAsync(appointment.PatientId);
                var doctor = await context.Doctors.FindAsync(appointment.DoctorId);

                if (patient == null || doctor == null)
                {
                    logger.LogWarning("Patient with ID {PatientId} or Doctor with ID {DoctorId} not found for appointment {AppointmentId}",
                        appointment.PatientId, appointment.DoctorId, appointment.Id);
                    continue;
                }

                context.Appointments.Add(appointment);
            }
        }

        await context.SaveChangesAsync();
        logger.LogInformation("Appointments seeded successfully");
    }

    private async Task ClearAppointmentsAsync()
    {
        var appointments = await context.Appointments.ToListAsync();
        if (appointments.Any())
        {
            context.Appointments.RemoveRange(appointments);
            await context.SaveChangesAsync();
            logger.LogInformation("Removed {Count} appointments", appointments.Count);
        }
        else
        {
            logger.LogInformation("No appointments to remove");
        }
    }

    private async Task ClearPatientsAsync()
    {
        var patients = await context.Patients.ToListAsync();
        if (patients.Any())
        {
            context.Patients.RemoveRange(patients);
            await context.SaveChangesAsync();
            logger.LogInformation("Removed {Count} patients", patients.Count);
        }
        else
        {
            logger.LogInformation("No patients to remove");
        }
    }

    private async Task ClearDoctorsAsync()
    {
        var doctors = await context.Doctors.ToListAsync();
        if (doctors.Any())
        {
            context.Doctors.RemoveRange(doctors);
            await context.SaveChangesAsync();
            logger.LogInformation("Removed {Count} doctors", doctors.Count);
        }
        else
        {
            logger.LogInformation("No doctors to remove");
        }
    }

    private async Task ClearSpecializationsAsync()
    {
        var specializations = await context.Specializations.ToListAsync();
        if (specializations.Any())
        {
            context.Specializations.RemoveRange(specializations);
            await context.SaveChangesAsync();
            logger.LogInformation("Removed {Count} specializations", specializations.Count);
        }
        else
        {
            logger.LogInformation("No specializations to remove");
        }
    }

    /// <summary>
    /// Resets the database by clearing all data and reseeding.
    /// Useful for testing and development scenarios.
    /// </summary>
    public async Task ResetAsync()
    {
        logger.LogInformation("Resetting database...");
        await ClearAsync();
        await SeedAsync();
        logger.LogInformation("Database reset completed successfully");
    }
}