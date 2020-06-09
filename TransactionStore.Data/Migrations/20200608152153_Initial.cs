using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TransactionStore.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "PaymentTransactions",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TransactionId = table.Column<string>(maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(),
                    Currency = table.Column<string>(fixedLength: true, maxLength: 3, nullable: true),
                    TransactionDate = table.Column<DateTime>(),
                    Status = table.Column<int>()
                },
                constraints: table => { table.PrimaryKey("PK_PaymentTransactions", x => x.Id); });

            migrationBuilder.CreateIndex(
                "IX_PaymentTransactions_TransactionId",
                "PaymentTransactions",
                "TransactionId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "PaymentTransactions");
        }
    }
}