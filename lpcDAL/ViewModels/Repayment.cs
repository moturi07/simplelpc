namespace lpcDAL.ViewModels
{
    public class Repayment
    {
        public string RepaymentMonth { get; set; }
        public double RepaymentAmount { get; set; }
        public double Principal { get; set; }
        public double Interest { get; set; }        
        public Installment[] Installments { get; set; }
        public double RemainingBalance { get; set; }
    }
    public class Installment
    {
        public int InstallmentId { get; set; }
        public double AmountPayable { get; set; }
    }
}
