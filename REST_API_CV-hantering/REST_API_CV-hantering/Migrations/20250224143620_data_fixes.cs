using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REST_API_CV_hantering.Migrations
{
    /// <inheritdoc />
    public partial class data_fixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "WorkExperiences");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "WorkExperiences",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "WorkExperiences",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "WorkExperiences");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "WorkExperiences");

            migrationBuilder.AddColumn<short>(
                name: "Year",
                table: "WorkExperiences",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }
    }
}
