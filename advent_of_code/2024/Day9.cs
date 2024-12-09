using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOC
{
    public record Input20249(int[] Lines);

    [AdventOfCode(2024, 9)]
    public static class Day9_2024
    {
        [MapInput]
        public static Input20249 Map(string[] lines)
        {
            return new(lines[0].ToCharArray().Select(c => c - '0').ToArray());
        }

        [Solver(1)]
        public static long Solve1(Input20249 input)
        {
            int currentFileId = 0;
            List<int> memory = [];

            for (int i = 0; i < input.Lines.Length; ++i)
            {
                int size = input.Lines[i];

                // File
                if (i % 2 == 0)
                {
                    while (size-- > 0)
                    {
                        memory.Add(currentFileId);
                    }

                    ++currentFileId;
                }
                // Free space
                else
                {
                    while (size-- > 0)
                    {
                        memory.Add(-1);
                    }
                }
            }

            int currentFileIdx = memory.Count - 1;
            int emptySpaceIdx = 0;

            while (memory[currentFileIdx] == -1)
                --currentFileIdx;

            while (memory[emptySpaceIdx] != -1)
                ++emptySpaceIdx;

            while (emptySpaceIdx < currentFileIdx)
            {
                memory[emptySpaceIdx++] = memory[currentFileIdx--];

                while (memory[currentFileIdx] == -1 && emptySpaceIdx <= currentFileIdx)
                    --currentFileIdx;

                while (memory[emptySpaceIdx] != -1 && emptySpaceIdx <= currentFileIdx)
                    ++emptySpaceIdx;
            }

            while (emptySpaceIdx < memory.Count)
            {
                memory[emptySpaceIdx++] = -1;
            }

            return memory.TakeWhile(id => id != -1).Select((id, idx) => id * (long)idx).Sum();
        }

        [Solver(2)]
        public static long Solve2(Input20249 input)
        {
            int currentFileId = 0;
            List<int> memory = [];
            List<(int, int, int)> allFileIds = [];

            for (int i = 0; i < input.Lines.Length; ++i)
            {
                int size = input.Lines[i];

                // File
                if (i % 2 == 0)
                {
                    allFileIds.Add((currentFileId, memory.Count, size));
                    while (size-- > 0)
                    {
                        memory.Add(currentFileId);
                    }

                    ++currentFileId;
                }
                // Free space
                else
                {
                    while (size-- > 0)
                    {
                        memory.Add(-1);
                    }
                }
            }

            allFileIds.Reverse();
            foreach (var file in allFileIds)
            {
                int? emptySpaceStart = FindFirstValidEmptySpace(memory, file.Item2, file.Item3);
                if (emptySpaceStart == null)
                {
                    continue;
                }

                int startOfEmptySpace = emptySpaceStart.Value;
                int startOfFileIdx = file.Item2;
                int endOfFileIdx = startOfFileIdx + file.Item3;
                while(startOfFileIdx < endOfFileIdx)
                {
                    memory[startOfEmptySpace++] = memory[startOfFileIdx];
                    memory[startOfFileIdx++] = -1;
                }
            }

            return memory.Select((id, idx) => {
                if (id == -1) {
                    return 0;
                }

                return id * (long)idx;
            }).Sum();
        }

        private static int? FindFirstValidEmptySpace(List<int> memory, int fileStart, int fileSize)
        {
            int startOfEmptySpaceIdx = memory.FindIndex(mem => mem == -1);
            do
            {
                int endOfEmptySpace = memory.FindIndex(startOfEmptySpaceIdx, mem => mem != -1);

                if (startOfEmptySpaceIdx > fileStart)
                {
                    return null;
                }

                if (endOfEmptySpace - startOfEmptySpaceIdx >= fileSize)
                {
                    return startOfEmptySpaceIdx;
                }

                startOfEmptySpaceIdx = memory.FindIndex(endOfEmptySpace + 1, mem => mem == -1);
            } while (startOfEmptySpaceIdx != -1);

            return null;
        }
    }
}