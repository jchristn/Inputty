using Xunit;

// The shared descriptors redirect the process-wide Console.In/Console.Out streams
// and mutate the static Inputty.DateTimeFormats property, so test cases must not run
// concurrently.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
