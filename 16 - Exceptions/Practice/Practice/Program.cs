using ATMApp;

public class ATM
{
    static void Main()
    {

        User user = new User("Alex", new CreditIban("Alex", "GE123", creditLimit: 2000), new DebitIban("Alex", "GE132", 5000));

        user.DebitAccount.Deposit(500);
        user.DebitAccount.Withdraw(300);
        user.CreditAccount.Deposit(7500);
        user.CreditAccount.Withdraw(700);

        User user2 = new User("Natasha", new CreditIban("Natasha", "GE123", creditLimit: 2000));
        user2.CreditAccount.Deposit(500);
        user2.CreditAccount.Withdraw(7000);

        



        Console.WriteLine("\n===================");
        // EXCEPTION METHODS TESTING
        try
        {
            // generate a chain of exceptions
            ExceptionHelper.CauseException();
        }
        catch (Exception ex)
        {
            Console.WriteLine("All messages: " + ExceptionHelper.GetAllInnerExMessage(ex));
            Console.WriteLine("Last message: " + ExceptionHelper.GetLastInnerExMessage(ex));
        }

    }
}