namespace Test.Shared
{
    using System;
    using System.IO;

    /// <summary>
    /// Helper that redirects Console.In/Console.Out so that the console-driven
    /// Inputty methods can be exercised deterministically in tests.
    /// </summary>
    public static class ConsoleHarness
    {
        /// <summary>
        /// Run a function against Inputty while feeding it the supplied console input.
        /// Each element of <paramref name="lines"/> represents a single line the
        /// user would type (as if followed by pressing Enter).
        /// </summary>
        /// <typeparam name="T">Return type of the function under test.</typeparam>
        /// <param name="func">The Inputty call to execute.</param>
        /// <param name="lines">The lines of input to supply, in order.</param>
        /// <returns>The value returned by <paramref name="func"/>.</returns>
        public static T Run<T>(Func<T> func, params string[] lines)
        {
            TextReader originalIn = Console.In;
            TextWriter originalOut = Console.Out;

            try
            {
                string input = lines == null || lines.Length == 0
                    ? string.Empty
                    : string.Join(Environment.NewLine, lines) + Environment.NewLine;

                Console.SetIn(new StringReader(input));
                Console.SetOut(new StringWriter());

                return func();
            }
            finally
            {
                Console.SetIn(originalIn);
                Console.SetOut(originalOut);
            }
        }
    }
}
