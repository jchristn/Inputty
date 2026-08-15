namespace Inputty.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using Xunit;
    using Sut = GetSomeInput.Inputty;

    /// <summary>
    /// Unit tests for the Inputty library. Console input is supplied via
    /// <see cref="ConsoleHarness"/>, which redirects Console.In/Console.Out.
    /// Each test documents whether it exercises a positive (happy path) or a
    /// negative (invalid input / retry / exception) scenario.
    /// </summary>
    public class InputtyTests
    {
        #region GetBoolean

        [Theory]
        [InlineData("y", true)]
        [InlineData("yes", true)]
        [InlineData("1", true)]
        public void GetBoolean_FalseDefault_AffirmativeInput_ReturnsTrue(string input, bool expected)
        {
            // Positive: explicit affirmative answers when the default is false.
            bool result = ConsoleHarness.Run(() => Sut.GetBoolean("Proceed?", false), input);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("n")]
        [InlineData("no")]
        [InlineData("0")]
        public void GetBoolean_TrueDefault_NegativeInput_ReturnsFalse(string input)
        {
            // Positive: explicit negative answers when the default is true.
            bool result = ConsoleHarness.Run(() => Sut.GetBoolean("Proceed?", true), input);
            Assert.False(result);
        }

        [Fact]
        public void GetBoolean_EmptyInput_ReturnsDefault()
        {
            // Positive: empty input falls back to the supplied default.
            Assert.True(ConsoleHarness.Run(() => Sut.GetBoolean("Proceed?", true), ""));
            Assert.False(ConsoleHarness.Run(() => Sut.GetBoolean("Proceed?", false), ""));
        }

        [Fact]
        public void GetBoolean_UnrecognizedInput_ResolvesToDefaultBias()
        {
            // Negative: unrecognized tokens are not affirmative/negative keywords.
            // With a true default, anything not negative resolves to true; with a
            // false default, anything not affirmative resolves to false.
            Assert.True(ConsoleHarness.Run(() => Sut.GetBoolean("Proceed?", true), "maybe"));
            Assert.False(ConsoleHarness.Run(() => Sut.GetBoolean("Proceed?", false), "maybe"));
        }

        #endregion

        #region GetString

        [Fact]
        public void GetString_NonEmptyInput_ReturnsInput()
        {
            // Positive.
            string result = ConsoleHarness.Run(() => Sut.GetString("Name?", null, false), "Joel");
            Assert.Equal("Joel", result);
        }

        [Fact]
        public void GetString_EmptyInput_ReturnsDefault()
        {
            // Positive: empty input yields the default answer.
            string result = ConsoleHarness.Run(() => Sut.GetString("Name?", "Anonymous", false), "");
            Assert.Equal("Anonymous", result);
        }

        [Fact]
        public void GetString_EmptyInput_AllowNull_ReturnsNull()
        {
            // Positive: null is permitted when there is no default.
            string result = ConsoleHarness.Run(() => Sut.GetString("Name?", null, true), "");
            Assert.Null(result);
        }

        [Fact]
        public void GetString_EmptyThenValue_NoNullAllowed_RetriesUntilNonEmpty()
        {
            // Negative: empty input is rejected (no default, null disallowed), so
            // the prompt loops until a non-empty value is supplied.
            string result = ConsoleHarness.Run(() => Sut.GetString("Name?", null, false), "", "", "Joel");
            Assert.Equal("Joel", result);
        }

        #endregion

        #region GetInteger

        [Fact]
        public void GetInteger_ValidInput_ReturnsValue()
        {
            // Positive.
            int result = ConsoleHarness.Run(() => Sut.GetInteger("Age?", 42, false, true), "7");
            Assert.Equal(7, result);
        }

        [Fact]
        public void GetInteger_EmptyInput_ReturnsDefault()
        {
            // Positive.
            int result = ConsoleHarness.Run(() => Sut.GetInteger("Age?", 42, false, true), "");
            Assert.Equal(42, result);
        }

        [Fact]
        public void GetInteger_NonNumericThenValid_Retries()
        {
            // Negative: non-numeric input is rejected and re-prompted.
            int result = ConsoleHarness.Run(() => Sut.GetInteger("Age?", 42, false, true), "abc", "13");
            Assert.Equal(13, result);
        }

        [Fact]
        public void GetInteger_NegativeRejectedWhenPositiveOnly_Retries()
        {
            // Negative: a negative value is rejected when positiveOnly is set.
            int result = ConsoleHarness.Run(() => Sut.GetInteger("Age?", 1, true, false), "-5", "9");
            Assert.Equal(9, result);
        }

        #endregion

        #region GetDecimal

        [Fact]
        public void GetDecimal_ValidInput_ReturnsValue()
        {
            // Positive.
            decimal result = ConsoleHarness.Run(() => Sut.GetDecimal("Amount?", 1.5m, false, true), "3.25");
            Assert.Equal(3.25m, result);
        }

        [Fact]
        public void GetDecimal_EmptyInput_ReturnsDefault()
        {
            // Positive.
            decimal result = ConsoleHarness.Run(() => Sut.GetDecimal("Amount?", 1.5m, false, true), "");
            Assert.Equal(1.5m, result);
        }

        [Fact]
        public void GetDecimal_NonNumericThenValid_Retries()
        {
            // Negative.
            decimal result = ConsoleHarness.Run(() => Sut.GetDecimal("Amount?", 1.5m, false, true), "xyz", "2.75");
            Assert.Equal(2.75m, result);
        }

        #endregion

        #region GetDouble

        [Fact]
        public void GetDouble_ValidInput_ReturnsValue()
        {
            // Positive.
            double result = ConsoleHarness.Run(() => Sut.GetDouble("Amount?", 1.5, false, true), "3.5");
            Assert.Equal(3.5, result);
        }

        [Fact]
        public void GetDouble_EmptyInput_ReturnsDefault()
        {
            // Positive.
            double result = ConsoleHarness.Run(() => Sut.GetDouble("Amount?", 9.9, false, true), "");
            Assert.Equal(9.9, result);
        }

        [Fact]
        public void GetDouble_NonNumericThenValid_Retries()
        {
            // Negative.
            double result = ConsoleHarness.Run(() => Sut.GetDouble("Amount?", 1.0, false, true), "notanumber", "4.2");
            Assert.Equal(4.2, result);
        }

        #endregion

        #region GetStringList

        [Fact]
        public void GetStringList_CollectsUntilEmptyLine()
        {
            // Positive.
            List<string> result = ConsoleHarness.Run(() => Sut.GetStringList("Item?", true), "a", "b", "c", "");
            Assert.Equal(new List<string> { "a", "b", "c" }, result);
        }

        [Fact]
        public void GetStringList_EmptyImmediately_AllowEmpty_ReturnsEmptyList()
        {
            // Positive.
            List<string> result = ConsoleHarness.Run(() => Sut.GetStringList("Item?", true), "");
            Assert.Empty(result);
        }

        [Fact]
        public void GetStringList_EmptyImmediately_DisallowEmpty_RequiresAtLeastOne()
        {
            // Negative: an empty first entry is rejected until at least one value
            // is provided, after which a subsequent empty line terminates input.
            List<string> result = ConsoleHarness.Run(() => Sut.GetStringList("Item?", false), "", "only", "");
            Assert.Equal(new List<string> { "only" }, result);
        }

        #endregion

        #region GetDictionary / GetNameValueCollection

        [Fact]
        public void GetDictionary_BuildsFromKeyValuePairs()
        {
            // Positive: an empty key terminates collection.
            Dictionary<string, string> result = ConsoleHarness.Run(
                () => Sut.GetDictionary<string, string>("Key:", "Value:"),
                "k1", "v1", "k2", "v2", "");

            Assert.Equal(2, result.Count);
            Assert.Equal("v1", result["k1"]);
            Assert.Equal("v2", result["k2"]);
        }

        [Fact]
        public void GetDictionary_TypedValue_ConvertsFromString()
        {
            // Positive: values are converted to the requested type.
            Dictionary<string, int> result = ConsoleHarness.Run(
                () => Sut.GetDictionary<string, int>("Key:", "Value:"),
                "one", "1", "");

            Assert.Equal(1, result["one"]);
        }

        [Fact]
        public void GetNameValueCollection_CaseInsensitive_BuildsCollection()
        {
            // Positive: case-insensitive lookups succeed regardless of key casing.
            NameValueCollection result = ConsoleHarness.Run(
                () => Sut.GetNameValueCollection("Key:", "Value:", true),
                "Alpha", "1", "Beta", "2", "");

            Assert.Equal("1", result["alpha"]);
            Assert.Equal("2", result["BETA"]);
        }

        #endregion

        #region GetDateTime

        [Fact]
        public void GetDateTime_ValidIsoDate_Parses()
        {
            // Positive.
            DateTime result = ConsoleHarness.Run(() => Sut.GetDateTime("When?"), "2024-01-15 13:30:00");
            Assert.Equal(new DateTime(2024, 1, 15, 13, 30, 0), result);
        }

        [Fact]
        public void GetDateTime_CustomFormat_Parses()
        {
            // Positive: a value matching one of the DateTimeFormats entries parses.
            // No timezone designator is used, so no local-time conversion occurs.
            DateTime result = ConsoleHarness.Run(() => Sut.GetDateTime("When?"), "06/15/2024 09:45");
            Assert.Equal(new DateTime(2024, 6, 15, 9, 45, 0), result);
        }

        [Fact]
        public void GetDateTime_Unparseable_ThrowsFormatException()
        {
            // Negative: a non-empty, unparseable value throws.
            Assert.Throws<FormatException>(
                () => ConsoleHarness.Run(() => Sut.GetDateTime("When?"), "definitely-not-a-date"));
        }

        [Fact]
        public void GetDateTime_WithDefault_EmptyInput_ReturnsDefault()
        {
            // Positive: empty input re-parses the default answer.
            DateTime dflt = new DateTime(2020, 6, 1, 12, 0, 0);
            DateTime result = ConsoleHarness.Run(() => Sut.GetDateTime("When?", dflt), "");
            Assert.Equal(dflt, result);
        }

        #endregion

        #region GetNullableDateTime

        [Fact]
        public void GetNullableDateTime_EmptyInput_ReturnsNull()
        {
            // Positive.
            DateTime? result = ConsoleHarness.Run(() => Sut.GetNullableDateTime("When?"), "");
            Assert.Null(result);
        }

        [Fact]
        public void GetNullableDateTime_ValidInput_ReturnsValue()
        {
            // Positive.
            DateTime? result = ConsoleHarness.Run(() => Sut.GetNullableDateTime("When?"), "2024-01-15 00:00:00");
            Assert.Equal(new DateTime(2024, 1, 15), result);
        }

        [Fact]
        public void GetNullableDateTime_Unparseable_ThrowsFormatException()
        {
            // Negative.
            Assert.Throws<FormatException>(
                () => ConsoleHarness.Run(() => Sut.GetNullableDateTime("When?"), "not-a-date"));
        }

        [Fact]
        public void GetNullableDateTime_WithDefault_EmptyInput_ReturnsDefault()
        {
            // Positive: when a default is supplied, an empty response re-parses the
            // default (GetString returns the default answer for empty input).
            DateTime dflt = new DateTime(2020, 1, 1);
            DateTime? result = ConsoleHarness.Run(
                () => Sut.GetNullableDateTime("When?", dflt), "");
            Assert.Equal(dflt, result);
        }

        [Fact]
        public void GetNullableDateTime_WithNullDefault_EmptyInput_ReturnsNull()
        {
            // Positive: with a null default and empty input, the result is null.
            DateTime? result = ConsoleHarness.Run(
                () => Sut.GetNullableDateTime("When?", null), "");
            Assert.Null(result);
        }

        #endregion

        #region GetGuid

        [Fact]
        public void GetGuid_ValidInput_Parses()
        {
            // Positive.
            Guid expected = Guid.Parse("11111111-1111-1111-1111-111111111111");
            Guid result = ConsoleHarness.Run(() => Sut.GetGuid("Id?"), expected.ToString());
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetGuid_EmptyInput_ReturnsDefault()
        {
            // Positive: empty input parses the supplied default GUID.
            Guid dflt = Guid.Parse("22222222-2222-2222-2222-222222222222");
            Guid result = ConsoleHarness.Run(() => Sut.GetGuid("Id?", dflt), "");
            Assert.Equal(dflt, result);
        }

        [Fact]
        public void GetGuid_InvalidInput_ThrowsFormatException()
        {
            // Negative.
            Assert.Throws<FormatException>(
                () => ConsoleHarness.Run(() => Sut.GetGuid("Id?"), "not-a-guid"));
        }

        #endregion

        #region DateTimeFormats property

        [Fact]
        public void DateTimeFormats_HasDefaults()
        {
            // Positive.
            Assert.NotNull(Sut.DateTimeFormats);
            Assert.NotEmpty(Sut.DateTimeFormats);
        }

        [Fact]
        public void DateTimeFormats_SetNull_ThrowsArgumentNullException()
        {
            // Negative.
            List<string> original = Sut.DateTimeFormats;
            try
            {
                Assert.Throws<ArgumentNullException>(() => Sut.DateTimeFormats = null);
            }
            finally
            {
                Sut.DateTimeFormats = original;
            }
        }

        [Fact]
        public void DateTimeFormats_SetEmpty_ThrowsArgumentException()
        {
            // Negative.
            List<string> original = Sut.DateTimeFormats;
            try
            {
                Assert.Throws<ArgumentException>(() => Sut.DateTimeFormats = new List<string>());
            }
            finally
            {
                Sut.DateTimeFormats = original;
            }
        }

        [Fact]
        public void DateTimeFormats_SetValid_Roundtrips()
        {
            // Positive.
            List<string> original = Sut.DateTimeFormats;
            try
            {
                List<string> custom = new List<string> { "yyyy" };
                Sut.DateTimeFormats = custom;
                Assert.Same(custom, Sut.DateTimeFormats);
            }
            finally
            {
                Sut.DateTimeFormats = original;
            }
        }

        #endregion
    }
}
