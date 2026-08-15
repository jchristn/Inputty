using Xunit;

// Inputty drives the process-wide Console.In / Console.Out. Redirecting those
// from multiple tests at once would race, so run tests sequentially.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
