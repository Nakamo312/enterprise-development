using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Clinic.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "Address", "BloodGroup", "ContactPhone", "DateOfBirth", "FullName", "Gender", "PassportNumber", "RhFactor" },
                values: new object[,]
                {
                    { 1L, "г. Москва, ул. Тверская, д. 10", "A", "+7 (495) 123-45-67", new DateOnly(1980, 1, 1), "Иванов Иван Иванович", "Male", "1111 111111", "P" },
                    { 2L, "г. Санкт-Петербург, Невский пр., д. 20", "B", "+7 (812) 234-56-78", new DateOnly(1990, 2, 2), "Петров Петр Петрович", "Male", "2222 222222", "N" },
                    { 3L, "г. Екатеринбург, ул. Ленина, д. 30", "AB", "+7 (343) 345-67-89", new DateOnly(1975, 3, 3), "Сидоров Сидор Сидорович", "Male", "3333 333333", "P" },
                    { 4L, "г. Новосибирск, Красный пр., д. 40", "O", "+7 (383) 456-78-90", new DateOnly(1985, 4, 4), "Смирнов Алексей Иванович", "Male", "4444 444444", "N" },
                    { 5L, "г. Казань, ул. Баумана, д. 50", "A", "+7 (843) 567-89-01", new DateOnly(1995, 5, 5), "Кузнецов Дмитрий Сергеевич", "Male", "5555 555555", "P" },
                    { 6L, "г. Нижний Новгород, ул. Большая Покровская, д. 60", "B", "+7 (831) 678-90-12", new DateOnly(1970, 6, 6), "Волкова Елена Андреевна", "Female", "6666 666666", "N" },
                    { 7L, "г. Челябинск, пр. Ленина, д. 70", "AB", "+7 (351) 789-01-23", new DateOnly(2000, 7, 7), "Лебедева Ольга Петровна", "Female", "7777 777777", "P" },
                    { 8L, "г. Самара, ул. Куйбышева, д. 80", "O", "+7 (846) 890-12-34", new DateOnly(1982, 8, 8), "Морозова Анна Владимировна", "Female", "8888 888888", "N" },
                    { 9L, "г. Омск, ул. Ленина, д. 90", "A", "+7 (3812) 901-23-45", new DateOnly(1988, 9, 9), "Соколов Андрей Юрьевич", "Male", "9999 999999", "P" },
                    { 10L, "г. Ростов-на-Дону, ул. Большая Садовая, д. 100", "B", "+7 (863) 012-34-56", new DateOnly(1977, 10, 10), "Васильев Сергей Михайлович", "Male", "1010 101010", "N" },
                    { 11L, "г. Уфа, пр. Октября, д. 110", "AB", "+7 (347) 123-56-78", new DateOnly(1993, 11, 11), "Зайцева Ирина Николаевна", "Female", "1111 222222", "P" },
                    { 12L, "г. Красноярск, пр. Мира, д. 120", "O", "+7 (391) 234-67-89", new DateOnly(1973, 12, 12), "Денисова Мария Александровна", "Female", "1212 333333", "N" },
                    { 13L, "г. Воронеж, ул. Плехановская, д. 130", "A", "+7 (473) 345-78-90", new DateOnly(1983, 1, 13), "Ершов Павел Дмитриевич", "Male", "1313 444444", "P" },
                    { 14L, "г. Пермь, Комсомольский пр., д. 140", "B", "+7 (342) 456-89-01", new DateOnly(1998, 2, 14), "Ковалев Роман Андреевич", "Male", "1414 555555", "N" },
                    { 15L, "г. Волгоград, пр. Ленина, д. 150", "AB", "+7 (8442) 567-90-12", new DateOnly(1979, 3, 15), "Смирнова Екатерина Сергеевна", "Female", "1515 666666", "P" }
                });

            migrationBuilder.InsertData(
                table: "Specializations",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Терапевт" },
                    { 2L, "Хирург" },
                    { 3L, "Кардиолог" },
                    { 4L, "Невролог" },
                    { 5L, "Офтальмолог" }
                });

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "Id", "Experience", "FullName", "PassportNumber", "SpecializationId", "YearOfBirth" },
                values: new object[,]
                {
                    { 1L, 15L, "Смирнов Иван Петрович", "2121 111111", 1L, 1970L },
                    { 2L, 8L, "Кузнецова Ольга Васильевна", "2222 222222", 2L, 1985L },
                    { 3L, 12L, "Попов Андрей Михайлович", "3333 333333", 3L, 1978L },
                    { 4L, 3L, "Соколова Елена Юрьевна", "4444 444444", 4L, 1992L },
                    { 5L, 20L, "Морозов Дмитрий Алексеевич", "5555 555555", 5L, 1965L },
                    { 6L, 10L, "Васильева Ирина Сергеевна", "6666 666666", 1L, 1980L },
                    { 7L, 7L, "Зайцев Алексей Николаевич", "7777 777777", 2L, 1987L },
                    { 8L, 18L, "Денисов Сергей Владимирович", "8888 888888", 3L, 1973L },
                    { 9L, 2L, "Ершова Мария Павловна", "9999 999999", 4L, 1995L },
                    { 10L, 14L, "Ковалев Андрей Романович", "1010 000000", 5L, 1976L },
                    { 11L, 9L, "Иванова Екатерина Андреевна", "1111 000000", 1L, 1982L },
                    { 12L, 5L, "Петров Павел Петрович", "1212 000000", 2L, 1990L }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "DateTime", "DoctorId", "IsRepeated", "PatientId", "RoomNumber" },
                values: new object[,]
                {
                    { 1L, new DateTime(2025, 10, 6, 9, 0, 0, 0, DateTimeKind.Utc), 1L, false, 1L, "101" },
                    { 2L, new DateTime(2025, 10, 11, 10, 0, 0, 0, DateTimeKind.Utc), 2L, true, 2L, "102" },
                    { 3L, new DateTime(2025, 9, 26, 11, 0, 0, 0, DateTimeKind.Utc), 3L, false, 3L, "103" },
                    { 4L, new DateTime(2025, 9, 25, 11, 0, 0, 0, DateTimeKind.Utc), 4L, true, 4L, "104" },
                    { 5L, new DateTime(2025, 9, 24, 11, 0, 0, 0, DateTimeKind.Utc), 5L, false, 5L, "105" },
                    { 6L, new DateTime(2025, 10, 26, 11, 0, 0, 0, DateTimeKind.Utc), 1L, true, 6L, "101" },
                    { 7L, new DateTime(2025, 9, 20, 11, 0, 0, 0, DateTimeKind.Utc), 2L, false, 7L, "102" },
                    { 8L, new DateTime(2025, 7, 6, 11, 0, 0, 0, DateTimeKind.Utc), 3L, true, 8L, "103" },
                    { 9L, new DateTime(2025, 7, 11, 11, 0, 0, 0, DateTimeKind.Utc), 4L, false, 9L, "104" },
                    { 10L, new DateTime(2025, 8, 22, 11, 0, 0, 0, DateTimeKind.Utc), 5L, true, 10L, "105" },
                    { 11L, new DateTime(2025, 11, 15, 11, 0, 0, 0, DateTimeKind.Utc), 2L, true, 1L, "102" },
                    { 12L, new DateTime(2025, 3, 3, 11, 0, 0, 0, DateTimeKind.Utc), 3L, false, 2L, "103" },
                    { 13L, new DateTime(2024, 9, 26, 11, 0, 0, 0, DateTimeKind.Utc), 4L, true, 3L, "104" },
                    { 14L, new DateTime(2023, 9, 26, 11, 0, 0, 0, DateTimeKind.Utc), 5L, false, 4L, "105" },
                    { 15L, new DateTime(2025, 4, 29, 11, 0, 0, 0, DateTimeKind.Utc), 1L, true, 5L, "101" },
                    { 16L, new DateTime(2024, 4, 29, 11, 0, 0, 0, DateTimeKind.Utc), 6L, false, 11L, "106" },
                    { 17L, new DateTime(2025, 11, 28, 11, 0, 0, 0, DateTimeKind.Utc), 7L, true, 12L, "107" },
                    { 18L, new DateTime(2025, 10, 16, 11, 0, 0, 0, DateTimeKind.Utc), 8L, false, 13L, "108" },
                    { 19L, new DateTime(2025, 10, 3, 11, 0, 0, 0, DateTimeKind.Utc), 9L, true, 14L, "109" },
                    { 20L, new DateTime(2024, 10, 16, 11, 0, 0, 0, DateTimeKind.Utc), 10L, false, 15L, "110" },
                    { 21L, new DateTime(2025, 8, 25, 11, 0, 0, 0, DateTimeKind.Utc), 11L, true, 6L, "111" },
                    { 22L, new DateTime(2025, 4, 17, 11, 0, 0, 0, DateTimeKind.Utc), 12L, false, 7L, "112" },
                    { 23L, new DateTime(2025, 11, 23, 11, 0, 0, 0, DateTimeKind.Utc), 1L, true, 8L, "101" },
                    { 24L, new DateTime(2025, 11, 1, 11, 0, 0, 0, DateTimeKind.Utc), 2L, false, 9L, "102" },
                    { 25L, new DateTime(2024, 11, 1, 11, 0, 0, 0, DateTimeKind.Utc), 3L, true, 10L, "103" },
                    { 26L, new DateTime(2025, 10, 13, 11, 0, 0, 0, DateTimeKind.Utc), 4L, false, 11L, "104" },
                    { 27L, new DateTime(2025, 10, 13, 12, 0, 0, 0, DateTimeKind.Utc), 5L, true, 12L, "105" },
                    { 28L, new DateTime(2025, 10, 13, 13, 0, 0, 0, DateTimeKind.Utc), 6L, false, 13L, "106" },
                    { 29L, new DateTime(2025, 10, 13, 14, 0, 0, 0, DateTimeKind.Utc), 7L, true, 14L, "107" },
                    { 30L, new DateTime(2024, 10, 13, 15, 0, 0, 0, DateTimeKind.Utc), 8L, false, 15L, "108" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 16L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 17L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 18L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 19L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 21L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 22L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 23L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 24L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 25L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 26L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 27L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 28L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 29L);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 30L);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "Id",
                keyValue: 5L);
        }
    }
}
