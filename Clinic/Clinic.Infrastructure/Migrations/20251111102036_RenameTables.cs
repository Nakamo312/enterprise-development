using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic.Infrastructure.Migrations;

/// <inheritdoc />
public partial class RenameTables : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Appointments_Doctors_DoctorId",
            table: "Appointments");

        migrationBuilder.DropForeignKey(
            name: "FK_Appointments_Patients_PatientId",
            table: "Appointments");

        migrationBuilder.DropForeignKey(
            name: "FK_Doctors_Specializations_SpecializationId",
            table: "Doctors");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Specializations",
            table: "Specializations");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Patients",
            table: "Patients");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Doctors",
            table: "Doctors");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Appointments",
            table: "Appointments");

        migrationBuilder.RenameTable(
            name: "Specializations",
            newName: "specializations");

        migrationBuilder.RenameTable(
            name: "Patients",
            newName: "patients");

        migrationBuilder.RenameTable(
            name: "Doctors",
            newName: "doctors");

        migrationBuilder.RenameTable(
            name: "Appointments",
            newName: "appointments");

        migrationBuilder.RenameColumn(
            name: "Name",
            table: "specializations",
            newName: "name");

        migrationBuilder.RenameColumn(
            name: "Id",
            table: "specializations",
            newName: "id");

        migrationBuilder.RenameColumn(
            name: "Gender",
            table: "patients",
            newName: "gender");

        migrationBuilder.RenameColumn(
            name: "Address",
            table: "patients",
            newName: "address");

        migrationBuilder.RenameColumn(
            name: "Id",
            table: "patients",
            newName: "id");

        migrationBuilder.RenameColumn(
            name: "RhFactor",
            table: "patients",
            newName: "rh_factor");

        migrationBuilder.RenameColumn(
            name: "PassportNumber",
            table: "patients",
            newName: "passport_number");

        migrationBuilder.RenameColumn(
            name: "FullName",
            table: "patients",
            newName: "full_name");

        migrationBuilder.RenameColumn(
            name: "DateOfBirth",
            table: "patients",
            newName: "date_of_birth");

        migrationBuilder.RenameColumn(
            name: "ContactPhone",
            table: "patients",
            newName: "contact_phone");

        migrationBuilder.RenameColumn(
            name: "BloodGroup",
            table: "patients",
            newName: "blood_group");

        migrationBuilder.RenameColumn(
            name: "Experience",
            table: "doctors",
            newName: "experience");

        migrationBuilder.RenameColumn(
            name: "Id",
            table: "doctors",
            newName: "id");

        migrationBuilder.RenameColumn(
            name: "YearOfBirth",
            table: "doctors",
            newName: "year_of_birth");

        migrationBuilder.RenameColumn(
            name: "SpecializationId",
            table: "doctors",
            newName: "specialization_id");

        migrationBuilder.RenameColumn(
            name: "PassportNumber",
            table: "doctors",
            newName: "passport_number");

        migrationBuilder.RenameColumn(
            name: "FullName",
            table: "doctors",
            newName: "full_name");

        migrationBuilder.RenameIndex(
            name: "IX_Doctors_SpecializationId",
            table: "doctors",
            newName: "IX_doctors_specialization_id");

        migrationBuilder.RenameColumn(
            name: "Id",
            table: "appointments",
            newName: "id");

        migrationBuilder.RenameColumn(
            name: "RoomNumber",
            table: "appointments",
            newName: "room_number");

        migrationBuilder.RenameColumn(
            name: "PatientId",
            table: "appointments",
            newName: "patient_id");

        migrationBuilder.RenameColumn(
            name: "IsRepeated",
            table: "appointments",
            newName: "is_repeated");

        migrationBuilder.RenameColumn(
            name: "DoctorId",
            table: "appointments",
            newName: "doctor_id");

        migrationBuilder.RenameColumn(
            name: "DateTime",
            table: "appointments",
            newName: "date_time");

        migrationBuilder.RenameIndex(
            name: "IX_Appointments_PatientId",
            table: "appointments",
            newName: "IX_appointments_patient_id");

        migrationBuilder.RenameIndex(
            name: "IX_Appointments_DoctorId",
            table: "appointments",
            newName: "IX_appointments_doctor_id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_specializations",
            table: "specializations",
            column: "id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_patients",
            table: "patients",
            column: "id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_doctors",
            table: "doctors",
            column: "id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_appointments",
            table: "appointments",
            column: "id");

        migrationBuilder.AddForeignKey(
            name: "FK_appointments_doctors_doctor_id",
            table: "appointments",
            column: "doctor_id",
            principalTable: "doctors",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_appointments_patients_patient_id",
            table: "appointments",
            column: "patient_id",
            principalTable: "patients",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_doctors_specializations_specialization_id",
            table: "doctors",
            column: "specialization_id",
            principalTable: "specializations",
            principalColumn: "id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_appointments_doctors_doctor_id",
            table: "appointments");

        migrationBuilder.DropForeignKey(
            name: "FK_appointments_patients_patient_id",
            table: "appointments");

        migrationBuilder.DropForeignKey(
            name: "FK_doctors_specializations_specialization_id",
            table: "doctors");

        migrationBuilder.DropPrimaryKey(
            name: "PK_specializations",
            table: "specializations");

        migrationBuilder.DropPrimaryKey(
            name: "PK_patients",
            table: "patients");

        migrationBuilder.DropPrimaryKey(
            name: "PK_doctors",
            table: "doctors");

        migrationBuilder.DropPrimaryKey(
            name: "PK_appointments",
            table: "appointments");

        migrationBuilder.RenameTable(
            name: "specializations",
            newName: "Specializations");

        migrationBuilder.RenameTable(
            name: "patients",
            newName: "Patients");

        migrationBuilder.RenameTable(
            name: "doctors",
            newName: "Doctors");

        migrationBuilder.RenameTable(
            name: "appointments",
            newName: "Appointments");

        migrationBuilder.RenameColumn(
            name: "name",
            table: "Specializations",
            newName: "Name");

        migrationBuilder.RenameColumn(
            name: "id",
            table: "Specializations",
            newName: "Id");

        migrationBuilder.RenameColumn(
            name: "gender",
            table: "Patients",
            newName: "Gender");

        migrationBuilder.RenameColumn(
            name: "address",
            table: "Patients",
            newName: "Address");

        migrationBuilder.RenameColumn(
            name: "id",
            table: "Patients",
            newName: "Id");

        migrationBuilder.RenameColumn(
            name: "rh_factor",
            table: "Patients",
            newName: "RhFactor");

        migrationBuilder.RenameColumn(
            name: "passport_number",
            table: "Patients",
            newName: "PassportNumber");

        migrationBuilder.RenameColumn(
            name: "full_name",
            table: "Patients",
            newName: "FullName");

        migrationBuilder.RenameColumn(
            name: "date_of_birth",
            table: "Patients",
            newName: "DateOfBirth");

        migrationBuilder.RenameColumn(
            name: "contact_phone",
            table: "Patients",
            newName: "ContactPhone");

        migrationBuilder.RenameColumn(
            name: "blood_group",
            table: "Patients",
            newName: "BloodGroup");

        migrationBuilder.RenameColumn(
            name: "experience",
            table: "Doctors",
            newName: "Experience");

        migrationBuilder.RenameColumn(
            name: "id",
            table: "Doctors",
            newName: "Id");

        migrationBuilder.RenameColumn(
            name: "year_of_birth",
            table: "Doctors",
            newName: "YearOfBirth");

        migrationBuilder.RenameColumn(
            name: "specialization_id",
            table: "Doctors",
            newName: "SpecializationId");

        migrationBuilder.RenameColumn(
            name: "passport_number",
            table: "Doctors",
            newName: "PassportNumber");

        migrationBuilder.RenameColumn(
            name: "full_name",
            table: "Doctors",
            newName: "FullName");

        migrationBuilder.RenameIndex(
            name: "IX_doctors_specialization_id",
            table: "Doctors",
            newName: "IX_Doctors_SpecializationId");

        migrationBuilder.RenameColumn(
            name: "id",
            table: "Appointments",
            newName: "Id");

        migrationBuilder.RenameColumn(
            name: "room_number",
            table: "Appointments",
            newName: "RoomNumber");

        migrationBuilder.RenameColumn(
            name: "patient_id",
            table: "Appointments",
            newName: "PatientId");

        migrationBuilder.RenameColumn(
            name: "is_repeated",
            table: "Appointments",
            newName: "IsRepeated");

        migrationBuilder.RenameColumn(
            name: "doctor_id",
            table: "Appointments",
            newName: "DoctorId");

        migrationBuilder.RenameColumn(
            name: "date_time",
            table: "Appointments",
            newName: "DateTime");

        migrationBuilder.RenameIndex(
            name: "IX_appointments_patient_id",
            table: "Appointments",
            newName: "IX_Appointments_PatientId");

        migrationBuilder.RenameIndex(
            name: "IX_appointments_doctor_id",
            table: "Appointments",
            newName: "IX_Appointments_DoctorId");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Specializations",
            table: "Specializations",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Patients",
            table: "Patients",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Doctors",
            table: "Doctors",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Appointments",
            table: "Appointments",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Appointments_Doctors_DoctorId",
            table: "Appointments",
            column: "DoctorId",
            principalTable: "Doctors",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Appointments_Patients_PatientId",
            table: "Appointments",
            column: "PatientId",
            principalTable: "Patients",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Doctors_Specializations_SpecializationId",
            table: "Doctors",
            column: "SpecializationId",
            principalTable: "Specializations",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}
