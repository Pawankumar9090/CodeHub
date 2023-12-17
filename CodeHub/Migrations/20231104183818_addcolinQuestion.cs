﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeHub.Migrations
{
    public partial class addcolinQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Questions");
        }
    }
}
