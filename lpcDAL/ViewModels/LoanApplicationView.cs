using lpcDAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lpcDAL.ViewModels
{
    public class LoanApplicationView
    {        
        public double LoanAmount { get; set; } = 0;
        public int LoanMonths { get; set; }
        public double InterestRate { get; set; }        
        public RepaymentFrequencies RepaymentFrequency { get; set; }
    }
}
