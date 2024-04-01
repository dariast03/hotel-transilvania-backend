﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelTransilvania.Migrations
{
    /// <inheritdoc />
    public partial class dos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Reserva",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Reserva");
        }
    }
}
