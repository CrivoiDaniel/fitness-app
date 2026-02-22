using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixUpdatedAtOnUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
ALTER TABLE users 
  MODIFY updated_at datetime(6) NOT NULL 
  DEFAULT CURRENT_TIMESTAMP(6) 
  ON UPDATE CURRENT_TIMESTAMP(6);
");

            migrationBuilder.Sql(@"
ALTER TABLE clients 
  MODIFY updated_at datetime(6) NOT NULL 
  DEFAULT CURRENT_TIMESTAMP(6) 
  ON UPDATE CURRENT_TIMESTAMP(6);
");

            migrationBuilder.Sql(@"
ALTER TABLE trainers 
  MODIFY updated_at datetime(6) NOT NULL 
  DEFAULT CURRENT_TIMESTAMP(6) 
  ON UPDATE CURRENT_TIMESTAMP(6);
");

            migrationBuilder.Sql(@"
ALTER TABLE benefits 
  MODIFY updated_at datetime(6) NOT NULL 
  DEFAULT CURRENT_TIMESTAMP(6) 
  ON UPDATE CURRENT_TIMESTAMP(6);
");

            migrationBuilder.Sql(@"
ALTER TABLE benefit_packages 
  MODIFY updated_at datetime(6) NOT NULL 
  DEFAULT CURRENT_TIMESTAMP(6) 
  ON UPDATE CURRENT_TIMESTAMP(6);
");

            migrationBuilder.Sql(@"
ALTER TABLE benefit_package_items 
  MODIFY updated_at datetime(6) NOT NULL 
  DEFAULT CURRENT_TIMESTAMP(6) 
  ON UPDATE CURRENT_TIMESTAMP(6);
");

            migrationBuilder.Sql(@"
ALTER TABLE subscription_plans 
  MODIFY updated_at datetime(6) NOT NULL 
  DEFAULT CURRENT_TIMESTAMP(6) 
  ON UPDATE CURRENT_TIMESTAMP(6);
");

            migrationBuilder.Sql(@"
ALTER TABLE subscriptions 
  MODIFY updated_at datetime(6) NOT NULL 
  DEFAULT CURRENT_TIMESTAMP(6) 
  ON UPDATE CURRENT_TIMESTAMP(6);
");

            migrationBuilder.Sql(@"
ALTER TABLE payments 
  MODIFY updated_at datetime(6) NOT NULL 
  DEFAULT CURRENT_TIMESTAMP(6) 
  ON UPDATE CURRENT_TIMESTAMP(6);
");
        }
        /// <inheritdoc />
       protected override void Down(MigrationBuilder migrationBuilder)
{
    migrationBuilder.Sql(@"
ALTER TABLE users 
  MODIFY updated_at datetime(6) NOT NULL 
  DEFAULT CURRENT_TIMESTAMP(6);
");
    migrationBuilder.Sql(@"
ALTER TABLE clients 
  MODIFY updated_at datetime(6) NOT NULL 
  DEFAULT CURRENT_TIMESTAMP(6);
");
    migrationBuilder.Sql(@"
ALTER TABLE trainers 
  MODIFY updated_at datetime(6) NOT NULL 
  DEFAULT CURRENT_TIMESTAMP(6);
");
    migrationBuilder.Sql(@"
ALTER TABLE benefits 
  MODIFY updated_at datetime(6) NOT NULL 
  DEFAULT CURRENT_TIMESTAMP(6);
");
    migrationBuilder.Sql(@"
ALTER TABLE benefit_packages 
  MODIFY updated_at datetime(6) NOT NULL 
  DEFAULT CURRENT_TIMESTAMP(6);
");
    migrationBuilder.Sql(@"
ALTER TABLE benefit_package_items 
  MODIFY updated_at datetime(6) NOT NULL 
  DEFAULT CURRENT_TIMESTAMP(6);
");
    migrationBuilder.Sql(@"
ALTER TABLE subscription_plans 
  MODIFY updated_at datetime(6) NOT NULL 
  DEFAULT CURRENT_TIMESTAMP(6);
");
    migrationBuilder.Sql(@"
ALTER TABLE subscriptions 
  MODIFY updated_at datetime(6) NOT NULL 
  DEFAULT CURRENT_TIMESTAMP(6);
");
    migrationBuilder.Sql(@"
ALTER TABLE payments 
  MODIFY updated_at datetime(6) NOT NULL 
  DEFAULT CURRENT_TIMESTAMP(6);
");
}
    }
}
