namespace Test.Xunit
{
    using System.Threading;
    using System.Threading.Tasks;
    using Touchstone.Core;
    using Touchstone.XunitAdapter;
    using Test.Shared;
    using global::Xunit;

    /// <summary>
    /// xUnit host for the shared Inputty Touchstone descriptors.
    ///
    /// Uses the theory-driven pattern so that each shared <see cref="TestCaseDescriptor"/>
    /// surfaces as an individual xUnit test in the runner and CI output. Test logic lives
    /// entirely in <see cref="InputtySuites"/>; this class only adapts it to xUnit.
    ///
    /// Parallelization is disabled (see AssemblyInfo.cs) because the descriptors redirect
    /// the process-wide Console streams and mutate the static DateTimeFormats property.
    /// </summary>
    public sealed class InputtyXunitTests
    {
        /// <summary>
        /// One theory row per non-skipped shared descriptor.
        /// </summary>
        /// <returns>Theory data of test case descriptors.</returns>
        public static TheoryData<TestCaseDescriptor> TestCases()
        {
            return new TouchstoneTheoryData(InputtySuites.All);
        }

        /// <summary>
        /// Execute a single shared descriptor.
        /// </summary>
        /// <param name="testCase">The descriptor to execute.</param>
        /// <returns>Task.</returns>
        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task RunTest(TestCaseDescriptor testCase)
        {
            await testCase.ExecuteAsync(CancellationToken.None);
        }
    }
}
