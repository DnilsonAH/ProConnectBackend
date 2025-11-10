using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProConnect_Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRevokedTokensTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "revoked_tokens",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    jti = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    revoked_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    expires_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "specialties",
                columns: table => new
                {
                    specialty_id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(600)", maxLength: 600, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.specialty_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(255)", nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password_hash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone_number = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    registration_date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    role = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    photo_url = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.user_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "professional_profiles",
                columns: table => new
                {
                    profile_id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<uint>(type: "int unsigned", nullable: false),
                    specialty_id = table.Column<uint>(type: "int unsigned", nullable: false),
                    experience = table.Column<string>(type: "varchar(600)", maxLength: 600, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    headline = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.profile_id);
                    table.ForeignKey(
                        name: "professional_profiles_specialty_id_foreign",
                        column: x => x.specialty_id,
                        principalTable: "specialties",
                        principalColumn: "specialty_id");
                    table.ForeignKey(
                        name: "professional_profiles_user_id_foreign",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "sessions",
                columns: table => new
                {
                    session_id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    start_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    professional_id = table.Column<uint>(type: "int unsigned", nullable: false),
                    client_id = table.Column<uint>(type: "int unsigned", nullable: false),
                    meet_url = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.session_id);
                    table.ForeignKey(
                        name: "sessions_client_id_foreign",
                        column: x => x.client_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "sessions_professional_id_foreign",
                        column: x => x.professional_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "verifications",
                columns: table => new
                {
                    verification_id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<uint>(type: "int unsigned", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValueSql: "'Pending'", collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    verification_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    notes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.verification_id);
                    table.ForeignKey(
                        name: "verifications_user_id_foreign",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "weekly_availabilities",
                columns: table => new
                {
                    weekly_availability_id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    professional_id = table.Column<uint>(type: "int unsigned", nullable: false),
                    start_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    end_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    week_day = table.Column<string>(type: "varchar(255)", nullable: false, defaultValueSql: "'DEFAULT TRUE'", collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.weekly_availability_id);
                    table.ForeignKey(
                        name: "weekly_availabilities_professional_id_foreign",
                        column: x => x.professional_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    payment_id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    session_id = table.Column<uint>(type: "int unsigned", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValueSql: "'Pending'", collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    method = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    payment_date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.payment_id);
                    table.ForeignKey(
                        name: "payments_session_id_foreign",
                        column: x => x.session_id,
                        principalTable: "sessions",
                        principalColumn: "session_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    review_id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    session_id = table.Column<uint>(type: "int unsigned", nullable: false),
                    client_id = table.Column<uint>(type: "int unsigned", nullable: false),
                    professional_id = table.Column<uint>(type: "int unsigned", nullable: false),
                    rating = table.Column<decimal>(type: "decimal(2,2)", precision: 2, scale: 2, nullable: false),
                    comment = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    review_date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.review_id);
                    table.ForeignKey(
                        name: "reviews_client_id_foreign",
                        column: x => x.client_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "reviews_professional_id_foreign",
                        column: x => x.professional_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "reviews_session_id_foreign",
                        column: x => x.session_id,
                        principalTable: "sessions",
                        principalColumn: "session_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "scheduleds",
                columns: table => new
                {
                    availability_id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    session_id = table.Column<uint>(type: "int unsigned", nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.availability_id);
                    table.ForeignKey(
                        name: "scheduleds_session_id_foreign",
                        column: x => x.session_id,
                        principalTable: "sessions",
                        principalColumn: "session_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateTable(
                name: "verification_documents",
                columns: table => new
                {
                    document_id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    verification_id = table.Column<uint>(type: "int unsigned", nullable: false),
                    document_type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    file_url = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    uploaded_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.document_id);
                    table.ForeignKey(
                        name: "verification_documents_verification_id_foreign",
                        column: x => x.verification_id,
                        principalTable: "verifications",
                        principalColumn: "verification_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateIndex(
                name: "payments_payment_date_index",
                table: "payments",
                column: "payment_date");

            migrationBuilder.CreateIndex(
                name: "payments_session_id_index",
                table: "payments",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "payments_status_index",
                table: "payments",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "payments_status_payment_date_index",
                table: "payments",
                columns: new[] { "status", "payment_date" });

            migrationBuilder.CreateIndex(
                name: "professional_profiles_specialty_id_index",
                table: "professional_profiles",
                column: "specialty_id");

            migrationBuilder.CreateIndex(
                name: "professional_profiles_user_id_unique",
                table: "professional_profiles",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "reviews_client_id_index",
                table: "reviews",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "reviews_professional_id_index",
                table: "reviews",
                column: "professional_id");

            migrationBuilder.CreateIndex(
                name: "reviews_rating_index",
                table: "reviews",
                column: "rating");

            migrationBuilder.CreateIndex(
                name: "reviews_review_date_index",
                table: "reviews",
                column: "review_date");

            migrationBuilder.CreateIndex(
                name: "reviews_session_id_foreign",
                table: "reviews",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "revoked_tokens_expires_at_index",
                table: "revoked_tokens",
                column: "expires_at");

            migrationBuilder.CreateIndex(
                name: "revoked_tokens_jti_unique",
                table: "revoked_tokens",
                column: "jti",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "scheduleds_session_id_foreign",
                table: "scheduleds",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "sessions_client_id_foreign",
                table: "sessions",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "sessions_professional_id_foreign",
                table: "sessions",
                column: "professional_id");

            migrationBuilder.CreateIndex(
                name: "specialties_name_unique",
                table: "specialties",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "users_email_unique",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "users_registration_date_index",
                table: "users",
                column: "registration_date");

            migrationBuilder.CreateIndex(
                name: "verification_documents_document_type_index",
                table: "verification_documents",
                column: "document_type");

            migrationBuilder.CreateIndex(
                name: "verification_documents_uploaded_at_index",
                table: "verification_documents",
                column: "uploaded_at");

            migrationBuilder.CreateIndex(
                name: "verification_documents_verification_id_index",
                table: "verification_documents",
                column: "verification_id");

            migrationBuilder.CreateIndex(
                name: "verifications_created_at_index",
                table: "verifications",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "verifications_status_index",
                table: "verifications",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "verifications_user_id_index",
                table: "verifications",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "verifications_verification_date_index",
                table: "verifications",
                column: "verification_date");

            migrationBuilder.CreateIndex(
                name: "professional_id_start_date_time_end_date_time_index",
                table: "weekly_availabilities",
                columns: new[] { "professional_id", "start_date_time", "end_date_time" });

            migrationBuilder.CreateIndex(
                name: "weekly_availabilities_end_date_time_index",
                table: "weekly_availabilities",
                column: "end_date_time");

            migrationBuilder.CreateIndex(
                name: "weekly_availabilities_professional_id_index",
                table: "weekly_availabilities",
                column: "professional_id");

            migrationBuilder.CreateIndex(
                name: "weekly_availabilities_start_date_time_index",
                table: "weekly_availabilities",
                column: "start_date_time");

            migrationBuilder.CreateIndex(
                name: "weekly_availabilities_week_day_index",
                table: "weekly_availabilities",
                column: "week_day");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "professional_profiles");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "revoked_tokens");

            migrationBuilder.DropTable(
                name: "scheduleds");

            migrationBuilder.DropTable(
                name: "verification_documents");

            migrationBuilder.DropTable(
                name: "weekly_availabilities");

            migrationBuilder.DropTable(
                name: "specialties");

            migrationBuilder.DropTable(
                name: "sessions");

            migrationBuilder.DropTable(
                name: "verifications");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
