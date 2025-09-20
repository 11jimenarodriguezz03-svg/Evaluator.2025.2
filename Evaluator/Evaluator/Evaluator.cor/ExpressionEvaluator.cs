using System;
using System.Collections.Generic;
using System.Globalization;
namespace Evaluator.Core
{
    public class ExpressionEvaluator
    {
        public static double Evaluate(string infix)
        {
            var postfix = InfixToPostfix(infix);
            return Calculate(postfix);
        }

        private static string InfixToPostfix(string infix)
        {
            var stack = new Stack<char>();
            var postfix = "";
            var number = "";

            foreach (char item in infix)
            {
                if (char.IsDigit(item) || item == '.')
                {
                    number += item;
                }
                else
                {
                    if (!string.IsNullOrEmpty(number))
                    {
                        postfix += number + " ";
                        number = "";
                    }

                    if (IsOperator(item))
                    {
                        if (item == ')')
                        {
                            while (stack.Peek() != '(')
                            {
                                postfix += stack.Pop() + " ";
                            }
                            stack.Pop();
                        }
                        else
                        {
                            while (stack.Count > 0 && PriorityInfix(item) <= PriorityStack(stack.Peek()))
                            {
                                postfix += stack.Pop() + " ";
                            }
                            stack.Push(item);
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(number))
            {
                postfix += number + " ";
            }

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
            '*' or '/' or '%' => 3,
            '+' or '-' => 2,
            '(' => 5,
            _ => throw new Exception("Invalid expression."),
        };

        private static int PriorityStack(char op) => op switch
        {
            '^' => 3,
            '*' or '/' or '%' => 3,
            '+' or '-' => 2,
            '(' => 0,
            _ => throw new Exception("Invalid expression."),
        };

        private static double Calculate(string postfix)
        {
            var stack = new Stack<double>();
            var tokens = postfix.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var token in tokens)
            {
                if (double.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out double number))
                {
                    stack.Push(number);
                }
                else if (token.Length == 1 && IsOperator(token[0]))
                {
                    var op2 = stack.Pop();
                    var op1 = stack.Pop();
                    stack.Push(Calculate(op1, token[0], op2));
                }
                else
                {
                    throw new Exception($"Invalid token: {token}");
                }
            }

            return stack.Pop();
        }

        private static double Calculate(double op1, char item, double op2) => item switch
        {
            '*' => op1 * op2,
            '/' => op1 / op2,
            '^' => Math.Pow(op1, op2),
            '+' => op1 + op2,
            '-' => op1 - op2,
            _ => throw new Exception("Invalid operator."),
        };
    }
}
