using System;
using System.Collections.Generic;
using System.Globalization;

namespace Evaluator.Core
{
    public static class ExpressionEvaluator
    {
        public static double Evaluate(string infix)
        {
            var tokens = Tokenize(infix);
            var postfix = ToPostfix(tokens);
            return EvalPostfix(postfix);
        }

        private enum Assoc { Left, Right }

        private readonly struct Token
        {
            public string Kind { get; }
            public string Text { get; }
            public Token(string kind, string text) { Kind = kind; Text = text; }
        }

        private static List<Token> Tokenize(string s)
        {
            var list = new List<Token>();
            int i = 0;

            while (i < s.Length)
            {
                char c = s[i];
                if (char.IsWhiteSpace(c)) { i++; continue; }

                if (char.IsDigit(c) || c == '.' || c == ',')
                {
                    int start = i;
                    i++;
                    while (i < s.Length && (char.IsDigit(s[i]) || s[i] == '.' || s[i] == ',')) i++;
                    var raw = s.Substring(start, i - start).Replace(',', '.');
                    if (!double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out _))
                        throw new Exception($"Invalid number '{raw}'.");
                    list.Add(new Token("num", raw));
                    continue;
                }

                if ("+-*/^()".IndexOf(c) >= 0)
                {
                    if ((c == '+' || c == '-') &&
                        (list.Count == 0 || list[^1].Kind == "op" || list[^1].Kind == "("))
                    {
                        int j = i + 1;
                        while (j < s.Length && char.IsWhiteSpace(s[j])) j++;
                        if (j < s.Length && (char.IsDigit(s[j]) || s[j] == '.' || s[j] == ','))
                        {
                            int start = j; j++;
                            while (j < s.Length && (char.IsDigit(s[j]) || s[j] == '.' || s[j] == ',')) j++;
                            var raw = s.Substring(start, j - start).Replace(',', '.');
                            var signed = (c == '-' ? "-" : "+") + raw;
                            if (!double.TryParse(signed, NumberStyles.Float, CultureInfo.InvariantCulture, out _))
                                throw new Exception($"Invalid number '{signed}'.");
                            list.Add(new Token("num", signed));
                            i = j;
                            continue;
                        }
                    }

                    if (c == '(') { list.Add(new Token("(", "(")); i++; continue; }
                    if (c == ')') { list.Add(new Token(")", ")")); i++; continue; }
                    list.Add(new Token("op", c.ToString()));
                    i++;
                    continue;
                }

                throw new Exception($"Invalid character '{c}'.");
            }

            return list;
        }

        private static int Prec(string op) => op switch
        {
            "^" => 4,
            "*" or "/" => 3,
            "+" or "-" => 2,
            _ => -1
        };

        private static Assoc AssocOf(string op) => op == "^" ? Assoc.Right : Assoc.Left;

        private static List<Token> ToPostfix(List<Token> tokens)
        {
            var output = new List<Token>();
            var ops = new Stack<string>();

            foreach (var t in tokens)
            {
                if (t.Kind == "num")
                {
                    output.Add(t);
                }
                else if (t.Kind == "op")
                {
                    while (ops.Count > 0 && ops.Peek() != "(")
                    {
                        var top = ops.Peek();
                        bool cond =
                            (AssocOf(t.Text) == Assoc.Left && Prec(t.Text) <= Prec(top)) ||
                            (AssocOf(t.Text) == Assoc.Right && Prec(t.Text) < Prec(top));
                        if (cond) output.Add(new Token("op", ops.Pop()));
                        else break;
                    }
                    ops.Push(t.Text);
                }
                else if (t.Kind == "(")
                {
                    ops.Push("(");
                }
                else if (t.Kind == ")")
                {
                    while (ops.Count > 0 && ops.Peek() != "(")
                        output.Add(new Token("op", ops.Pop()));
                    if (ops.Count == 0 || ops.Pop() != "(") throw new Exception("Mismatched parentheses.");
                }
            }

            while (ops.Count > 0)
            {
                var op = ops.Pop();
                if (op == "(" || op == ")") throw new Exception("Mismatched parentheses.");
                output.Add(new Token("op", op));
            }

            return output;
        }

        private static double EvalPostfix(List<Token> postfix)
        {
            var st = new Stack<double>();
            foreach (var t in postfix)
            {
                if (t.Kind == "num")
                {
                    st.Push(double.Parse(t.Text, CultureInfo.InvariantCulture));
                }
                else
                {
                    if (st.Count < 2) throw new Exception("Malformed expression.");
                    var b = st.Pop();
                    var a = st.Pop();
                    st.Push(t.Text switch
                    {
                        "+" => a + b,
                        "-" => a - b,
                        "*" => a * b,
                        "/" => a / b,
                        "^" => Math.Pow(a, b),
                        _ => throw new Exception("Unknown operator")
                    });
                }
            }
            if (st.Count != 1) throw new Exception("Malformed expression.");
            return st.Pop();
        }
    }
}
