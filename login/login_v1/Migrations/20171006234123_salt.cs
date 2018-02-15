using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace login_v1.Migrations
{
    public partial class salt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "salt",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "salt",
                table: "User");
        }
    }
}
