namespace ATMApp
{
    internal class CreditIban : IBAN
    {
        public CreditIban(string owner, string ibanNumber, decimal initialBalance = 0, decimal creditLimit = 5000) : base(owner, ibanNumber, initialBalance)
                         => CreditLimit = creditLimit;
        public decimal CreditLimit { get; private set; }
        public override void Withdraw(decimal amount)
        {
            try
            {
                if (amount <= 0)
                    throw new InvalidTransactionException($"{Owner} Withdrawal amount can not be negative or 0.");

                if (Balance - amount < -1 * CreditLimit)
                    throw new InvalidTransactionException($"{Owner} Credit limit exceeded!");

                Balance -= amount;
                Console.WriteLine($"{Owner} Withdrew {amount}GEL. New balance: {Balance}GEL. Credit limit: {CreditLimit} (Credit Card)");
            }
            catch (Exception ex) { Console.WriteLine(ExceptionHelper.GetLastInnerExMessage(ex)); }
        }

    }
}