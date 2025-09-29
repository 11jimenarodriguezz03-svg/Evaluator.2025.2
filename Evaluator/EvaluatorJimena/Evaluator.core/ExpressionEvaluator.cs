namespace Evaluator.Core
{
    public class ExpressionEvaluator
    {
        public static double Evaluate(string infix)
        {
            var postfix = InfixToPostfix(infix);
            return Calulate(postfix);
        }

        private static string InfixToPostfix(string infix)
        {
            var stack = new Stack<char>();
            var postfix = string.Empty;
            var number = string.Empty;

            foreach (char item in infix)
            {
                if (char.IsDigit(item) || item == '.')
                {
                    number += item;
                }
                else
                {
                    if (number.Length > 0)
                    {
                        postfix += number + " ";
                        number = string.Empty;
                    }

                    if (IsOperator(item))
                    {
                        if (item == ')')
                        {
                            while (stack.Count > 0 && stack.Peek() != '(')
                            {
                                postfix += stack.Pop() + " ";
                            }
                            if (stack.Count > 0 && stack.Peek() == '(')
                                stack.Pop();
                        }
                        else
                        {
                            while (stack.Count > 0 && item != '(' &&
                                   PriorityInfix(item) <= PriorityStack(stack.Peek()))
                            {
                                postfix += stack.Pop() + " ";
                            }
                            stack.Push(item);
                        }
                    }
                }
            }

            if (number.Length > 0)
                postfix += number + " ";

            while (stack.Count > 0)
            {
                postfix += stack.Pop() + " ";
            }

            return postfix.Trim();
        }

        private static bool IsOperator(char item) =>
            item is '^' or '/' or '*' or '%' or '+' or '-' or '(' or ')';

        private static int PriorityInfix(char op) => op switch
        {
            '^' => 4,
            '*' or '/' or '%' => 2,
            '-' or '+' => 1,
            '(' => 5,
            _ => throw new Exception("Invalid expression."),
        };

        private static int PriorityStack(char op) => op switch
        {
            '^' => 3,
            '*' or '/' or '%' => 2,
            '-' or '+' => 1,
            '(' => 0,
            _ => throw new Exception("Invalid expression."),
        };

        private static double Calulate(string postfix)
        {
            var stack = new Stack<double>();
            var tokens = postfix.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var token in tokens)
            {
                if (double.TryParse(token, System.Globalization.NumberStyles.Any,
                                    System.Globalization.CultureInfo.InvariantCulture, out double num))
                {
                    stack.Push(num);
                }
                else
                {
                    var op2 = stack.Pop();
                    var op1 = stack.Pop();
                    stack.Push(Calulate(op1, token[0], op2));
                }
            }

            return stack.Pop();
        }

        private static double Calulate(double op1, char item, double op2) => item switch
        {
            '*' => op1 * op2,
            '/' => op1 / op2,
            '^' => Math.Pow(op1, op2),
            '+' => op1 + op2,
            '-' => op1 - op2,
            _ => throw new Exception("Invalid expression."),
        };
    }
}
