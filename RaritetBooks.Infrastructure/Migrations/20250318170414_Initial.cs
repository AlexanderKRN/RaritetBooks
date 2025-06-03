using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaritetBooks.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "seller_requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    registration_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2025, 3, 18, 17, 4, 13, 597, DateTimeKind.Unspecified).AddTicks(7746), new TimeSpan(0, 0, 0, 0, 0))),
                    email = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    patronomic = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_seller_requests", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    telegram_id = table.Column<string>(type: "text", nullable: true),
                    whats_app_id = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    permissions = table.Column<string[]>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "admins",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_admins", x => x.id);
                    table.ForeignKey(
                        name: "fk_admins_users_id",
                        column: x => x.id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clients", x => x.id);
                    table.ForeignKey(
                        name: "fk_clients_users_id",
                        column: x => x.id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sellers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    registration_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2025, 3, 18, 17, 4, 13, 583, DateTimeKind.Unspecified).AddTicks(5174), new TimeSpan(0, 0, 0, 0, 0))),
                    rating = table.Column<int>(type: "integer", nullable: true),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    patronomic = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    social_contacts = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sellers", x => x.id);
                    table.ForeignKey(
                        name: "fk_sellers_users_id",
                        column: x => x.id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    author = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    user_seller_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                    table.ForeignKey(
                        name: "fk_products_sellers_user_seller_id",
                        column: x => x.user_seller_id,
                        principalTable: "sellers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "seller_photos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_seller_id = table.Column<Guid>(type: "uuid", nullable: false),
                    path = table.Column<string>(type: "text", nullable: false),
                    is_main = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_seller_photos", x => x.id);
                    table.ForeignKey(
                        name: "fk_seller_photos_sellers_user_seller_id",
                        column: x => x.user_seller_id,
                        principalTable: "sellers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_photos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    path = table.Column<string>(type: "text", nullable: false),
                    is_main = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_photos", x => x.id);
                    table.ForeignKey(
                        name: "fk_product_photos_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_product_photos_product_id",
                table: "product_photos",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_user_seller_id",
                table: "products",
                column: "user_seller_id");

            migrationBuilder.CreateIndex(
                name: "ix_seller_photos_user_seller_id",
                table: "seller_photos",
                column: "user_seller_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admins");

            migrationBuilder.DropTable(
                name: "clients");

            migrationBuilder.DropTable(
                name: "product_photos");

            migrationBuilder.DropTable(
                name: "seller_photos");

            migrationBuilder.DropTable(
                name: "seller_requests");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "sellers");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
