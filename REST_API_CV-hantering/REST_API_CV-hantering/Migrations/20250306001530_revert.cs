﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REST_API_CV_hantering.Migrations
{
    /// <inheritdoc />
    public partial class revert : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkExpereinceId",
                table: "WorkExperiences",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "WorkExperiences",
                newName: "WorkExpereinceId");
        }
    }
}
