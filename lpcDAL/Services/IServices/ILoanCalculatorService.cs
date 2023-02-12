using lpcDAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lpcDAL.Services.IServices
{
    public interface ILoanCalculatorService
    {
        string CalculateLoanPayment(LoanApplicationView model);
    }
}
