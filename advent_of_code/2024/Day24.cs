using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;

namespace AOC
{
    public record Instruction202424()
    {
        public string Left { get; set; }
        public string Op { get; set; }
        public string Right { get; set; }
        public string Out { get; set; }
        public bool ReadyToExecute { get; set; }
    }

    public record Input242424()
    {
        public Dictionary<string, int> Input { get; set; }
        public List<Instruction202424> Connections { get; set; }
    }

    [AdventOfCode(2024, 24)]
    public static class Day24_2024
    {
        [MapInput]
        public static Input242424 Map(string[] lines)
        {
            Dictionary<string, int> Input = [];
            List<Instruction202424> Connections = [];

            int i = 0;

            for (i = 0; lines[i] != ""; ++i)
            {
                var l = lines[i].Split(": ");
                Input[l[0]] = int.Parse(l[1]);
            }

            ++i;

            for (; i < lines.Length; ++i)
            {
                var leftRight = lines[i].Split(" -> ");
                var opsAndShit = leftRight[0].Split(" ");

                Connections.Add(new() {
                    Left = opsAndShit[0],
                    Op = opsAndShit[1],
                    Right = opsAndShit[2],
                    Out = leftRight[1],
                    ReadyToExecute = Input.ContainsKey(opsAndShit[0]) && Input.ContainsKey(opsAndShit[2]) 
                });
            }

            return new Input242424(){
                Input = Input,
                Connections = Connections
            };
        }

        public static int RunOp(int l, int r, string op)
        {
            return op switch {
                "AND" => l & r,
                "OR" => l | r,
                "XOR" => l ^ r,
                _ => throw new Exception($"Fuck {op}")
            };
        }

        [Solver(1)]
        public static long Solve1(Input242424 input)
        {
            var executed = new HashSet<string>();
            var nExecutions = 0;
            while (true)
            {
                var toExecute = input.Connections.Where(i => i.ReadyToExecute).ToList();
                if (toExecute.Count == 0)
                {
                    break;
                }
                ++nExecutions;

                foreach (var i in toExecute)
                {
                    input.Input[i.Out] = RunOp(input.Input[i.Left], input.Input[i.Right], i.Op);
                    i.ReadyToExecute = false;

                    executed.Add(i.Left);
                    executed.Add(i.Right);
                }

                foreach (var i in input.Connections)
                {
                    i.ReadyToExecute = input.Input.ContainsKey(i.Left) && input.Input.ContainsKey(i.Right) && !executed.Contains(i.Left);
                }
            }

            Console.WriteLine(nExecutions);

            return Convert.ToInt64(string.Join("", input.Input.Where(c => c.Key.StartsWith('z')).OrderByDescending(c => c.Key).Select(c => c.Value)), 2);
        }

        [Solver(2)]
        public static long Solve2(Input242424 input)
        {

            var min = 1L << 44;
            var max = (1L << 45) - 1;
            var r = new Random();

            while (true)
            {
                var a = r.NextInt64(min, max);
                var b = r.NextInt64(min, max);
                var tmpA = a;
                var tmpB = b;

                input.Input.Clear();

                for (int i = 0; i < 45; ++i)
                {
                    input.Input[$"x{i:00}"] = (int)(tmpA & 1);
                    input.Input[$"y{i:00}"] = (int)(tmpB & 1);

                    tmpA >>= 1;
                    tmpB >>= 1;
                }

                foreach (var i in input.Connections)
                {
                    i.ReadyToExecute = input.Input.ContainsKey(i.Left) && input.Input.ContainsKey(i.Right);
                }

                var xNum = Convert.ToInt64(string.Join("", input.Input.Where(c => c.Key.StartsWith('x')).OrderByDescending(c => c.Key).Select(c => c.Value)), 2);
                var yNum = Convert.ToInt64(string.Join("", input.Input.Where(c => c.Key.StartsWith('y')).OrderByDescending(c => c.Key).Select(c => c.Value)), 2);

                Console.WriteLine($"{xNum}, {yNum}, {a}, {b}");

                var executed = new HashSet<string>();
                while (true)
                {
                    var toExecute = input.Connections.Where(i => i.ReadyToExecute).ToList();
                    if (toExecute.Count == 0)
                    {
                        break;
                    }

                    foreach (var i in toExecute)
                    {
                        input.Input[i.Out] = RunOp(input.Input[i.Left], input.Input[i.Right], i.Op);
                        i.ReadyToExecute = false;

                        executed.Add(i.Left);
                        executed.Add(i.Right);
                    }

                    foreach (var i in input.Connections)
                    {
                        i.ReadyToExecute = input.Input.ContainsKey(i.Left) && input.Input.ContainsKey(i.Right) && !executed.Contains(i.Left);
                    }
                }

                var zNum = Convert.ToInt64(string.Join("", input.Input.Where(c => c.Key.StartsWith('z')).OrderByDescending(c => c.Key).Select(c => c.Value)), 2);
                if (zNum != xNum + yNum || zNum != a + b)
                {
                    Console.WriteLine($"Uh oh 2 {a} + {b} != {zNum}");
                    Console.WriteLine($"Uh oh {xNum:b} + {yNum:b} = {zNum:b} expected {a+b:b}");
                    Console.ReadKey();
                }

            }


            return 1;
        }
    }
}