namespace ATMApp
{
    internal class DebitIban : IBAN
    {
        public DebitIban(string owner, string ibanNumber, decimal initialBalance = 0) : base(owner, ibanNumber, initialBalance) { }

        public override void Withdraw(decimal amount)
        {
            try
            {
                if (amount <= 0)
                    throw new InvalidTransactionException("Withdrawal amount can not be negative or 0.");

                if (amount > Balance)
                    throw new InvalidTransactionException("Insufficient funds for debit account.");

                Balance -= amount;
                Console.WriteLine($"{Owner} Withdrew {amount}GEL. New balance: {Balance}GEL. (Debit Card)");
            }
            catch (Exception ex) { Console.WriteLine(ExceptionHelper.GetLastInnerExMessage(ex)); }
        }
    }
}