/// <summary>
///  This is a simple loans application core web API
/// </summary>
using lpcDAL.Services.IServices;
using lpcDAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace lpcApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoansController : ControllerBase
    {
        private readonly ILoanCalculatorService _loansService;

        public LoansController(ILoanCalculatorService loansService)
        {
            _loansService = loansService;
        }

        /// <summary>
        ///  This is a calculate api that receives loan application data and gives amortization Schedule
        /// </summary>
        /// <remarks>
        /// Sample value of message
        /// {
        /// "loanAmount": 100,
        /// "loanMonths": 12,
        /// "interestRate": 10,
        /// "repaymentFrequency": "Monthly" /*Bi Monthly, Weekly*/
        /// }
        /// </remarks>
        [HttpPost, Route("calculate")]
        public async Task<ActionResult<string>> SaveAccountDataAsync([FromBody] LoanApplicationView model)
        {
            try
            {
                return Ok(_loansService.CalculateLoanPayment(model));
                //return Ok(_loansService.CalculateLoanPayment2(model));
            }
            catch (Exception ex)
            {
                return BadRequest("Error . " + ex.Message);
            }
        }
    }
}
