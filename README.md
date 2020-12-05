# General

These are my solutions to the Advent of Code puzzles.

I've chosen C# because .NET 5 is the new shiny, and Visual Studio made it
pretty easy to get rolling with it. I'm not actually sure if I'm going to end
up making use of any C# 9 features, but .NET 5 is supposed to have a pretty
massive speed-up over previous versions, so solutions go brrr.

Anyways, if you'd like to run this you'll need the .NET 5 SDK installed.

The dotnet cli can be used to run. From the root:

```
dotnet run -p ./advent_of_code [year] [day] [puzzle]
```

From Visual Studio:

1. Right click the advent_of_code project and select Properties.
2. Navigate to Debug.
3. Update command line arguments to desired puzzle.

# Goals

* Solve the problems.
* Write clear code that communicates the intent.
* Use nifty constructs when applicable.
* Code should go brrr for the most part.

# Contributions

I will accept contributions for spelling errors and things along those lines,
but probably won't accept any contributions that overhaul the code in any major
way.
