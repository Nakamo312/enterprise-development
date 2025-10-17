namespace Clinic.Tests;

/// <summary>
/// Tests for clinic functionalities.
/// </summary>
public class UnitTests(DataFixture testData) : IClassFixture<DataFixture>
{
    private readonly DataFixture _testData = testData;

    /// <summary>
    /// Tests doctor experience filter.
    /// </summary>
    [Fact]
    public void GetDoctorsExperienceGreater()
    {
        const uint minExperience = 10;

        var expectedFullNames = new List<string>
        {
            "Васильева Ирина Сергеевна",
            "Денисов Сергей Владимирович",
            "Ковалев Андрей Романович",
            "Морозов Дмитрий Алексеевич",
            "Попов Андрей Михайлович",
            "Смирнов Иван Петрович"
        };

        var actualFullNames = _testData.Doctors
            .Where(d => d.Experience >= minExperience)
            .Select(d => d.FullName)
            .Order()
            .ToList();

        Assert.Equal(expectedFullNames, actualFullNames);
    }

    /// <summary>
    /// Tests patient retrieval by doctor.
    /// </summary>
    [Fact]
    public void GetPatientsByDoctorInfo()
    {
        var targetDoctor = _testData.Doctors.First();
        var doctorId = targetDoctor.Id;

        var expectedPatientsFullNames = new List<string>
        {
            "Волкова Елена Андреевна",
            "Иванов Иван Иванович",
            "Кузнецов Дмитрий Сергеевич",
            "Морозова Анна Владимировна"
        };

        var actualPatientsFullNames = _testData.Appointments
            .Where(a => a.DoctorId == doctorId)
            .Join(
                _testData.Patients,
                a => a.PatientId,
                p => p.Id,
                (a, p) => p
            )
            .DistinctBy(p => p.Id)
            .OrderBy(p => p.FullName)
            .Select(p => p.FullName)
            .ToList();

        Assert.Equal(expectedPatientsFullNames, actualPatientsFullNames);
    }

    /// <summary>
    /// Tests retrieval of repeated appointments from the last month.
    /// </summary>
    [Fact]
    public void GetNumberAppointmentsInfo()
    {
        var endDate = new DateTime(2025, 10, 16);
        var startDate = new DateTime(2025, 9, 16);

        var expectedAppointmentIds = new List<uint> { 2, 4, 19, 27, 29 };

        var actualAppointmentIds = _testData.Appointments
            .Where(a => a.IsRepeated && a.DateTime >= startDate && a.DateTime <= endDate)
            .Select(a => a.Id)
            .ToList();

        Assert.Equal(expectedAppointmentIds, actualAppointmentIds);
    }

    /// <summary>
    /// Tests retrieval of patients older than 30 who visited multiple doctors.
    /// </summary>
    [Fact]
    public void GetPatientsInfo()
    {
        var thirtyYearsAgo = new DateOnly(1995, 10, 16);

        var expectedPatientsFullNames = new List<string>
        {
            "Васильев Сергей Михайлович",
            "Волкова Елена Андреевна",
            "Денисова Мария Александровна",
            "Ершов Павел Дмитриевич",
            "Зайцева Ирина Николаевна",
            "Иванов Иван Иванович",
            "Кузнецов Дмитрий Сергеевич",
            "Морозова Анна Владимировна",
            "Петров Петр Петрович",
            "Сидоров Сидор Сидорович",
            "Смирнов Алексей Иванович",
            "Смирнова Екатерина Сергеевна",
            "Соколов Андрей Юрьевич"
        };

        var actualPatientsFullNames = _testData.Appointments
            .GroupBy(a => a.PatientId)
            .Where(g => g.Select(a => a.DoctorId).Distinct().Count() > 1)
            .Select(g => _testData.Patients.First(p => p.Id == g.Key))
            .Where(p => p.DateOfBirth < thirtyYearsAgo)
            .OrderBy(p => p.FullName)
            .Select(p => p.FullName)
            .ToList();

        Assert.Equal(expectedPatientsFullNames, actualPatientsFullNames);
    }

    /// <summary>
    /// Tests retrieval of appointments for the current month in a selected room.
    /// </summary>
    [Fact]
    public void GetAppointmentsInfo()
    {
        var thisMonthStart = new DateTime(2025, 10, 1);
        var thisMonthEnd = new DateTime(2025, 10, 31);
        var selectedRoomNumber = "101";

        var expectedAppointmentIds = new List<uint> { 1, 6 };

        var actualAppointmentIds = _testData.Appointments
            .Where(a => a.DateTime.Date >= thisMonthStart && a.DateTime.Date <= thisMonthEnd && a.RoomNumber == selectedRoomNumber)
            .Select(a => a.Id)
            .ToList();

        Assert.Equal(expectedAppointmentIds, actualAppointmentIds);
    }
}
