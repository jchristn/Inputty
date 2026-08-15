namespace Test.Automated
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Touchstone.Cli;
    using Test.Shared;

    /// <summary>
    /// Touchstone console (CLI) runner for the Inputty test descriptors.
    /// Runs every suite defined in <see cref="InputtySuites"/> and prints a colored,
    /// tabular summary. Returns exit code 0 when all tests pass, 1 when any fails.
    ///
    /// Usage:
    ///   dotnet run --project Test.Automated
    ///   dotnet run --project Test.Automated -- --results results.json
    /// </summary>
    internal static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            string resultsPath = null;

            for (int i = 0; i < args.Length; i++)
            {
                if (String.Equals(args[i], "--results", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    resultsPath = args[i + 1];
                    i++;
                }
            }

            return await ConsoleRunner.RunAsync(
                InputtySuites.All,
                sink: null,
                resultsPath: resultsPath,
                cancellationToken: CancellationToken.None);
        }
    }
}
