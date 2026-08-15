namespace Test.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Threading;
    using System.Threading.Tasks;
    using Touchstone.Core;
    using Sut = GetSomeInput.Inputty;

    /// <summary>
    /// Central source of truth for all Inputty tests.
    ///
    /// Tests are defined once as runner-agnostic Touchstone descriptors and executed
    /// through any host: the console runner (Test.Automated), xUnit (Test.Xunit), or
    /// NUnit (Test.Nunit). Console input is supplied via <see cref="ConsoleHarness"/>,
    /// which redirects Console.In/Console.Out so the console-driven methods can be
    /// exercised deterministically.
    ///
    /// Each case is annotated in its display name as a positive (happy-path) or a
    /// negative (invalid input / retry / exception) scenario. Every public member of
    /// the Inputty surface is covered.
    /// </summary>
    public static class InputtySuites
    {
        /// <summary>
        /// All Inputty test suites, in a stable order suitable for sequential
        /// execution (the console-redirection harness and the mutable
        /// DateTimeFormats property require tests to run one at a time).
        /// </summary>
        public static IReadOnlyList<TestSuiteDescriptor> All
        {
            get
            {
                return new List<TestSuiteDescriptor>
                {
                    GetBooleanSuite(),
                    GetStringSuite(),
                    GetIntegerSuite(),
                    GetDecimalSuite(),
                    GetDoubleSuite(),
                    GetStringListSuite(),
                    GetDictionarySuite(),
                    GetNameValueCollectionSuite(),
                    GetDateTimeSuite(),
                    GetNullableDateTimeSuite(),
                    GetGuidSuite(),
                    DateTimeFormatsSuite()
                };
            }
        }

        #region GetBoolean

        private static TestSuiteDescriptor GetBooleanSuite()
        {
            return new TestSuiteDescriptor(
                "GetBoolean",
                "GetBoolean(question, trueDefault)",
                new List<TestCaseDescriptor>
                {
                    Case("GetBoolean", "TrueDefault_Empty", "[+] true default, empty input returns true", () =>
                        Check.True(ConsoleHarness.Run(() => Sut.GetBoolean("Proceed?", true), ""))),

                    Case("GetBoolean", "FalseDefault_Empty", "[+] false default, empty input returns false", () =>
                        Check.False(ConsoleHarness.Run(() => Sut.GetBoolean("Proceed?", false), ""))),

                    Case("GetBoolean", "FalseDefault_LowerY", "[+] false default, 'y' returns true", () =>
                        Check.True(ConsoleHarness.Run(() => Sut.GetBoolean("Proceed?", false), "y"))),

                    Case("GetBoolean", "FalseDefault_Yes", "[+] false default, 'yes' returns true", () =>
                        Check.True(ConsoleHarness.Run(() => Sut.GetBoolean("Proceed?", false), "yes"))),

                    Case("GetBoolean", "FalseDefault_One", "[+] false default, '1' returns true", () =>
                        Check.True(ConsoleHarness.Run(() => Sut.GetBoolean("Proceed?", false), "1"))),

                    Case("GetBoolean", "FalseDefault_UpperY", "[+] false default, 'Y' is case-insensitive true", () =>
                        Check.True(ConsoleHarness.Run(() => Sut.GetBoolean("Proceed?", false), "Y"))),

                    Case("GetBoolean", "FalseDefault_UpperYes", "[+] false default, 'YES' is case-insensitive true", () =>
                        Check.True(ConsoleHarness.Run(() => Sut.GetBoolean("Proceed?", false), "YES"))),

                    Case("GetBoolean", "TrueDefault_LowerN", "[+] true default, 'n' returns false", () =>
                        Check.False(ConsoleHarness.Run(() => Sut.GetBoolean("Proceed?", true), "n"))),

                    Case("GetBoolean", "TrueDefault_No", "[+] true default, 'no' returns false", () =>
                        Check.False(ConsoleHarness.Run(() => Sut.GetBoolean("Proceed?", true), "no"))),

                    Case("GetBoolean", "TrueDefault_Zero", "[+] true default, '0' returns false", () =>
                        Check.False(ConsoleHarness.Run(() => Sut.GetBoolean("Proceed?", true), "0"))),

                    Case("GetBoolean", "TrueDefault_UpperN", "[+] true default, 'N' is case-insensitive false", () =>
                        Check.False(ConsoleHarness.Run(() => Sut.GetBoolean("Proceed?", true), "N"))),

                    Case("GetBoolean", "TrueDefault_Affirmative", "[+] true default, 'y' remains true", () =>
                        Check.True(ConsoleHarness.Run(() => Sut.GetBoolean("Proceed?", true), "y"))),

                    Case("GetBoolean", "FalseDefault_Negative", "[+] false default, 'n' remains false", () =>
                        Check.False(ConsoleHarness.Run(() => Sut.GetBoolean("Proceed?", false), "n"))),

                    Case("GetBoolean", "TrueDefault_Unrecognized", "[-] true default, unrecognized token biases to true", () =>
                        Check.True(ConsoleHarness.Run(() => Sut.GetBoolean("Proceed?", true), "maybe"))),

                    Case("GetBoolean", "FalseDefault_Unrecognized", "[-] false default, unrecognized token biases to false", () =>
                        Check.False(ConsoleHarness.Run(() => Sut.GetBoolean("Proceed?", false), "maybe"))),

                    Case("GetBoolean", "FalseDefault_PaddedY_NotTrimmed", "[-] false default, ' y ' is not trimmed so returns false", () =>
                        Check.False(ConsoleHarness.Run(() => Sut.GetBoolean("Proceed?", false), " y ")))
                });
        }

        #endregion

        #region GetString

        private static TestSuiteDescriptor GetStringSuite()
        {
            return new TestSuiteDescriptor(
                "GetString",
                "GetString(question, defaultAnswer, allowNull)",
                new List<TestCaseDescriptor>
                {
                    Case("GetString", "NonEmpty_ReturnsInput", "[+] non-empty input is returned verbatim", () =>
                        Check.Equal("Joel", ConsoleHarness.Run(() => Sut.GetString("Name?", null, false), "Joel"))),

                    Case("GetString", "Empty_WithDefault_ReturnsDefault", "[+] empty input returns the default answer", () =>
                        Check.Equal("Anonymous", ConsoleHarness.Run(() => Sut.GetString("Name?", "Anonymous", false), ""))),

                    Case("GetString", "Empty_AllowNull_NoDefault_ReturnsNull", "[+] empty input returns null when null is allowed", () =>
                        Check.Null(ConsoleHarness.Run(() => Sut.GetString("Name?", null, true), ""))),

                    Case("GetString", "NonEmpty_WithDefault_ReturnsInput", "[+] non-empty input overrides the default", () =>
                        Check.Equal("Joel", ConsoleHarness.Run(() => Sut.GetString("Name?", "Anonymous", false), "Joel"))),

                    Case("GetString", "DefaultBeatsAllowNull", "[+] a supplied default takes precedence over allowNull on empty input", () =>
                        Check.Equal("d", ConsoleHarness.Run(() => Sut.GetString("Name?", "d", true), ""))),

                    Case("GetString", "Whitespace_ReturnsWhitespace", "[+] whitespace is non-empty and returned as-is", () =>
                        Check.Equal(" ", ConsoleHarness.Run(() => Sut.GetString("Name?", null, false), " "))),

                    Case("GetString", "EmptyThenValue_NoNull_Retries", "[-] empty input is rejected (no default, null disallowed) until non-empty supplied", () =>
                        Check.Equal("Joel", ConsoleHarness.Run(() => Sut.GetString("Name?", null, false), "", "", "Joel")))
                });
        }

        #endregion

        #region GetInteger

        private static TestSuiteDescriptor GetIntegerSuite()
        {
            return new TestSuiteDescriptor(
                "GetInteger",
                "GetInteger(question, defaultAnswer, positiveOnly, allowZero)",
                new List<TestCaseDescriptor>
                {
                    Case("GetInteger", "Valid", "[+] valid integer is returned", () =>
                        Check.Equal(7, ConsoleHarness.Run(() => Sut.GetInteger("Age?", 42, false, true), "7"))),

                    Case("GetInteger", "Empty_ReturnsDefault", "[+] empty input returns the default", () =>
                        Check.Equal(42, ConsoleHarness.Run(() => Sut.GetInteger("Age?", 42, false, true), ""))),

                    Case("GetInteger", "Negative_Allowed", "[+] negative value accepted when positiveOnly is false", () =>
                        Check.Equal(-5, ConsoleHarness.Run(() => Sut.GetInteger("Value?", 1, false, true), "-5"))),

                    Case("GetInteger", "Positive_PositiveOnly", "[+] positive value accepted when positiveOnly is true", () =>
                        Check.Equal(9, ConsoleHarness.Run(() => Sut.GetInteger("Value?", 1, true, false), "9"))),

                    Case("GetInteger", "Zero_AllowZero", "[+] zero accepted when allowZero is true", () =>
                        Check.Equal(0, ConsoleHarness.Run(() => Sut.GetInteger("Value?", 1, false, true), "0"))),

                    Case("GetInteger", "Padded_Parses", "[+] surrounding whitespace is tolerated by integer parsing", () =>
                        Check.Equal(7, ConsoleHarness.Run(() => Sut.GetInteger("Age?", 42, false, true), "  7  "))),

                    Case("GetInteger", "MaxValue", "[+] Int32.MaxValue parses", () =>
                        Check.Equal(int.MaxValue, ConsoleHarness.Run(() => Sut.GetInteger("Value?", 0, false, true), int.MaxValue.ToString()))),

                    Case("GetInteger", "MinValue_Allowed", "[+] Int32.MinValue parses when positiveOnly is false", () =>
                        Check.Equal(int.MinValue, ConsoleHarness.Run(() => Sut.GetInteger("Value?", 0, false, true), int.MinValue.ToString()))),

                    Case("GetInteger", "Zero_NotAllowZero_StillReturnsZero", "[-] zero is returned even when allowZero is false (documents actual behavior)", () =>
                        Check.Equal(0, ConsoleHarness.Run(() => Sut.GetInteger("Value?", 1, false, false), "0"))),

                    Case("GetInteger", "NonNumericThenValid_Retries", "[-] non-numeric input is rejected and re-prompted", () =>
                        Check.Equal(13, ConsoleHarness.Run(() => Sut.GetInteger("Age?", 42, false, true), "abc", "13"))),

                    Case("GetInteger", "NegativePositiveOnly_Retries", "[-] negative value rejected when positiveOnly, then accepts valid", () =>
                        Check.Equal(9, ConsoleHarness.Run(() => Sut.GetInteger("Age?", 1, true, false), "-5", "9"))),

                    Case("GetInteger", "OverflowThenValid_Retries", "[-] value too large for Int32 is rejected, then accepts valid", () =>
                        Check.Equal(5, ConsoleHarness.Run(() => Sut.GetInteger("Value?", 0, false, true), "99999999999999", "5")))
                });
        }

        #endregion

        #region GetDecimal

        private static TestSuiteDescriptor GetDecimalSuite()
        {
            return new TestSuiteDescriptor(
                "GetDecimal",
                "GetDecimal(question, defaultAnswer, positiveOnly, allowZero)",
                new List<TestCaseDescriptor>
                {
                    Case("GetDecimal", "Valid", "[+] valid decimal is returned", () =>
                        Check.Equal(3.25m, ConsoleHarness.Run(() => Sut.GetDecimal("Amount?", 1.5m, false, true), "3.25"))),

                    Case("GetDecimal", "Empty_ReturnsDefault", "[+] empty input returns the default", () =>
                        Check.Equal(1.5m, ConsoleHarness.Run(() => Sut.GetDecimal("Amount?", 1.5m, false, true), ""))),

                    Case("GetDecimal", "IntegerInput", "[+] integer-formatted input parses as decimal", () =>
                        Check.Equal(5m, ConsoleHarness.Run(() => Sut.GetDecimal("Amount?", 1.5m, false, true), "5"))),

                    Case("GetDecimal", "Negative_Allowed", "[+] negative value accepted when positiveOnly is false", () =>
                        Check.Equal(-2.5m, ConsoleHarness.Run(() => Sut.GetDecimal("Amount?", 1m, false, true), "-2.5"))),

                    Case("GetDecimal", "Zero_AllowZero", "[+] zero accepted when allowZero is true", () =>
                        Check.Equal(0m, ConsoleHarness.Run(() => Sut.GetDecimal("Amount?", 1m, false, true), "0"))),

                    Case("GetDecimal", "NonNumericThenValid_Retries", "[-] non-numeric input is rejected and re-prompted", () =>
                        Check.Equal(2.75m, ConsoleHarness.Run(() => Sut.GetDecimal("Amount?", 1.5m, false, true), "xyz", "2.75"))),

                    Case("GetDecimal", "NegativePositiveOnly_Retries", "[-] negative rejected when positiveOnly, then accepts valid", () =>
                        Check.Equal(4.0m, ConsoleHarness.Run(() => Sut.GetDecimal("Amount?", 1m, true, false), "-2.5", "4.0")))
                });
        }

        #endregion

        #region GetDouble

        private static TestSuiteDescriptor GetDoubleSuite()
        {
            return new TestSuiteDescriptor(
                "GetDouble",
                "GetDouble(question, defaultAnswer, positiveOnly, allowZero)",
                new List<TestCaseDescriptor>
                {
                    Case("GetDouble", "Valid", "[+] valid double is returned", () =>
                        Check.Equal(3.5, ConsoleHarness.Run(() => Sut.GetDouble("Amount?", 1.5, false, true), "3.5"))),

                    Case("GetDouble", "Empty_ReturnsDefault", "[+] empty input returns the default", () =>
                        Check.Equal(9.9, ConsoleHarness.Run(() => Sut.GetDouble("Amount?", 9.9, false, true), ""))),

                    Case("GetDouble", "Negative_Allowed", "[+] negative value accepted when positiveOnly is false", () =>
                        Check.Equal(-1.5, ConsoleHarness.Run(() => Sut.GetDouble("Amount?", 1.0, false, true), "-1.5"))),

                    Case("GetDouble", "Zero_AllowZero", "[+] zero accepted when allowZero is true", () =>
                        Check.Equal(0.0, ConsoleHarness.Run(() => Sut.GetDouble("Amount?", 1.0, false, true), "0"))),

                    Case("GetDouble", "NonNumericThenValid_Retries", "[-] non-numeric input is rejected and re-prompted", () =>
                        Check.Equal(4.2, ConsoleHarness.Run(() => Sut.GetDouble("Amount?", 1.0, false, true), "notanumber", "4.2"))),

                    Case("GetDouble", "NegativePositiveOnly_Retries", "[-] negative rejected when positiveOnly, then accepts valid", () =>
                        Check.Equal(4.2, ConsoleHarness.Run(() => Sut.GetDouble("Amount?", 1.0, true, false), "-1.5", "4.2")))
                });
        }

        #endregion

        #region GetStringList

        private static TestSuiteDescriptor GetStringListSuite()
        {
            return new TestSuiteDescriptor(
                "GetStringList",
                "GetStringList(question, allowEmpty)",
                new List<TestCaseDescriptor>
                {
                    Case("GetStringList", "CollectsUntilEmpty", "[+] collects lines until an empty line terminates input", () =>
                        Check.Sequence(new List<string> { "a", "b", "c" },
                            ConsoleHarness.Run(() => Sut.GetStringList("Item?", true), "a", "b", "c", ""))),

                    Case("GetStringList", "SingleThenEmpty", "[+] a single value followed by empty returns one element", () =>
                        Check.Sequence(new List<string> { "x" },
                            ConsoleHarness.Run(() => Sut.GetStringList("Item?", true), "x", ""))),

                    Case("GetStringList", "Duplicates_Preserved", "[+] duplicate values are preserved in order", () =>
                        Check.Sequence(new List<string> { "a", "a" },
                            ConsoleHarness.Run(() => Sut.GetStringList("Item?", true), "a", "a", ""))),

                    Case("GetStringList", "EmptyImmediately_AllowEmpty", "[+] empty first line returns an empty list when allowEmpty is true", () =>
                        Check.Empty(ConsoleHarness.Run(() => Sut.GetStringList("Item?", true), ""))),

                    Case("GetStringList", "EmptyFirst_DisallowEmpty_RequiresOne", "[-] empty first line rejected when allowEmpty is false until one value supplied", () =>
                        Check.Sequence(new List<string> { "only" },
                            ConsoleHarness.Run(() => Sut.GetStringList("Item?", false), "", "only", "")))
                });
        }

        #endregion

        #region GetDictionary

        private static TestSuiteDescriptor GetDictionarySuite()
        {
            return new TestSuiteDescriptor(
                "GetDictionary",
                "GetDictionary<TKey, TValue>(keyQuestion, valQuestion)",
                new List<TestCaseDescriptor>
                {
                    Case("GetDictionary", "BuildsFromPairs", "[+] builds a dictionary from key/value pairs until an empty key", () =>
                    {
                        Dictionary<string, string> d = ConsoleHarness.Run(
                            () => Sut.GetDictionary<string, string>("Key:", "Value:"),
                            "k1", "v1", "k2", "v2", "");
                        Check.Equal(2, d.Count);
                        Check.Equal("v1", d["k1"]);
                        Check.Equal("v2", d["k2"]);
                    }),

                    Case("GetDictionary", "TypedValue_ConvertsInt", "[+] values are converted to the requested value type", () =>
                    {
                        Dictionary<string, int> d = ConsoleHarness.Run(
                            () => Sut.GetDictionary<string, int>("Key:", "Value:"),
                            "one", "1", "");
                        Check.Equal(1, d["one"]);
                    }),

                    Case("GetDictionary", "TypedKey_ConvertsInt", "[+] keys are converted to the requested key type", () =>
                    {
                        Dictionary<int, string> d = ConsoleHarness.Run(
                            () => Sut.GetDictionary<int, string>("Key:", "Value:"),
                            "1", "one", "");
                        Check.Equal("one", d[1]);
                    }),

                    Case("GetDictionary", "EmptyValue_YieldsDefault", "[+] an empty value stores the type default (null for reference types)", () =>
                    {
                        Dictionary<string, string> d = ConsoleHarness.Run(
                            () => Sut.GetDictionary<string, string>("Key:", "Value:"),
                            "k1", "", "");
                        Check.Equal(1, d.Count);
                        Check.Null(d["k1"]);
                    }),

                    Case("GetDictionary", "EmptyKeyImmediately_ReturnsEmpty", "[-] an empty first key terminates collection with an empty dictionary", () =>
                    {
                        Dictionary<string, string> d = ConsoleHarness.Run(
                            () => Sut.GetDictionary<string, string>("Key:", "Value:"),
                            "");
                        Check.Empty(d);
                    })
                });
        }

        #endregion

        #region GetNameValueCollection

        private static TestSuiteDescriptor GetNameValueCollectionSuite()
        {
            return new TestSuiteDescriptor(
                "GetNameValueCollection",
                "GetNameValueCollection(keyQuestion, valQuestion, caseInsensitive)",
                new List<TestCaseDescriptor>
                {
                    Case("GetNameValueCollection", "CaseInsensitive_Lookup", "[+] case-insensitive lookups succeed regardless of key casing", () =>
                    {
                        NameValueCollection nvc = ConsoleHarness.Run(
                            () => Sut.GetNameValueCollection("Key:", "Value:", true),
                            "Alpha", "1", "Beta", "2", "");
                        Check.Equal("1", nvc["alpha"]);
                        Check.Equal("2", nvc["BETA"]);
                    }),

                    Case("GetNameValueCollection", "CaseInsensitiveFalse_StillInsensitive", "[-] caseInsensitive:false still yields case-insensitive lookups (default NameValueCollection ctor is case-insensitive)", () =>
                    {
                        NameValueCollection nvc = ConsoleHarness.Run(
                            () => Sut.GetNameValueCollection("Key:", "Value:", false),
                            "Alpha", "1", "");
                        Check.Equal("1", nvc["Alpha"]);
                        Check.Equal("1", nvc["alpha"]);
                    }),

                    Case("GetNameValueCollection", "DuplicateKeys_Combine", "[+] repeated keys combine into a comma-separated value", () =>
                    {
                        NameValueCollection nvc = ConsoleHarness.Run(
                            () => Sut.GetNameValueCollection("Key:", "Value:", true),
                            "k", "1", "k", "2", "");
                        Check.Equal("1,2", nvc["k"]);
                    }),

                    Case("GetNameValueCollection", "EmptyKeyImmediately_ReturnsEmpty", "[-] an empty first key terminates collection with an empty collection", () =>
                    {
                        NameValueCollection nvc = ConsoleHarness.Run(
                            () => Sut.GetNameValueCollection("Key:", "Value:", true),
                            "");
                        Check.Equal(0, nvc.Count);
                    })
                });
        }

        #endregion

        #region GetDateTime

        private static TestSuiteDescriptor GetDateTimeSuite()
        {
            return new TestSuiteDescriptor(
                "GetDateTime",
                "GetDateTime(question) and GetDateTime(question, defaultAnswer)",
                new List<TestCaseDescriptor>
                {
                    Case("GetDateTime", "ValidIso_Parses", "[+] an ISO-style timestamp parses", () =>
                        Check.Equal(new DateTime(2024, 1, 15, 13, 30, 0),
                            ConsoleHarness.Run(() => Sut.GetDateTime("When?"), "2024-01-15 13:30:00"))),

                    Case("GetDateTime", "CustomFormat_Parses", "[+] a value matching a configured format parses", () =>
                        Check.Equal(new DateTime(2024, 6, 15, 9, 45, 0),
                            ConsoleHarness.Run(() => Sut.GetDateTime("When?"), "06/15/2024 09:45"))),

                    Case("GetDateTime", "DateOnly_Parses", "[+] a date-only value parses to midnight", () =>
                        Check.Equal(new DateTime(2024, 1, 15),
                            ConsoleHarness.Run(() => Sut.GetDateTime("When?"), "2024-01-15"))),

                    Case("GetDateTime", "WithDefault_Empty_ReturnsDefault", "[+] empty input re-parses the supplied default", () =>
                    {
                        DateTime dflt = new DateTime(2020, 6, 1, 12, 0, 0);
                        Check.Equal(dflt, ConsoleHarness.Run(() => Sut.GetDateTime("When?", dflt), ""));
                    }),

                    Case("GetDateTime", "WithDefault_ValidInput_ReturnsInput", "[+] a valid value overrides the default", () =>
                        Check.Equal(new DateTime(2024, 3, 10, 8, 0, 0),
                            ConsoleHarness.Run(() => Sut.GetDateTime("When?", new DateTime(2020, 1, 1)), "2024-03-10 08:00:00"))),

                    Case("GetDateTime", "Unparseable_Throws", "[-] a non-empty, unparseable value throws FormatException", () =>
                        Check.Throws<FormatException>(
                            () => ConsoleHarness.Run(() => Sut.GetDateTime("When?"), "definitely-not-a-date"))),

                    Case("GetDateTime", "WithDefault_Unparseable_Throws", "[-] an unparseable value throws even when a default is supplied", () =>
                        Check.Throws<FormatException>(
                            () => ConsoleHarness.Run(() => Sut.GetDateTime("When?", new DateTime(2020, 1, 1)), "nope")))
                });
        }

        #endregion

        #region GetNullableDateTime

        private static TestSuiteDescriptor GetNullableDateTimeSuite()
        {
            return new TestSuiteDescriptor(
                "GetNullableDateTime",
                "GetNullableDateTime(question) and GetNullableDateTime(question, defaultAnswer)",
                new List<TestCaseDescriptor>
                {
                    Case("GetNullableDateTime", "Empty_ReturnsNull", "[+] empty input returns null", () =>
                        Check.Null(ConsoleHarness.Run(() => Sut.GetNullableDateTime("When?"), ""))),

                    Case("GetNullableDateTime", "Valid_ReturnsValue", "[+] a valid value parses", () =>
                        Check.Equal(new DateTime(2024, 1, 15),
                            ConsoleHarness.Run(() => Sut.GetNullableDateTime("When?"), "2024-01-15 00:00:00"))),

                    Case("GetNullableDateTime", "DateOnly_Parses", "[+] a date-only value parses to midnight", () =>
                        Check.Equal(new DateTime(2024, 7, 4),
                            ConsoleHarness.Run(() => Sut.GetNullableDateTime("When?"), "2024-07-04"))),

                    Case("GetNullableDateTime", "WithDefault_Empty_ReturnsDefault", "[+] empty input re-parses the supplied default", () =>
                    {
                        DateTime dflt = new DateTime(2020, 1, 1);
                        Check.Equal(dflt, ConsoleHarness.Run(() => Sut.GetNullableDateTime("When?", dflt), ""));
                    }),

                    Case("GetNullableDateTime", "WithNullDefault_Empty_ReturnsNull", "[+] empty input with a null default returns null", () =>
                        Check.Null(ConsoleHarness.Run(() => Sut.GetNullableDateTime("When?", null), ""))),

                    Case("GetNullableDateTime", "WithNullDefault_Valid_ReturnsValue", "[+] a valid value parses even when the default is null", () =>
                        Check.Equal(new DateTime(2024, 5, 5, 5, 5, 5),
                            ConsoleHarness.Run(() => Sut.GetNullableDateTime("When?", null), "2024-05-05 05:05:05"))),

                    Case("GetNullableDateTime", "Unparseable_Throws", "[-] a non-empty, unparseable value throws FormatException", () =>
                        Check.Throws<FormatException>(
                            () => ConsoleHarness.Run(() => Sut.GetNullableDateTime("When?"), "not-a-date"))),

                    Case("GetNullableDateTime", "WithNullDefault_Unparseable_Throws", "[-] an unparseable value throws even with a null default", () =>
                        Check.Throws<FormatException>(
                            () => ConsoleHarness.Run(() => Sut.GetNullableDateTime("When?", null), "xx")))
                });
        }

        #endregion

        #region GetGuid

        private static TestSuiteDescriptor GetGuidSuite()
        {
            return new TestSuiteDescriptor(
                "GetGuid",
                "GetGuid(question, defaultAnswer)",
                new List<TestCaseDescriptor>
                {
                    Case("GetGuid", "Valid_Parses", "[+] a valid GUID string parses", () =>
                    {
                        Guid expected = Guid.Parse("11111111-1111-1111-1111-111111111111");
                        Check.Equal(expected, ConsoleHarness.Run(() => Sut.GetGuid("Id?"), expected.ToString()));
                    }),

                    Case("GetGuid", "Braces_Parses", "[+] a brace-delimited GUID string parses", () =>
                    {
                        Guid expected = Guid.Parse("22222222-2222-2222-2222-222222222222");
                        Check.Equal(expected, ConsoleHarness.Run(() => Sut.GetGuid("Id?"), "{22222222-2222-2222-2222-222222222222}"));
                    }),

                    Case("GetGuid", "Empty_NoDefault_ReturnsEmptyGuid", "[+] empty input with no explicit default parses to Guid.Empty", () =>
                        Check.Equal(Guid.Empty, ConsoleHarness.Run(() => Sut.GetGuid("Id?"), ""))),

                    Case("GetGuid", "Empty_WithDefault_ReturnsDefault", "[+] empty input parses the supplied default GUID", () =>
                    {
                        Guid dflt = Guid.Parse("33333333-3333-3333-3333-333333333333");
                        Check.Equal(dflt, ConsoleHarness.Run(() => Sut.GetGuid("Id?", dflt), ""));
                    }),

                    Case("GetGuid", "WithDefault_ValidInput_Overrides", "[+] a valid value overrides the default GUID", () =>
                    {
                        Guid dflt = Guid.Parse("33333333-3333-3333-3333-333333333333");
                        Guid entered = Guid.Parse("44444444-4444-4444-4444-444444444444");
                        Check.Equal(entered, ConsoleHarness.Run(() => Sut.GetGuid("Id?", dflt), entered.ToString()));
                    }),

                    Case("GetGuid", "Invalid_Throws", "[-] an invalid GUID string throws FormatException", () =>
                        Check.Throws<FormatException>(
                            () => ConsoleHarness.Run(() => Sut.GetGuid("Id?"), "not-a-guid")))
                });
        }

        #endregion

        #region DateTimeFormats

        private static TestSuiteDescriptor DateTimeFormatsSuite()
        {
            return new TestSuiteDescriptor(
                "DateTimeFormats",
                "DateTimeFormats property",
                new List<TestCaseDescriptor>
                {
                    Case("DateTimeFormats", "HasDefaults", "[+] the default format list is populated", () =>
                    {
                        Check.NotNull(Sut.DateTimeFormats);
                        Check.NotEmpty(Sut.DateTimeFormats);
                    }),

                    Case("DateTimeFormats", "SetValid_Roundtrips", "[+] a valid list is stored and returned by reference", () =>
                    {
                        List<string> original = Sut.DateTimeFormats;
                        try
                        {
                            List<string> custom = new List<string> { "yyyy" };
                            Sut.DateTimeFormats = custom;
                            Check.Same(custom, Sut.DateTimeFormats);
                        }
                        finally
                        {
                            Sut.DateTimeFormats = original;
                        }
                    }),

                    Case("DateTimeFormats", "SetNull_Throws", "[-] assigning null throws ArgumentNullException", () =>
                    {
                        List<string> original = Sut.DateTimeFormats;
                        try
                        {
                            Check.Throws<ArgumentNullException>(() => Sut.DateTimeFormats = null);
                        }
                        finally
                        {
                            Sut.DateTimeFormats = original;
                        }
                    }),

                    Case("DateTimeFormats", "SetEmpty_Throws", "[-] assigning an empty list throws ArgumentException", () =>
                    {
                        List<string> original = Sut.DateTimeFormats;
                        try
                        {
                            Check.Throws<ArgumentException>(() => Sut.DateTimeFormats = new List<string>());
                        }
                        finally
                        {
                            Sut.DateTimeFormats = original;
                        }
                    })
                });
        }

        #endregion

        #region Helpers

        private static TestCaseDescriptor Case(string suiteId, string caseId, string displayName, Action body)
        {
            return new TestCaseDescriptor(
                suiteId,
                caseId,
                displayName,
                delegate (CancellationToken ct)
                {
                    body();
                    return Task.CompletedTask;
                });
        }

        #endregion
    }
}
