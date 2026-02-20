using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "PriorityId",
            //    table: "Tasks",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.CreateTable(
            //    name: "Priorities",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        Color = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
            //        DisplayOrder = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Priorities", x => x.Id);
            //    });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            //migrationBuilder.InsertData(
            //    table: "Priorities",
            //    columns: new[] { "Id", "Color", "DisplayOrder", "Name" },
            //    values: new object[,]
            //    {
            //        { 1, "#DC2626", 1, "Muito Alta" },
            //        { 2, "#F59E0B", 2, "Alta" },
            //        { 3, "#3B82F6", 3, "Média" },
            //        { 4, "#10B981", 4, "Baixa" },
            //        { 5, "#8B5CF6", 5, "Muito Baixa" }
            //    });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "Priorities");

            migrationBuilder.DropTable(
                name: "Users");

            //migrationBuilder.DropColumn(
            //    name: "PriorityId",
            //    table: "Tasks");
        }
    }
}
