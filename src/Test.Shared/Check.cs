namespace Test.Shared
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Exception thrown when a Touchstone assertion fails. Touchstone captures the
    /// full exception (message and stack trace) and surfaces it in every runner.
    /// </summary>
    public sealed class AssertionException : Exception
    {
        /// <summary>
        /// Initialize a new assertion exception.
        /// </summary>
        /// <param name="message">Failure message.</param>
        public AssertionException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Minimal, runner-agnostic assertion helpers used by the shared Touchstone
    /// test descriptors. Failures throw <see cref="AssertionException"/> so that no
    /// dependency on any specific test framework is required in the shared library.
    /// </summary>
    public static class Check
    {
        /// <summary>
        /// Assert that two values are equal using the default equality comparer.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="expected">Expected value.</param>
        /// <param name="actual">Actual value.</param>
        public static void Equal<T>(T expected, T actual)
        {
            if (!EqualityComparer<T>.Default.Equals(expected, actual))
                throw new AssertionException(
                    "Expected <" + Describe(expected) + "> but got <" + Describe(actual) + ">.");
        }

        /// <summary>
        /// Assert that two reference values are the same instance.
        /// </summary>
        /// <param name="expected">Expected instance.</param>
        /// <param name="actual">Actual instance.</param>
        public static void Same(object expected, object actual)
        {
            if (!ReferenceEquals(expected, actual))
                throw new AssertionException("Expected the same instance but the references differ.");
        }

        /// <summary>
        /// Assert that a condition is true.
        /// </summary>
        /// <param name="condition">Condition to evaluate.</param>
        /// <param name="message">Optional failure message.</param>
        public static void True(bool condition, string message = null)
        {
            if (!condition)
                throw new AssertionException(message ?? "Expected condition to be true but it was false.");
        }

        /// <summary>
        /// Assert that a condition is false.
        /// </summary>
        /// <param name="condition">Condition to evaluate.</param>
        /// <param name="message">Optional failure message.</param>
        public static void False(bool condition, string message = null)
        {
            if (condition)
                throw new AssertionException(message ?? "Expected condition to be false but it was true.");
        }

        /// <summary>
        /// Assert that a value is null.
        /// </summary>
        /// <param name="value">Value to evaluate.</param>
        public static void Null(object value)
        {
            if (value != null)
                throw new AssertionException("Expected null but got <" + Describe(value) + ">.");
        }

        /// <summary>
        /// Assert that a value is not null.
        /// </summary>
        /// <param name="value">Value to evaluate.</param>
        public static void NotNull(object value)
        {
            if (value == null)
                throw new AssertionException("Expected a non-null value but got null.");
        }

        /// <summary>
        /// Assert that a sequence is empty.
        /// </summary>
        /// <param name="collection">Sequence to evaluate.</param>
        public static void Empty(IEnumerable collection)
        {
            if (collection == null)
                throw new AssertionException("Expected an empty collection but got null.");

            IEnumerator e = collection.GetEnumerator();
            try
            {
                if (e.MoveNext())
                    throw new AssertionException("Expected an empty collection but it contained elements.");
            }
            finally
            {
                (e as IDisposable)?.Dispose();
            }
        }

        /// <summary>
        /// Assert that a sequence contains at least one element.
        /// </summary>
        /// <param name="collection">Sequence to evaluate.</param>
        public static void NotEmpty(IEnumerable collection)
        {
            if (collection == null)
                throw new AssertionException("Expected a non-empty collection but got null.");

            IEnumerator e = collection.GetEnumerator();
            try
            {
                if (!e.MoveNext())
                    throw new AssertionException("Expected a non-empty collection but it was empty.");
            }
            finally
            {
                (e as IDisposable)?.Dispose();
            }
        }

        /// <summary>
        /// Assert that two sequences contain equal elements in the same order.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="expected">Expected sequence.</param>
        /// <param name="actual">Actual sequence.</param>
        public static void Sequence<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            if (expected == null || actual == null)
            {
                if (!ReferenceEquals(expected, actual))
                    throw new AssertionException("Expected sequences to match but one was null.");
                return;
            }

            List<T> e = new List<T>(expected);
            List<T> a = new List<T>(actual);

            if (e.Count != a.Count)
                throw new AssertionException("Expected " + e.Count + " element(s) but got " + a.Count + ".");

            for (int i = 0; i < e.Count; i++)
            {
                if (!EqualityComparer<T>.Default.Equals(e[i], a[i]))
                    throw new AssertionException(
                        "Sequences differ at index " + i + ": expected <" + Describe(e[i]) +
                        "> but got <" + Describe(a[i]) + ">.");
            }
        }

        /// <summary>
        /// Assert that the supplied action throws an exception of the expected type
        /// (or a derived type).
        /// </summary>
        /// <typeparam name="TException">Expected exception type.</typeparam>
        /// <param name="action">Action expected to throw.</param>
        public static void Throws<TException>(Action action) where TException : Exception
        {
            try
            {
                action();
            }
            catch (TException)
            {
                return;
            }
            catch (Exception ex)
            {
                throw new AssertionException(
                    "Expected exception of type " + typeof(TException).Name +
                    " but got " + ex.GetType().Name + ": " + ex.Message);
            }

            throw new AssertionException(
                "Expected exception of type " + typeof(TException).Name + " but no exception was thrown.");
        }

        private static string Describe(object value)
        {
            return value == null ? "null" : value.ToString();
        }
    }
}
