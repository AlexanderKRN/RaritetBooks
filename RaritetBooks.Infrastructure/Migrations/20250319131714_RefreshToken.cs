using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaritetBooks.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "refresh_token",
                table: "users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "refresh_token_expiry_time",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "registration_date",
                table: "sellers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2025, 3, 19, 13, 17, 11, 722, DateTimeKind.Unspecified).AddTicks(101), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTimeOffset(new DateTime(2025, 3, 18, 17, 4, 13, 583, DateTimeKind.Unspecified).AddTicks(5174), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "registration_date",
                table: "seller_requests",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2025, 3, 19, 13, 17, 11, 832, DateTimeKind.Unspecified).AddTicks(7226), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTimeOffset(new DateTime(2025, 3, 18, 17, 4, 13, 597, DateTimeKind.Unspecified).AddTicks(7746), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "refresh_token",
                table: "users");

            migrationBuilder.DropColumn(
                name: "refresh_token_expiry_time",
                table: "users");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "registration_date",
                table: "sellers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2025, 3, 18, 17, 4, 13, 583, DateTimeKind.Unspecified).AddTicks(5174), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTimeOffset(new DateTime(2025, 3, 19, 13, 17, 11, 722, DateTimeKind.Unspecified).AddTicks(101), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "registration_date",
                table: "seller_requests",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2025, 3, 18, 17, 4, 13, 597, DateTimeKind.Unspecified).AddTicks(7746), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTimeOffset(new DateTime(2025, 3, 19, 13, 17, 11, 832, DateTimeKind.Unspecified).AddTicks(7226), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
