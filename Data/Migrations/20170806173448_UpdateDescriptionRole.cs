using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class UpdateDescriptionRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppRoleClaims_AppRoles_IdentityRoleId",
                table: "AppRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRoles_AppRoles_IdentityRoleId",
                table: "AppUserRoles");

            migrationBuilder.RenameColumn(
                name: "IdentityRoleId",
                table: "AppUserRoles",
                newName: "AppRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserRoles_IdentityRoleId",
                table: "AppUserRoles",
                newName: "IX_AppUserRoles_AppRoleId");

            migrationBuilder.RenameColumn(
                name: "IdentityRoleId",
                table: "AppRoleClaims",
                newName: "AppRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AppRoleClaims_IdentityRoleId",
                table: "AppRoleClaims",
                newName: "IX_AppRoleClaims_AppRoleId");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AppRoles",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AppRoles",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppRoleClaims_AppRoles_AppRoleId",
                table: "AppRoleClaims",
                column: "AppRoleId",
                principalTable: "AppRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserRoles_AppRoles_AppRoleId",
                table: "AppUserRoles",
                column: "AppRoleId",
                principalTable: "AppRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppRoleClaims_AppRoles_AppRoleId",
                table: "AppRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRoles_AppRoles_AppRoleId",
                table: "AppUserRoles");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AppRoles");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AppRoles");

            migrationBuilder.RenameColumn(
                name: "AppRoleId",
                table: "AppUserRoles",
                newName: "IdentityRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserRoles_AppRoleId",
                table: "AppUserRoles",
                newName: "IX_AppUserRoles_IdentityRoleId");

            migrationBuilder.RenameColumn(
                name: "AppRoleId",
                table: "AppRoleClaims",
                newName: "IdentityRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AppRoleClaims_AppRoleId",
                table: "AppRoleClaims",
                newName: "IX_AppRoleClaims_IdentityRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppRoleClaims_AppRoles_IdentityRoleId",
                table: "AppRoleClaims",
                column: "IdentityRoleId",
                principalTable: "AppRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserRoles_AppRoles_IdentityRoleId",
                table: "AppUserRoles",
                column: "IdentityRoleId",
                principalTable: "AppRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
