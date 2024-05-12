using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorApointment.Data.Migrations
{
    public partial class ver5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("2dd4ec71-5669-42d7-9cf9-bb17220c64c7"),
                column: "ConcurrencyStamp",
                value: "c3a45476-c94b-4dd0-a373-042228dc1064");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("50fe257e-6475-41f0-93f7-f530d622362b"),
                column: "ConcurrencyStamp",
                value: "75ad91f7-2f9a-401f-8002-df0fdf488d1f");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "26c565f6-939a-42d4-aca2-ccff649c5799");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "304c47b0-4379-4dc9-9710-820307e4efd1", "AQAAAAEAACcQAAAAEFRhKxQi+wyeNJ4bQevH3ITD6/H99Pv4jDjZPKybemGtHmPnvLytnch78c//jrbwgw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("2dd4ec71-5669-42d7-9cf9-bb17220c64c7"),
                column: "ConcurrencyStamp",
                value: "7b5668c1-4858-43c4-a93b-effed599ed12");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("50fe257e-6475-41f0-93f7-f530d622362b"),
                column: "ConcurrencyStamp",
                value: "d2235cd2-6b6f-441d-aaa4-eed604954ca1");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "fdee04e8-e416-49d5-b736-914404d320e3");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "59b2f234-6c26-4466-bc34-3fe693d2103c", "AQAAAAEAACcQAAAAEIylYpph3MB54E1FxCfi/Hddmun5wSalt4PsxhtftxL0rFeLQFaIDPZUzeIourdB+g==" });
        }
    }
}
