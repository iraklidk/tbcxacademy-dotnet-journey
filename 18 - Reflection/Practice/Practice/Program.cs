using Practice;
using System.Reflection;

public class Program
{
    static void Main()
    {
        Console.WriteLine("Loading Calculator...");

        Type calcType = typeof(Calculator);
        object calculator = Activator.CreateInstance(calcType);

        Console.WriteLine("Calculator Methods: ");

        MethodInfo[] methods = calcType.GetMethods();

        // print calculator methods
        foreach(MethodInfo method in methods)
        {
            if(method.Name == "GetHashCode" || method.Name == "Equals" || method.Name == "ToString" || method.Name == "GetType") continue;
            Console.Write($"{method.Name}(");
            ParameterInfo[] parameters = method.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                Console.Write($"{parameters[i].ParameterType.Name} {parameters[i].Name}");
                if (i < parameters.Length - 1) Console.Write(", ");
            }
            Console.WriteLine(")");
        }

        // get the operation to execute from the user
        Console.Write("Choose Operation: ");
        string methodName = Console.ReadLine();
        MethodInfo chosenMethod = Array.Find(methods, m => m.Name.Equals(methodName, StringComparison.OrdinalIgnoreCase));
        if (chosenMethod == null)
        {
            Console.WriteLine("Invalid method.");
            return;
        }

        ParameterInfo[] methodParams = chosenMethod.GetParameters();
        object[] argsForMethod = new object[methodParams.Length];

        // get parameters from the user
        for (int i = 0; i < methodParams.Length; ++i)
        {
            while (true)
            {
                Console.WriteLine($"Enter {methodParams[i].Name} ({methodParams[i].ParameterType.Name}):");
                string input = Console.ReadLine();
                try
                {
                    argsForMethod[i] = Convert.ChangeType(input, methodParams[i].ParameterType);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"invalid input: {e.Message}");
                }
            }
        }

        // execute chosen method and print result
        try
        {
            var result = chosenMethod.Invoke(calculator, argsForMethod);
            Console.WriteLine($"\nMethod \"{chosenMethod.Name}\" Result: {result}");
        }
        catch (Exception outer)
        {
            Console.WriteLine($"Error executing method: {outer.InnerException.Message}");
        }

    }
}