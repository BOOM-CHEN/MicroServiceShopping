using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping.Entity.Migrations
{
    /// <inheritdoc />
    public partial class mysql10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "T_User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    UserName = table.Column<string>(type: "longtext", nullable: true),
                    UserEmail = table.Column<string>(type: "longtext", nullable: false),
                    UserPassword = table.Column<string>(type: "longtext", nullable: false),
                    RegisterTime = table.Column<string>(type: "longtext", nullable: false),
                    UserPhoneNumber = table.Column<string>(type: "longtext", nullable: true),
                    UserRecieveAddress = table.Column<string>(type: "longtext", nullable: true),
                    Role = table.Column<string>(type: "longtext", nullable: true),
                    Token = table.Column<string>(type: "longtext", nullable: true),
                    UserAvatar = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_User", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "T_UserPassword",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(128)", nullable: false),
                    PublicKey = table.Column<string>(type: "longtext", nullable: false),
                    IV = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_UserPassword", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_UserPassword_T_User_UserId",
                        column: x => x.UserId,
                        principalTable: "T_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_T_UserPassword_UserId",
                table: "T_UserPassword",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_UserPassword");

            migrationBuilder.DropTable(
                name: "T_User");
        }
    }
}
