using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithSubscriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "benefit_packages",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    schedule_weekday = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    schedule_weekend = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_benefit_packages", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "benefits",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    display_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_benefits", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    first_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    last_name = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password_hash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone_number = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    role = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "subscription_plans",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    duration_months = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    benefit_package_id = table.Column<int>(type: "int", nullable: false),
                    is_recurring = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    allow_installments = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    max_installments = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subscription_plans", x => x.id);
                    table.ForeignKey(
                        name: "FK_subscription_plans_benefit_packages_benefit_package_id",
                        column: x => x.benefit_package_id,
                        principalTable: "benefit_packages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "benefit_package_items",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    benefit_package_id = table.Column<int>(type: "int", nullable: false),
                    benefit_id = table.Column<int>(type: "int", nullable: false),
                    value = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_benefit_package_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_benefit_package_items_benefit_packages_benefit_package_id",
                        column: x => x.benefit_package_id,
                        principalTable: "benefit_packages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_benefit_package_items_benefits_benefit_id",
                        column: x => x.benefit_id,
                        principalTable: "benefits",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "date", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clients", x => x.id);
                    table.ForeignKey(
                        name: "FK_clients_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "trainers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    specialization = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    years_of_experience = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<decimal>(type: "decimal(3,2)", nullable: false, defaultValue: 5.0m),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trainers", x => x.id);
                    table.ForeignKey(
                        name: "FK_trainers_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "subscriptions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    client_id = table.Column<int>(type: "int", nullable: false),
                    subscription_plan_id = table.Column<int>(type: "int", nullable: false),
                    start_date = table.Column<DateTime>(type: "DATE", nullable: false),
                    end_date = table.Column<DateTime>(type: "DATE", nullable: true),
                    status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    auto_renew = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subscriptions", x => x.id);
                    table.ForeignKey(
                        name: "FK_subscriptions_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_subscriptions_subscription_plans_subscription_plan_id",
                        column: x => x.subscription_plan_id,
                        principalTable: "subscription_plans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    subscription_id = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    payment_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    installment_number = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    transaction_id = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.id);
                    table.ForeignKey(
                        name: "FK_payments_subscriptions_subscription_id",
                        column: x => x.subscription_id,
                        principalTable: "subscriptions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "idx_benefit_package_item_benefit",
                table: "benefit_package_items",
                column: "benefit_id");

            migrationBuilder.CreateIndex(
                name: "idx_benefit_package_item_package",
                table: "benefit_package_items",
                column: "benefit_package_id");

            migrationBuilder.CreateIndex(
                name: "idx_benefit_package_item_unique",
                table: "benefit_package_items",
                columns: new[] { "benefit_package_id", "benefit_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_benefit_package_is_active",
                table: "benefit_packages",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "idx_benefit_package_name",
                table: "benefit_packages",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_benefit_display_name",
                table: "benefits",
                column: "display_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_benefit_is_active",
                table: "benefits",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "idx_benefit_name",
                table: "benefits",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_clients_user_id",
                table: "clients",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_payment_date",
                table: "payments",
                column: "payment_date");

            migrationBuilder.CreateIndex(
                name: "idx_payment_installment_unique",
                table: "payments",
                columns: new[] { "subscription_id", "installment_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_payment_status",
                table: "payments",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "idx_payment_subscription",
                table: "payments",
                column: "subscription_id");

            migrationBuilder.CreateIndex(
                name: "idx_payment_transaction_unique",
                table: "payments",
                column: "transaction_id",
                unique: true,
                filter: "transaction_id IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "idx_subscription_plan_benefit_package",
                table: "subscription_plans",
                column: "benefit_package_id");

            migrationBuilder.CreateIndex(
                name: "idx_subscription_plan_is_active",
                table: "subscription_plans",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "idx_subscription_plan_type",
                table: "subscription_plans",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "idx_subscription_plan_type_package_active",
                table: "subscription_plans",
                columns: new[] { "type", "benefit_package_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "idx_subscription_client",
                table: "subscriptions",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "idx_subscription_client_plan_status",
                table: "subscriptions",
                columns: new[] { "client_id", "subscription_plan_id", "status" });

            migrationBuilder.CreateIndex(
                name: "idx_subscription_end_date",
                table: "subscriptions",
                column: "end_date");

            migrationBuilder.CreateIndex(
                name: "idx_subscription_plan",
                table: "subscriptions",
                column: "subscription_plan_id");

            migrationBuilder.CreateIndex(
                name: "idx_subscription_status",
                table: "subscriptions",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_trainers_user_id",
                table: "trainers",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "benefit_package_items");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "trainers");

            migrationBuilder.DropTable(
                name: "benefits");

            migrationBuilder.DropTable(
                name: "subscriptions");

            migrationBuilder.DropTable(
                name: "clients");

            migrationBuilder.DropTable(
                name: "subscription_plans");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "benefit_packages");
        }
    }
}
