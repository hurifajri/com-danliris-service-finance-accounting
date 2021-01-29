﻿using Com.Danliris.Service.Finance.Accounting.Lib.Models.BudgetCashflow;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow
{
    public class BudgetCashflowDivisionUnitItemDto
    {
        public BudgetCashflowDivisionUnitItemDto()
        {
        }

        public BudgetCashflowDivisionUnitItemDto(BudgetCashflowUnitModel cashflowUnit, UnitDto divisionUnit)
        {
            CashflowUnit = cashflowUnit;
            if (cashflowUnit != null)
            {
                Nominal = cashflowUnit.Nominal;
                CurrencyNominal = cashflowUnit.CurrencyNominal;
                Actual = cashflowUnit.Total;
            }
            Unit = divisionUnit;
        }

        public BudgetCashflowDivisionUnitItemDto(DivisionDto division, double divisionCurrencyNominal, double divisionNominal, double divisionActual)
        {
            Division = division;
            CurrencyNominal = divisionCurrencyNominal;
            Nominal = divisionNominal;
            Actual = divisionActual;
        }

        public BudgetCashflowDivisionUnitItemDto(DivisionDto division, UnitDto divisionUnit, double nominal, double currencyNominal, double actual)
        {
            Division = division;
            Unit = divisionUnit;
            Nominal = nominal;
            CurrencyNominal = currencyNominal;
            Actual = actual;
        }

        public BudgetCashflowUnitModel CashflowUnit { get; private set; }
        public UnitDto Unit { get; private set; }
        public double Nominal { get; private set; }
        public double CurrencyNominal { get; private set; }
        public double Actual { get; private set; }
        public DivisionDto Division { get; private set; }
    }
}