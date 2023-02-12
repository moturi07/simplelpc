namespace lpcDAL.ViewModels
{
    public class LoanRepaymentView
    {
        public double LoanAmount { get; set; }
        public int LoanTerm { get; set; }
        public double InterestRate { get; set; }
        public string RepaymentFrequency { get; set; }
        public double TotalInterest { get; set; }
        public double TotalRepayment { get; set; }
        public Repayment[] Repayments { get; set; }

        public bool HasError { get => !string.IsNullOrEmpty(ErrorMessage); }
        public string ErrorMessage { get; set; }
    }
}
