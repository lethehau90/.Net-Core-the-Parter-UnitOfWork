using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Data.Migrations
{
    public partial class AddErrorPermissionsFunction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Errors",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    StackTrace = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Errors", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Functions",
                columns: table => new
                {
                    ID = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    IconCss = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    ParentId = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    URL = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Functions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Functions_Functions_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Functions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CanCreate = table.Column<bool>(nullable: false),
                    CanDelete = table.Column<bool>(nullable: false),
                    CanRead = table.Column<bool>(nullable: false),
                    CanUpdate = table.Column<bool>(nullable: false),
                    FunctionId = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: true),
                    RoleId = table.Column<string>(maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Permissions_Functions_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "Functions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Permissions_AppRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AppRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Functions_ParentId",
                table: "Functions",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_FunctionId",
                table: "Permissions",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_RoleId",
                table: "Permissions",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Errors");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Functions");
        }
    }
}
