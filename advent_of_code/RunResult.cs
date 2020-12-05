namespace AOC
{
    public record RunResult(object Value, long InputTimeMs, long SolveTimeMs)
    {
        public long TotalTimeMs { get => InputTimeMs + SolveTimeMs; }
    };
}
