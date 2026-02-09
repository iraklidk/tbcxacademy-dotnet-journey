namespace ATMApp
{
    internal class User
    {
        public string Name { get; private set; }
        public IBAN DebitAccount { get; private set; }
        public IBAN CreditAccount { get; private set; }

        public User(string name, IBAN creditAcc = null, IBAN debitAcc = null) 
        {
            Name = name;
            CreditAccount = creditAcc ?? new CreditIban(Name, "GE1");
            DebitAccount = debitAcc ?? new DebitIban(Name, "GE2");
        }
    }
}