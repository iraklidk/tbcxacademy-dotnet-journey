using MyCollections;

public class Program
{
    static bool IsBalancedBrackets(string userInput)
    {
        int length = userInput.Length;
        MyStack<char> stack = new MyStack<char>();
        for (int i = 0; i < length; ++i)
        {
            char c = userInput[i];
            if (c == '(' || c == '{' || c == '[') stack.Push(c);
            else if (stack.Length == 0 || (stack.Peek() == '(' && c != ')') ||
                   (stack.Peek() == '{' && c != '}') || (stack.Peek() == '[' && c != ']')) return false;
            else stack.Pop();
        }

        return stack.Length == 0;
    }
    static void Main1()
    {
        string[] testCases = {
        "()", "()[]{}", "(]", "([)]", "{[]}", "", "(((((((", "))))))",
        "([]{})", "([{})", "((()))", "{[()]}", "({[)]})", "()[{}]",
        "[({})]", "(([]){})", "{[}]", "(){[({})]}", "((({{{[[[]]]}}})))",
        "({}[]())", "({}[(])", "((()()()))", "((({{{}}})))", "[[[[[]]]]]",
        "{[()]()[]}", "((([[[{{}}}]])))", "({[({[]})]})", "([[[[]]]])",
        "{[({})]([])}", "([(){}])", "(({{[[]]}}))", "([)]()", "({[]}){}",
        "({[({[()]})]})", "(((((((((())))))))))", "[]{}(){}[]", "({}[]({}))",
        "{[()()]}", "({[]})", "([({})])", "((({{}})))", "([][{}])", "(([]){})"
        };

        foreach (string testCase in testCases) Console.WriteLine("=== " + testCase + (IsBalancedBrackets(testCase) ?
            " Balanced Brackets ===\n" : " Unbalanced Brackets ===\n"));

    }
}

// ([(([[)