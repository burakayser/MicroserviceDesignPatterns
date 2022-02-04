using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SagaStateMachineWorker.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderStateInstance",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuyerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Payment_CardName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Payment_CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Payment_Expiration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Payment_CVV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Payment_TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStateInstance", x => x.CorrelationId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderStateInstance");
        }
    }
}
