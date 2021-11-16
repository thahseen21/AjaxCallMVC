using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AjaxCallMVC.Migrations
{
    public partial class AddEmployeeDOB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EmployeeDOB",
                table: "Employee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeDOB",
                table: "Employee");
        }
    }
}
