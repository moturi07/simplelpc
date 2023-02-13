# simplelpc
This is a simple REST API with a loan calculation function which takes the following input parameters:
1. Loan amount
2. Loan term (in months)
3. Interest rate (per year)
4. Repayment frequency (monthly, bi-monthly, or weekly)

It then outputs the following information:
1. Total interest to be paid over the loan term
2. Total amount to be repaid over the loan term
3. A breakdown of the loan repayments over the loan term, including the principal and interest amounts, and the remaining balance after each repayment.
Assuming that the interest is calculated based on the outstanding balance of the loan, and that the repayments are applied first to the interest and then to the principal.


To run the application please perform the steps below
1. Open powershell
2. Navigate to the project folder
3. build your application by running dotnet build command
4. run your application by dotnet run command
5. Your application should now run preferrably in "https://localhost:44351;http://localhost:34361"