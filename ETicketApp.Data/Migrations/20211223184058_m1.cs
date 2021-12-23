﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace ETicketApp.Data.Migrations
{
    public partial class m1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RolesId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RolesId",
                table: "AspNetUsers",
                column: "RolesId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_RolesId",
                table: "AspNetUsers",
                column: "RolesId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_RolesId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RolesId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RolesId",
                table: "AspNetUsers");
        }
    }
}
