using Clinic.Domain.Enums;
using Clinic.Domain.Models;

namespace Clinic.Infrastructure.Data;

/// <summary>
/// Provides seed data for the Clinic domain entities.
/// This class contains initial data for database seeding and testing purposes.
/// </summary>
public static class DataSeed
{
    /// <summary>
    /// Gets the list of patients for seeding.
    /// </summary>
    public static List<Patient> Patients { get; } =
    [
        new()
        {
            Id = 1,
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
            Id = 2,
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
            Id = 3,
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
            Id = 4,
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
            Id = 5,
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
            Id = 6,
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
            Id = 7,
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
            Id = 8,
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
            Id = 9,
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
            Id = 10,
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
            Id = 11,
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
            Id = 12,
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
            Id = 13,
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
            Id = 14,
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
            Id = 15,
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
    public static List<Doctor> Doctors { get; } =
    [
        new()
        {
            Id = 1,
            FullName = "Смирнов Иван Петрович",
            PassportNumber = "2121 111111",
            YearOfBirth = 1970,
            SpecializationId = 1,
            Experience = 15
        },
        new()
        {
            Id = 2,
            FullName = "Кузнецова Ольга Васильевна",
            PassportNumber = "2222 222222",
            YearOfBirth = 1985,
            SpecializationId = 2,
            Experience = 8
        },
        new()
        {
            Id = 3,
            FullName = "Попов Андрей Михайлович",
            PassportNumber = "3333 333333",
            YearOfBirth = 1978,
            SpecializationId = 3,
            Experience = 12
        },
        new()
        {
            Id = 4,
            FullName = "Соколова Елена Юрьевна",
            PassportNumber = "4444 444444",
            YearOfBirth = 1992,
            SpecializationId = 4,
            Experience = 3
        },
        new()
        {
            Id = 5,
            FullName = "Морозов Дмитрий Алексеевич",
            PassportNumber = "5555 555555",
            YearOfBirth = 1965,
            SpecializationId = 5,
            Experience = 20
        },
        new()
        {
            Id = 6,
            FullName = "Васильева Ирина Сергеевна",
            PassportNumber = "6666 666666",
            YearOfBirth = 1980,
            SpecializationId = 1,
            Experience = 10
        },
        new()
        {
            Id = 7,
            FullName = "Зайцев Алексей Николаевич",
            PassportNumber = "7777 777777",
            YearOfBirth = 1987,
            SpecializationId = 2,
            Experience = 7
        },
        new()
        {
            Id = 8,
            FullName = "Денисов Сергей Владимирович",
            PassportNumber = "8888 888888",
            YearOfBirth = 1973,
            SpecializationId = 3,
            Experience = 18
        },
        new()
        {
            Id = 9,
            FullName = "Ершова Мария Павловна",
            PassportNumber = "9999 999999",
            YearOfBirth = 1995,
            SpecializationId = 4,
            Experience = 2
        },
        new()
        {
            Id = 10,
            FullName = "Ковалев Андрей Романович",
            PassportNumber = "1010 000000",
            YearOfBirth = 1976,
            SpecializationId = 5,
            Experience = 14
        },
        new()
        {
            Id = 11,
            FullName = "Иванова Екатерина Андреевна",
            PassportNumber = "1111 000000",
            YearOfBirth = 1982,
            SpecializationId = 1,
            Experience = 9
        },
        new()
        {
            Id = 12,
            FullName = "Петров Павел Петрович",
            PassportNumber = "1212 000000",
            YearOfBirth = 1990,
            SpecializationId = 2,
            Experience = 5
        }
    ];

    /// <summary>
    /// Gets the list of specializations for seeding.
    /// </summary>
    public static List<Specialization> Specializations { get; } =
    [
        new()
        {
            Id = 1,
            Name = "Терапевт"
        },
        new()
        {
            Id = 2,
            Name = "Хирург"
        },
        new()
        {
            Id = 3,
            Name = "Кардиолог"
        },
        new()
        {
            Id = 4,
            Name = "Невролог"
        },
        new()
        {
            Id = 5,
            Name = "Офтальмолог"
        }
    ];

    /// <summary>
    /// Gets the list of appointments for seeding.
    /// </summary>
    public static List<Appointment> Appointments { get; } =
    [
        new()
        {
            Id = 1,
            PatientId = 1,
            DoctorId = 1,
            DateTime = new DateTime(2025, 10, 6, 9, 0, 0),
            RoomNumber = "101",
            IsRepeated = false
        },
        new()
        {
            Id = 2,
            PatientId = 2,
            DoctorId = 2,
            DateTime = new DateTime(2025, 10, 11, 10, 0, 0),
            RoomNumber = "102",
            IsRepeated = true
        },
        new()
        {
            Id = 3,
            PatientId = 3,
            DoctorId = 3,
            DateTime = new DateTime(2025, 9, 26, 11, 0, 0),
            RoomNumber = "103",
            IsRepeated = false
        },
        new()
        {
            Id = 4,
            PatientId = 4,
            DoctorId = 4,
            DateTime = new DateTime(2025, 9, 25, 11, 0, 0),
            RoomNumber = "104",
            IsRepeated = true
        },
        new()
        {
            Id = 5,
            PatientId = 5,
            DoctorId = 5,
            DateTime = new DateTime(2025, 9, 24, 11, 0, 0),
            RoomNumber = "105",
            IsRepeated = false
        },
        new()
        {
            Id = 6,
            PatientId = 6,
            DoctorId = 1,
            DateTime = new DateTime(2025, 10, 26, 11, 0, 0),
            RoomNumber = "101",
            IsRepeated = true
        },
        new()
        {
            Id = 7,
            PatientId = 7,
            DoctorId = 2,
            DateTime = new DateTime(2025, 9, 20, 11, 0, 0),
            RoomNumber = "102",
            IsRepeated = false
        },
        new()
        {
            Id = 8,
            PatientId = 8,
            DoctorId = 3,
            DateTime = new DateTime(2025, 7, 6, 11, 0, 0),
            RoomNumber = "103",
            IsRepeated = true
        },
        new()
        {
            Id = 9,
            PatientId = 9,
            DoctorId = 4,
            DateTime = new DateTime(2025, 7, 11, 11, 0, 0),
            RoomNumber = "104",
            IsRepeated = false
        },
        new()
        {
            Id = 10,
            PatientId = 10,
            DoctorId = 5,
            DateTime = new DateTime(2025, 8, 22, 11, 0, 0),
            RoomNumber = "105",
            IsRepeated = true
        },
        new()
        {
            Id = 11,
            PatientId = 1,
            DoctorId = 2,
            DateTime = new DateTime(2025, 11, 15, 11, 0, 0),
            RoomNumber = "102",
            IsRepeated = true
        },
        new()
        {
            Id = 12,
            PatientId = 2,
            DoctorId = 3,
            DateTime = new DateTime(2025, 3, 3, 11, 0, 0),
            RoomNumber = "103",
            IsRepeated = false
        },
        new()
        {
            Id = 13,
            PatientId = 3,
            DoctorId = 4,
            DateTime = new DateTime(2024, 9, 26, 11, 0, 0),
            RoomNumber = "104",
            IsRepeated = true
        },
        new()
        {
            Id = 14,
            PatientId = 4,
            DoctorId = 5,
            DateTime = new DateTime(2023, 9, 26, 11, 0, 0),
            RoomNumber = "105",
            IsRepeated = false
        },
        new()
        {
            Id = 15,
            PatientId = 5,
            DoctorId = 1,
            DateTime = new DateTime(2025, 4, 29, 11, 0, 0),
            RoomNumber = "101",
            IsRepeated = true
        },
        new()
        {
            Id = 16,
            PatientId = 11,
            DoctorId = 6,
            DateTime = new DateTime(2024, 4, 29, 11, 0, 0),
            RoomNumber = "106",
            IsRepeated = false
        },
        new()
        {
            Id = 17,
            PatientId = 12,
            DoctorId = 7,
            DateTime = new DateTime(2025, 11, 28, 11, 0, 0),
            RoomNumber = "107",
            IsRepeated = true
        },
        new()
        {
            Id = 18,
            PatientId = 13,
            DoctorId = 8,
            DateTime = new DateTime(2025, 10, 16, 11, 0, 0),
            RoomNumber = "108",
            IsRepeated = false
        },
        new()
        {
            Id = 19,
            PatientId = 14,
            DoctorId = 9,
            DateTime = new DateTime(2025, 10, 3, 11, 0, 0),
            RoomNumber = "109",
            IsRepeated = true
        },
        new()
        {
            Id = 20,
            PatientId = 15,
            DoctorId = 10,
            DateTime = new DateTime(2024, 10, 16, 11, 0, 0),
            RoomNumber = "110",
            IsRepeated = false
        },
        new()
        {
            Id = 21,
            PatientId = 6,
            DoctorId = 11,
            DateTime = new DateTime(2025, 8, 25, 11, 0, 0),
            RoomNumber = "111",
            IsRepeated = true
        },
        new()
        {
            Id = 22,
            PatientId = 7,
            DoctorId = 12,
            DateTime = new DateTime(2025, 4, 17, 11, 0, 0),
            RoomNumber = "112",
            IsRepeated = false
        },
        new()
        {
            Id = 23,
            PatientId = 8,
            DoctorId = 1,
            DateTime = new DateTime(2025, 11, 23, 11, 0, 0),
            RoomNumber = "101",
            IsRepeated = true
        },
        new()
        {
            Id = 24,
            PatientId = 9,
            DoctorId = 2,
            DateTime = new DateTime(2025, 11, 1, 11, 0, 0),
            RoomNumber = "102",
            IsRepeated = false
        },
        new()
        {
            Id = 25,
            PatientId = 10,
            DoctorId = 3,
            DateTime = new DateTime(2024, 11, 1, 11, 0, 0),
            RoomNumber = "103",
            IsRepeated = true
        },
        new()
        {
            Id = 26,
            PatientId = 11,
            DoctorId = 4,
            DateTime = new DateTime(2025, 10, 13, 11, 0, 0),
            RoomNumber = "104",
            IsRepeated = false
        },
        new()
        {
            Id = 27,
            PatientId = 12,
            DoctorId = 5,
            DateTime = new DateTime(2025, 10, 13, 12, 0, 0),
            RoomNumber = "105",
            IsRepeated = true
        },
        new()
        {
            Id = 28,
            PatientId = 13,
            DoctorId = 6,
            DateTime = new DateTime(2025, 10, 13, 13, 0, 0),
            RoomNumber = "106",
            IsRepeated = false
        },
        new()
        {
            Id = 29,
            PatientId = 14,
            DoctorId = 7,
            DateTime = new DateTime(2025, 10, 13, 14, 0, 0),
            RoomNumber = "107",
            IsRepeated = true
        },
        new()
        {
            Id = 30,
            PatientId = 15,
            DoctorId = 8,
            DateTime = new DateTime(2024, 10, 13, 15, 0, 0),
            RoomNumber = "108",
            IsRepeated = false
        }
    ];
}