using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC
{
    public class Calculator
    {
        private Dictionary<char, Func<long, long, long>> _ops;
        private Dictionary<char, int> _precedence;
        private bool _samePrecedence = true;

        public Calculator()
        {
            _ops = new Dictionary<char, Func<long, long, long>>();
            _precedence = new Dictionary<char, int>();
        }

        public long Evaluate(string expression) => Evaluate(ToPostfix(expression));

        public void AddOp(char op, Func<long, long, long> action, int precedence = 0)
        {
            _ops[op] = action;
            _precedence[op] = precedence;
            _samePrecedence = _samePrecedence && precedence == 0;
        }

        private long Evaluate(List<char> postfix)
        {
            var results = new Stack<long>();

            foreach (var c in postfix)
            {
                if (c >= '0' && c <= '9')
                {
                    results.Push(c - '0');
                }
                else
                {
                    long left = results.Pop();
                    long right = results.Pop();

                    results.Push(_ops[c](left, right));
                }
            }

            return results.Pop();
        }

        private List<char> ToPostfix(string expression)
        {
            var postfix = new List<char>();
            var ops = new Stack<char>();

            void Move()
            {
                while (ops.Any() && ops.Peek() != '(')
                {
                    postfix.Add(ops.Pop());
                }
            }

            foreach (var c in expression)
            {
                if (c >= '0' && c <= '9')
                {
                    postfix.Add(c);
                }
                else if (_ops.ContainsKey(c))
                {
                    while (ops.Any() && ops.Peek() != '(' && _precedence[c] <= _precedence[ops.Peek()])
                    {
                        postfix.Add(ops.Pop());
                    }

                    ops.Push(c);
                }
                else if (c == '(')
                {
                    ops.Push(c);
                }
                else if (c == ')')
                {
                    Move();
                    ops.Pop();
                }
            }

            Move();

            return postfix;
        }
    }
}
