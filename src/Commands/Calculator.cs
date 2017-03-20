using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myConsole.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class Evaluate
    {
        public static string eval(string expression)
        {
            string result = "0"; //calculated expression (double converted to string) or Errormessage starting with "ERROR" (+ optional Errormessage)
            var x = ParseAsRPN(expression);
            foreach (var y in x)
            {
                Console.WriteLine(y);
            }
            double res;
            try
            {
                res = evaluate(x);
                return res.ToString();
            }
            catch
            {
                return "ERROR";
            }
        }

        public static Queue<string> ParseAsRPN(string expression)
        {
            var output = new Queue<string>();
            var opStack = new Stack<string>();
            string[] tokens = Regex.Split(expression, @"(\(|\)|(?<!e|E)-|(?<!e|E)\+|\*|/|&|\s+)");
            Console.WriteLine(expression);
            foreach (var token in tokens)
            {
                double a;
                if (String.IsNullOrWhiteSpace(token)) continue;
                else if (Double.TryParse(token, out a))
                {
                    output.Enqueue(token);
                }
                else if (token == "(") opStack.Push(token);
                else if (token == ")")
                {
                    string nextToken;
                    do
                    {
                        nextToken = opStack.Peek();
                        if (nextToken != "(")
                        {
                            output.Enqueue(opStack.Pop());
                        }
                        else
                        {
                            nextToken = opStack.Pop();
                        }

                    } while (nextToken != "(");
                }
                else
                {
                    //functions
                    if (token.Length > 1)
                    {
                        opStack.Push(token);
                        continue;
                    }
                    //operators
                    if (ops.ContainsKey(token))
                    {
                        //exponation always popped to opstack
                        if (opStack.Count == 0 || token == "&")
                        {
                            opStack.Push(token);
                        }
                        else
                        {
                            while (LowerPrecedence(token, opStack))
                            {
                                output.Enqueue(opStack.Pop());
                            }
                            opStack.Push(token);

                        }

                    }
                };
            }
            var length = opStack.Count;
            for (var i = 0; i < length; i++)
            {
                output.Enqueue(opStack.Pop());
            }
            return output;

        }

        public static bool LowerPrecedence(string currentOperator, Stack<string> operators)
        {
            var o1Precedence = OpPrecedence[currentOperator];
            var nextToken = (operators.Count > 0) ? operators.Peek() : "";
            if (funcs.ContainsKey(nextToken) || nextToken == "" || nextToken == "("
                || nextToken == ")") return false;
            return o1Precedence <= OpPrecedence[nextToken];

        }

        public static Dictionary<string, Func<double, double>> funcs = new Dictionary<string, Func<double, double>>
        {
            ["log"] = (num) => Math.Log(num, 10),
            ["ln"] = (num) => Math.Log(num),
            ["exp"] = (num) => Math.Exp(num),
            ["sqrt"] = (num) => Math.Sqrt(num),
            ["abs"] = (num) => Math.Abs(num),
            ["atan"] = (num) => Math.Atan(num),
            ["acos"] = (num) => Math.Acos(num),
            ["asin"] = (num) => Math.Asin(num),
            ["sinh"] = (num) => Math.Sinh(num),
            ["cosh"] = (num) => Math.Cosh(num),
            ["tanh"] = (num) => Math.Tanh(num),
            ["tan"] = (num) => Math.Tan(num),
            ["sin"] = (num) => Math.Sin(num),
            ["cos"] = (num) => Math.Cos(num),

        };
        public static Dictionary<string, Func<double, double, double>> ops =
    new Dictionary<string, Func<double, double, double>> {
            { "+", (a, b) => a + b },
            { "-", (a, b) => a - b },
            { "*", (a, b) => a * b },
            { "/", (a, b) => a / b },
            { "&", (a, b) => Math.Pow(a, b) },
    };

        public static Dictionary<string, int> OpPrecedence = new Dictionary<string, int>()
        {
            ["+"] = 0,
            ["-"] = 0,
            ["/"] = 1,
            ["*"] = 1,
            ["&"] = 2,
        };
        public static double evaluate(Queue<string> expr)
        {
            var st = new Stack<double>();
            foreach (var s in expr)
            {
                double a, b;
                if (Double.TryParse(s, out a))
                {
                    st.Push(a);
                    continue;
                }
                if (ops.ContainsKey(s))
                {
                    b = st.Pop();
                    a = st.Pop();
                    st.Push(ops[s](a, b));
                }
                else
                {
                    b = st.Pop();
                    st.Push(funcs[s](b));
                }
            }
            return st.Pop();
        }
    }

}

