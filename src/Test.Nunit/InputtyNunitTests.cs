namespace Test.Nunit
{
    using System.Collections;
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Touchstone.Core;
    using Touchstone.NunitAdapter;
    using Test.Shared;

    /// <summary>
    /// NUnit host for the shared Inputty Touchstone descriptors.
    ///
    /// Uses the TestCaseSource-driven pattern so that each shared
    /// <see cref="TestCaseDescriptor"/> surfaces as an individual NUnit test. Test logic
    /// lives entirely in <see cref="InputtySuites"/>; this class only adapts it to NUnit.
    ///
    /// The fixture is marked non-parallelizable because the descriptors redirect the
    /// process-wide Console streams and mutate the static DateTimeFormats property.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class InputtyNunitTests
    {
        private static IEnumerable TestCases()
        {
            return new TouchstoneTestCaseSource(InputtySuites.All);
        }

        /// <summary>
        /// Execute a single shared descriptor.
        /// </summary>
        /// <param name="testCase">The descriptor to execute.</param>
        /// <returns>Task.</returns>
        [Test]
        [TestCaseSource(nameof(TestCases))]
        public async Task RunTest(TestCaseDescriptor testCase)
        {
            await testCase.ExecuteAsync(CancellationToken.None);
        }
    }
}
