namespace AOC
{
    public struct RunResult
    {
        public object Value;
        public long InputTimeEllapsedMs;
        public long SolveTimeEllapsedMs;
        public long TotalTimeEllapsedMs { get => InputTimeEllapsedMs + SolveTimeEllapsedMs; }
    }
}
