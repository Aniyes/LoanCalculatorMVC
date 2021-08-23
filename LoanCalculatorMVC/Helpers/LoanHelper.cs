using LoanCalculatorMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanCalculatorMVC.Helpers
{
    public class LoanHelper
    {

        public Loan getPayments(Loan loan)
        {

            //Calculate Monthly Payments
            loan.Payment = CalcPayment(loan.Amount, loan.Rate, loan.Term);
            //Create loop from 1 to the term
            var balance = loan.Amount;
            var totalInterest = 0.0m;
            var monthlyInterest = 0.0m;
            var monthlyPrinciple = 0.0m;
            var monthlyRate = CalcMonthlyRate(loan.Rate);
            //loop over each month until Term is reached for loan
            for (int month = 1; month < loan.Term + 1; month++)
            {
                monthlyInterest = CalcMonthlyInterest(balance, monthlyRate);
                totalInterest += monthlyInterest;
                monthlyPrinciple = loan.Payment - monthlyInterest;
                balance -= monthlyPrinciple;


                LoanPayment loanPayment = new();

                loanPayment.Month = month;
                loanPayment.Payment = loan.Payment;
                loanPayment.MonthlyPrinciple = monthlyPrinciple;
                loanPayment.Monthlyinterest = monthlyInterest;
                loanPayment.TotalInterest = totalInterest;
                loanPayment.Balance = balance;

                //push the object into the loan model
                loan.Payments.Add(loanPayment);

            }

            loan.TotalInterest = totalInterest;
            loan.TotalCost = loan.Amount + loan.TotalInterest;

            return loan;

        }

        private decimal CalcPayment(decimal amount, decimal rate, int term)
        {

            decimal monthlyRate = CalcMonthlyRate(rate);
            var rateD = Convert.ToDouble(monthlyRate);
            var amountD = Convert.ToDouble(amount);

            var paymentD = (amountD * rateD) / (1 - Math.Pow(1 + rateD, -term));


            return Convert.ToDecimal(paymentD);
        }

        private decimal CalcMonthlyRate(decimal rate)
        {
            return rate / 1200;

        }

        private decimal CalcMonthlyInterest(decimal balance, decimal monthlyRate)
        {
            return balance * monthlyRate;
        }
    }
}
