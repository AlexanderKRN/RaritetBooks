using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaritetBooks.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserActivation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "refresh_token_expiry_time",
                table: "users",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "activation_link",
                table: "users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_activated",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "registration_date",
                table: "sellers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2025, 3, 25, 5, 28, 47, 873, DateTimeKind.Unspecified).AddTicks(88), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTimeOffset(new DateTime(2025, 3, 19, 13, 17, 11, 722, DateTimeKind.Unspecified).AddTicks(101), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "registration_date",
                table: "seller_requests",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2025, 3, 25, 5, 28, 47, 880, DateTimeKind.Unspecified).AddTicks(1151), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTimeOffset(new DateTime(2025, 3, 19, 13, 17, 11, 832, DateTimeKind.Unspecified).AddTicks(7226), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "activation_link",
                table: "users");

            migrationBuilder.DropColumn(
                name: "is_activated",
                table: "users");

            migrationBuilder.AlterColumn<DateTime>(
                name: "refresh_token_expiry_time",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "registration_date",
                table: "sellers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2025, 3, 19, 13, 17, 11, 722, DateTimeKind.Unspecified).AddTicks(101), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTimeOffset(new DateTime(2025, 3, 25, 5, 28, 47, 873, DateTimeKind.Unspecified).AddTicks(88), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "registration_date",
                table: "seller_requests",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2025, 3, 19, 13, 17, 11, 832, DateTimeKind.Unspecified).AddTicks(7226), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTimeOffset(new DateTime(2025, 3, 25, 5, 28, 47, 880, DateTimeKind.Unspecified).AddTicks(1151), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
