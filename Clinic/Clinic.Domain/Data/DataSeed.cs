using Clinic.Domain.Enums;
using Clinic.Domain.Models;

namespace Clinic.Domain.Data;

/// <summary>
/// Provides seed data for the Clinic domain entities.
/// This class contains initial data for database seeding and testing purposes.
/// </summary>
public class DataSeed
{
    /// <summary>
    /// Gets the list of patients for seeding.
    /// </summary>
    public List<Patient> Patients { get; } =
    [
        new()
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
            FullName = "Иванов Иван Иванович",
            PassportNumber = "1111 111111",
            DateOfBirth = new DateOnly(1980, 1, 1),
            Address = "г. Москва, ул. Тверская, д. 10",
            ContactPhone = "+7 (495) 123-45-67",
            BloodGroup = BloodGroup.A,
            Gender = Gender.Male,
            RhFactor = RhFactor.P
        },
        new()
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000002"),
            FullName = "Петров Петр Петрович",
            PassportNumber = "2222 222222",
            DateOfBirth = new DateOnly(1990, 2, 2),
            Address = "г. Санкт-Петербург, Невский пр., д. 20",
            ContactPhone = "+7 (812) 234-56-78",
            BloodGroup = BloodGroup.B,
            Gender = Gender.Male,
            RhFactor = RhFactor.N
        },
        new()
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000003"),
            FullName = "Сидоров Сидор Сидорович",
            PassportNumber = "3333 333333",
            DateOfBirth = new DateOnly(1975, 3, 3),
            Address = "г. Екатеринбург, ул. Ленина, д. 30",
            ContactPhone = "+7 (343) 345-67-89",
            BloodGroup = BloodGroup.AB,
            Gender = Gender.Male,
            RhFactor = RhFactor.P
        },
        new()
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000004"),
            FullName = "Смирнов Алексей Иванович",
            PassportNumber = "4444 444444",
            DateOfBirth = new DateOnly(1985, 4, 4),
            Address = "г. Новосибирск, Красный пр., д. 40",
            ContactPhone = "+7 (383) 456-78-90",
            BloodGroup = BloodGroup.O,
            Gender = Gender.Male,
            RhFactor = RhFactor.N
        },
        new()
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000005"),
            FullName = "Кузнецов Дмитрий Сергеевич",
            PassportNumber = "5555 555555",
            DateOfBirth = new DateOnly(1995, 5, 5),
            Address = "г. Казань, ул. Баумана, д. 50",
            ContactPhone = "+7 (843) 567-89-01",
            BloodGroup = BloodGroup.A,
            Gender = Gender.Male,
            RhFactor = RhFactor.P
        },
        new()
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000006"),
            FullName = "Волкова Елена Андреевна",
            PassportNumber = "6666 666666",
            DateOfBirth = new DateOnly(1970, 6, 6),
            Address = "г. Нижний Новгород, ул. Большая Покровская, д. 60",
            ContactPhone = "+7 (831) 678-90-12",
            BloodGroup = BloodGroup.B,
            Gender = Gender.Female,
            RhFactor = RhFactor.N
        },
        new()
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000007"),
            FullName = "Лебедева Ольга Петровна",
            PassportNumber = "7777 777777",
            DateOfBirth = new DateOnly(2000, 7, 7),
            Address = "г. Челябинск, пр. Ленина, д. 70",
            ContactPhone = "+7 (351) 789-01-23",
            BloodGroup = BloodGroup.AB,
            Gender = Gender.Female,
            RhFactor = RhFactor.P
        },
        new()
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000008"),
            FullName = "Морозова Анна Владимировна",
            PassportNumber = "8888 888888",
            DateOfBirth = new DateOnly(1982, 8, 8),
            Address = "г. Самара, ул. Куйбышева, д. 80",
            ContactPhone = "+7 (846) 890-12-34",
            BloodGroup = BloodGroup.O,
            Gender = Gender.Female,
            RhFactor = RhFactor.N
        },
        new()
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000009"),
            FullName = "Соколов Андрей Юрьевич",
            PassportNumber = "9999 999999",
            DateOfBirth = new DateOnly(1988, 9, 9),
            Address = "г. Омск, ул. Ленина, д. 90",
            ContactPhone = "+7 (3812) 901-23-45",
            BloodGroup = BloodGroup.A,
            Gender = Gender.Male,
            RhFactor = RhFactor.P
        },
        new()
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000010"),
            FullName = "Васильев Сергей Михайлович",
            PassportNumber = "1010 101010",
            DateOfBirth = new DateOnly(1977, 10, 10),
            Address = "г. Ростов-на-Дону, ул. Большая Садовая, д. 100",
            ContactPhone = "+7 (863) 012-34-56",
            BloodGroup = BloodGroup.B,
            Gender = Gender.Male,
            RhFactor = RhFactor.N
        },
        new()
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000011"),
            FullName = "Зайцева Ирина Николаевна",
            PassportNumber = "1111 222222",
            DateOfBirth = new DateOnly(1993, 11, 11),
            Address = "г. Уфа, пр. Октября, д. 110",
            ContactPhone = "+7 (347) 123-56-78",
            BloodGroup = BloodGroup.AB,
            Gender = Gender.Female,
            RhFactor = RhFactor.P
        },
        new()
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000012"),
            FullName = "Денисова Мария Александровна",
            PassportNumber = "1212 333333",
            DateOfBirth = new DateOnly(1973, 12, 12),
            Address = "г. Красноярск, пр. Мира, д. 120",
            ContactPhone = "+7 (391) 234-67-89",
            BloodGroup = BloodGroup.O,
            Gender = Gender.Female,
            RhFactor = RhFactor.N
        },
        new()
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000013"),
            FullName = "Ершов Павел Дмитриевич",
            PassportNumber = "1313 444444",
            DateOfBirth = new DateOnly(1983, 1, 13),
            Address = "г. Воронеж, ул. Плехановская, д. 130",
            ContactPhone = "+7 (473) 345-78-90",
            BloodGroup = BloodGroup.A,
            Gender = Gender.Male,
            RhFactor = RhFactor.P
        },
        new()
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000014"),
            FullName = "Ковалев Роман Андреевич",
            PassportNumber = "1414 555555",
            DateOfBirth = new DateOnly(1998, 2, 14),
            Address = "г. Пермь, Комсомольский пр., д. 140",
            ContactPhone = "+7 (342) 456-89-01",
            BloodGroup = BloodGroup.B,
            Gender = Gender.Male,
            RhFactor = RhFactor.N
        },
        new()
        {
            Id = Guid.Parse("10000000-0000-0000-0000-000000000015"),
            FullName = "Смирнова Екатерина Сергеевна",
            PassportNumber = "1515 666666",
            DateOfBirth = new DateOnly(1979, 3, 15),
            Address = "г. Волгоград, пр. Ленина, д. 150",
            ContactPhone = "+7 (8442) 567-90-12",
            BloodGroup = BloodGroup.AB,
            Gender = Gender.Female,
            RhFactor = RhFactor.P
        }
    ];

    /// <summary>
    /// Gets the list of doctors for seeding.
    /// </summary>
    public List<Doctor> Doctors { get; } =
    [
        new()
        {
            Id = Guid.Parse("20000000-0000-0000-0000-000000000001"),
            FullName = "Смирнов Иван Петрович",
            PassportNumber = "2121 111111",
            YearOfBirth = 1970,
            SpecializationId = Guid.Parse("30000000-0000-0000-0000-000000000001"),
            Experience = 15
        },
        new()
        {
            Id = Guid.Parse("20000000-0000-0000-0000-000000000002"),
            FullName = "Кузнецова Ольга Васильевна",
            PassportNumber = "2222 222222",
            YearOfBirth = 1985,
            SpecializationId = Guid.Parse("30000000-0000-0000-0000-000000000002"),
            Experience = 8
        },
        new()
        {
            Id = Guid.Parse("20000000-0000-0000-0000-000000000003"),
            FullName = "Попов Андрей Михайлович",
            PassportNumber = "3333 333333",
            YearOfBirth = 1978,
            SpecializationId = Guid.Parse("30000000-0000-0000-0000-000000000003"),
            Experience = 12
        },
        new()
        {
            Id = Guid.Parse("20000000-0000-0000-0000-000000000004"),
            FullName = "Соколова Елена Юрьевна",
            PassportNumber = "4444 444444",
            YearOfBirth = 1992,
            SpecializationId = Guid.Parse("30000000-0000-0000-0000-000000000004"),
            Experience = 3
        },
        new()
        {
            Id = Guid.Parse("20000000-0000-0000-0000-000000000005"),
            FullName = "Морозов Дмитрий Алексеевич",
            PassportNumber = "5555 555555",
            YearOfBirth = 1965,
            SpecializationId = Guid.Parse("30000000-0000-0000-0000-000000000005"),
            Experience = 20
        },
        new()
        {
            Id = Guid.Parse("20000000-0000-0000-0000-000000000006"),
            FullName = "Васильева Ирина Сергеевна",
            PassportNumber = "6666 666666",
            YearOfBirth = 1980,
            SpecializationId = Guid.Parse("30000000-0000-0000-0000-000000000001"),
            Experience = 10
        },
        new()
        {
            Id = Guid.Parse("20000000-0000-0000-0000-000000000007"),
            FullName = "Зайцев Алексей Николаевич",
            PassportNumber = "7777 777777",
            YearOfBirth = 1987,
            SpecializationId = Guid.Parse("30000000-0000-0000-0000-000000000002"),
            Experience = 7
        },
        new()
        {
            Id = Guid.Parse("20000000-0000-0000-0000-000000000008"),
            FullName = "Денисов Сергей Владимирович",
            PassportNumber = "8888 888888",
            YearOfBirth = 1973,
            SpecializationId = Guid.Parse("30000000-0000-0000-0000-000000000003"),
            Experience = 18
        },
        new()
        {
            Id = Guid.Parse("20000000-0000-0000-0000-000000000009"),
            FullName = "Ершова Мария Павловна",
            PassportNumber = "9999 999999",
            YearOfBirth = 1995,
            SpecializationId = Guid.Parse("30000000-0000-0000-0000-000000000004"),
            Experience = 2
        },
        new()
        {
            Id = Guid.Parse("20000000-0000-0000-0000-000000000010"),
            FullName = "Ковалев Андрей Романович",
            PassportNumber = "1010 000000",
            YearOfBirth = 1976,
            SpecializationId = Guid.Parse("30000000-0000-0000-0000-000000000005"),
            Experience = 14
        },
        new()
        {
            Id = Guid.Parse("20000000-0000-0000-0000-000000000011"),
            FullName = "Иванова Екатерина Андреевна",
            PassportNumber = "1111 000000",
            YearOfBirth = 1982,
            SpecializationId = Guid.Parse("30000000-0000-0000-0000-000000000001"),
            Experience = 9
        },
        new()
        {
            Id = Guid.Parse("20000000-0000-0000-0000-000000000012"),
            FullName = "Петров Павел Петрович",
            PassportNumber = "1212 000000",
            YearOfBirth = 1990,
            SpecializationId = Guid.Parse("30000000-0000-0000-0000-000000000002"),
            Experience = 5
        }
    ];

    /// <summary>
    /// Gets the list of specializations for seeding.
    /// </summary>
    public List<Specialization> Specializations { get; } =
    [
        new()
        {
            Id = Guid.Parse("30000000-0000-0000-0000-000000000001"),
            Name = "Терапевт"
        },
        new()
        {
            Id = Guid.Parse("30000000-0000-0000-0000-000000000002"),
            Name = "Хирург"
        },
        new()
        {
            Id = Guid.Parse("30000000-0000-0000-0000-000000000003"),
            Name = "Кардиолог"
        },
        new()
        {
            Id = Guid.Parse("30000000-0000-0000-0000-000000000004"),
            Name = "Невролог"
        },
        new()
        {
            Id = Guid.Parse("30000000-0000-0000-0000-000000000005"),
            Name = "Офтальмолог"
        }
    ];

    /// <summary>
    /// Gets the list of appointments for seeding.
    /// </summary>
    public List<Appointment> Appointments { get; } =
    [
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000001"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000001"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000001"),
            DateTime = new DateTime(2025, 10, 6, 9, 0, 0, DateTimeKind.Utc),
            RoomNumber = "101",
            IsRepeated = false
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000002"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000002"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000002"),
            DateTime = new DateTime(2025, 10, 11, 10, 0, 0, DateTimeKind.Utc),
            RoomNumber = "102",
            IsRepeated = true
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000003"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000003"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000003"),
            DateTime = new DateTime(2025, 9, 26, 11, 0, 0, DateTimeKind.Utc),
            RoomNumber = "103",
            IsRepeated = false
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000004"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000004"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000004"),
            DateTime = new DateTime(2025, 9, 25, 11, 0, 0, DateTimeKind.Utc),
            RoomNumber = "104",
            IsRepeated = true
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000005"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000005"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000005"),
            DateTime = new DateTime(2025, 9, 24, 11, 0, 0, DateTimeKind.Utc),
            RoomNumber = "105",
            IsRepeated = false
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000006"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000006"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000001"),
            DateTime = new DateTime(2025, 10, 26, 11, 0, 0, DateTimeKind.Utc),
            RoomNumber = "101",
            IsRepeated = true
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000007"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000007"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000002"),
            DateTime = new DateTime(2025, 9, 20, 11, 0, 0, DateTimeKind.Utc),
            RoomNumber = "102",
            IsRepeated = false
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000008"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000008"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000003"),
            DateTime = new DateTime(2025, 7, 6, 11, 0, 0, DateTimeKind.Utc),
            RoomNumber = "103",
            IsRepeated = true
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000009"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000009"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000004"),
            DateTime = new DateTime(2025, 7, 11, 11, 0, 0, DateTimeKind.Utc),
            RoomNumber = "104",
            IsRepeated = false
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000010"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000010"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000005"),
            DateTime = new DateTime(2025, 8, 22, 11, 0, 0, DateTimeKind.Utc),
            RoomNumber = "105",
            IsRepeated = true
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000011"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000001"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000002"),
            DateTime = new DateTime(2025, 11, 15, 11, 0, 0, DateTimeKind.Utc),
            RoomNumber = "102",
            IsRepeated = true
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000012"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000002"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000003"),
            DateTime = new DateTime(2025, 3, 3, 11, 0, 0, DateTimeKind.Utc),
            RoomNumber = "103",
            IsRepeated = false
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000013"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000003"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000004"),
            DateTime = new DateTime(2024, 9, 26, 11, 0, 0, DateTimeKind.Utc),
            RoomNumber = "104",
            IsRepeated = true
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000014"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000004"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000005"),
            DateTime = new DateTime(2023, 9, 26, 11, 0, 0, DateTimeKind.Utc),
            RoomNumber = "105",
            IsRepeated = false
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000015"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000005"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000001"),
            DateTime = new DateTime(2025, 4, 29, 11, 0, 0, DateTimeKind.Utc),
            RoomNumber = "101",
            IsRepeated = true
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000016"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000011"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000006"),
            DateTime = new DateTime(2024, 4, 29, 11, 0, 0, DateTimeKind.Utc),
            RoomNumber = "106",
            IsRepeated = false
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000017"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000012"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000007"),
            DateTime = new DateTime(2025, 11, 28, 11, 0, 0, DateTimeKind.Utc),
            RoomNumber = "107",
            IsRepeated = true
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000018"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000013"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000008"),
            DateTime = new DateTime(2025, 10, 16, 11, 0, 0, DateTimeKind.Utc),
            RoomNumber = "108",
            IsRepeated = false
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000019"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000014"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000009"),
            DateTime = new DateTime(2025, 10, 3, 11, 0, 0, DateTimeKind.Utc),
            RoomNumber = "109",
            IsRepeated = true
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000020"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000015"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000010"),
            DateTime = new DateTime(2024, 10, 16, 11, 0, 0, DateTimeKind.Utc),
            RoomNumber = "110",
            IsRepeated = false
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000021"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000006"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000011"),
            DateTime = new DateTime(2025, 8, 25, 11, 0, 0, DateTimeKind.Utc),
            RoomNumber = "111",
            IsRepeated = true
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000022"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000007"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000012"),
            DateTime = new DateTime(2025, 4, 17, 11, 0, 0, DateTimeKind.Utc),
            RoomNumber = "112",
            IsRepeated = false
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000023"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000008"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000001"),
            DateTime = new DateTime(2025, 11, 23, 11, 0, 0, DateTimeKind.Utc),
            RoomNumber = "101",
            IsRepeated = true
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000024"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000009"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000002"),
            DateTime = new DateTime(2025, 11, 1, 11, 0, 0, DateTimeKind.Utc),
            RoomNumber = "102",
            IsRepeated = false
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000025"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000010"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000003"),
            DateTime = new DateTime(2024, 11, 1, 11, 0, 0, DateTimeKind.Utc),
            RoomNumber = "103",
            IsRepeated = true
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000026"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000011"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000004"),
            DateTime = new DateTime(2025, 10, 13, 11, 0, 0, DateTimeKind.Utc),
            RoomNumber = "104",
            IsRepeated = false
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000027"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000012"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000005"),
            DateTime = new DateTime(2025, 10, 13, 12, 0, 0, DateTimeKind.Utc),
            RoomNumber = "105",
            IsRepeated = true
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000028"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000013"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000006"),
            DateTime = new DateTime(2025, 10, 13, 13, 0, 0, DateTimeKind.Utc),
            RoomNumber = "106",
            IsRepeated = false
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000029"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000014"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000007"),
            DateTime = new DateTime(2025, 10, 13, 14, 0, 0, DateTimeKind.Utc),
            RoomNumber = "107",
            IsRepeated = true
        },
        new()
        {
            Id = Guid.Parse("40000000-0000-0000-0000-000000000030"),
            PatientId = Guid.Parse("10000000-0000-0000-0000-000000000015"),
            DoctorId = Guid.Parse("20000000-0000-0000-0000-000000000008"),
            DateTime = new DateTime(2024, 10, 13, 15, 0, 0, DateTimeKind.Utc),
            RoomNumber = "108",
            IsRepeated = false
        }
    ];
}