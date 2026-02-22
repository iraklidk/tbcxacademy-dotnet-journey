namespace ATMApp
{
    internal abstract class IBAN
    {
        public string Owner { get; private set; }
        public string IBANNumber { get; private set; }
        public decimal Balance { get; protected set; }

        public IBAN(string owner, string ibanNumber, decimal initialBalance = 0)
                    => (Owner, IBANNumber,  Balance) = (owner, ibanNumber, initialBalance); 

        public virtual void Deposit(decimal amount)
        {
            if (amount <= 0) throw new InvalidTransactionException($"{Owner}: Deposit amount can not be negative");

            Balance += amount;
            Console.WriteLine($"{Owner} Deposited {amount}GEL. New balance: {Balance}GEL.");
        }

        public abstract void Withdraw(decimal amount);
    }
}