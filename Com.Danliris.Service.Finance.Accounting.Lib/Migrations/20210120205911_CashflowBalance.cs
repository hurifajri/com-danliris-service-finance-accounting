﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class CashflowBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InitialCashBalances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    UnitId = table.Column<int>(nullable: false),
                    DivisionId = table.Column<int>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    Nominal = table.Column<double>(nullable: false),
                    CurrencyNominal = table.Column<double>(nullable: false),
                    Total = table.Column<double>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InitialCashBalances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RealCashBalances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    UnitId = table.Column<int>(nullable: false),
                    DivisionId = table.Column<int>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    Nominal = table.Column<double>(nullable: false),
                    CurrencyNominal = table.Column<double>(nullable: false),
                    Total = table.Column<double>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealCashBalances", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InitialCashBalances");

            migrationBuilder.DropTable(
                name: "RealCashBalances");
        }
    }
}
