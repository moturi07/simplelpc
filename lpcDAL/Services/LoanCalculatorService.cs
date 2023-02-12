using lpcDAL.Data;
using lpcDAL.Services.IServices;
using lpcDAL.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;

namespace lpcDAL.Services
{
    public class LoanCalculatorService : ILoanCalculatorService
    {
        private readonly ILogger<LoanCalculatorService> _logger;
        public LoanCalculatorService(ILogger<LoanCalculatorService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///  This is a loan calculation service that gives amortization Schedule
        /// </summary>
        public string CalculateLoanPayment(LoanApplicationView model)
        {
            _logger.LogInformation($"Calculate amortization Schedule for : {model}");
            try
            {
                //get loan amount applied for
                double loanAmount = model.LoanAmount;
                //get loan interest rate
                double interestRate = model.InterestRate/100;
                //get the amount of months for the loan to be repayed
                int loanTerm = model.LoanMonths;

                //Get repayment frequencies for each loan
                int repaymentsPerMonth;
                switch (model.RepaymentFrequency)
                {
                    case RepaymentFrequencies.Monthly:
                        repaymentsPerMonth = 1;
                        break;
                    case RepaymentFrequencies.Bi_Monthly:
                        repaymentsPerMonth = 2;
                        break;
                    case RepaymentFrequencies.Weekly:
                        repaymentsPerMonth = 4;
                        break;
                    default:
                        repaymentsPerMonth = 1;
                        break;
                }                              

                //declare empty variables
                double balance = loanAmount;

                //Get Approximate monthly repayment for the loan
                var monthpayment = -Financial.Pmt(interestRate/12, loanTerm, loanAmount);

                //get total repayment for the loan
                double totalAmount = monthpayment* loanTerm;

                //get total interest for the loan
                double totalInterest = totalAmount - loanAmount;

                //declare empty array to hold repayment information for each month
                Repayment[] repayments = new Repayment[(int)loanTerm];
                for (int i = 0; i < (int)loanTerm; i++)
                {
                    //declare empty array to hold repayment schedule information for each month, bi monthly or weekly
                    Installment[] installments = new Installment[(int)repaymentsPerMonth];
                    //declare start date of loan repayment
                    var sstartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(i+1);
                    //get remaining amount after each month repaid
                    var remainingAmount = -Financial.FV(interestRate / 12, i + 1, -monthpayment, loanAmount, 0);
                    //get principal amount for each month
                    var monthlyprincipal = -Financial.PPmt(interestRate / 12, i+1, loanTerm, loanAmount, 0, 0);     
                    
                    //check if remaining amount is less than the repayment
                    if (balance<monthpayment)
                    {
                        monthpayment = balance;
                    }
                    else
                    {                        
                    }
                    balance = balance - monthpayment;
                    repayments[i] = new Repayment
                    {
                        RepaymentMonth = sstartDate.ToString("MMMM yyyy"),
                        RepaymentAmount = RoundUp((monthpayment), 2),
                        Principal = RoundUp(monthlyprincipal, 2),                        
                        Interest = RoundUp(monthpayment - monthlyprincipal, 2),
                        Installments = installments,
                        RemainingBalance = RoundUp(remainingAmount, 2),                        
                    };                    
                    for (int x = 0; x < (int)repaymentsPerMonth; x++)
                    {
                        installments[x] = new Installment
                        {
                            InstallmentId = x + 1,
                            AmountPayable = RoundUp(monthpayment / (int)repaymentsPerMonth, 2),
                        };
                    }
                }
                var result = new LoanRepaymentView
                {
                    LoanAmount = model.LoanAmount,
                    LoanTerm = model.LoanMonths,
                    InterestRate = model.InterestRate,
                    RepaymentFrequency = model.RepaymentFrequency.ToString(),
                    TotalInterest = RoundUp(totalInterest,2),
                    TotalRepayment = RoundUp(totalAmount,2),
                    Repayments = repayments,
                };
                var json = JsonConvert.SerializeObject(result);

                //return json response
                return json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }


        private static double RoundUp(double input, int places)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }
    }
}
