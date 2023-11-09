namespace GetSomeInput
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Runtime.Serialization;

    /// <summary>
    /// Inputty is a helpful library for getting values from the console.
    /// </summary>
    public static class Inputty
    {
        #region Public-Members

        /// <summary>
        /// DateTime formats to evaluate when receiving DateTime input.
        /// </summary>
        public static List<string> DateTimeFormats
        {
            get
            {
                return _DateTimeFormats;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(DateTimeFormats));
                if (value.Count < 1) throw new ArgumentException("At least one DateTime format must be supplied.");
                _DateTimeFormats = value;
            }
        }

        #endregion

        #region Private-Members

        private static List<string> _DateTimeFormats = new List<string>
        {
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-ddTHH:mm:ss",
            "yyyy-MM-ddTHH:mm:ssK",
            "yyyy-MM-dd HH:mm:ss.ffffff",
            "yyyy-MM-ddTHH:mm:ss.ffffff",
            "yyyy-MM-ddTHH:mm:ss.fffffffK",
            "yyyy-MM-dd",
            "MM/dd/yyyy HH:mm",
            "MM/dd/yyyy hh:mm tt",
            "MM/dd/yyyy H:mm",
            "MM/dd/yyyy h:mm tt",
            "MM/dd/yyyy HH:mm:ss",
            "yyyyMMddTHHmmssZ"
        };

        #endregion

        #region Public-Methods

        /// <summary>
        /// Retrieve a Boolean value from the console.
        /// </summary>
        /// <param name="question">Question/prompt.</param>
        /// <param name="trueDefault">Flag to indicate if the default response is true.</param>
        /// <returns>Boolean.</returns>
        public static bool GetBoolean(string question, bool trueDefault)
        {
            Console.Write(question);

            if (trueDefault) Console.Write(" [Y/n]? ");
            else Console.Write(" [y/N]? ");

            string userInput = Console.ReadLine();

            if (String.IsNullOrEmpty(userInput))
            {
                if (trueDefault) return true;
                return false;
            }

            userInput = userInput.ToLower();

            if (trueDefault)
            {
                if (
                    (String.Compare(userInput, "n") == 0)
                    || (String.Compare(userInput, "no") == 0)
                    || (String.Compare(userInput, "0") == 0)
                   )
                {
                    return false;
                }

                return true;
            }
            else
            {
                if (
                    (String.Compare(userInput, "y") == 0)
                    || (String.Compare(userInput, "yes") == 0)
                    || (String.Compare(userInput, "1") == 0)
                   )
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Retrieve a string value from the console.
        /// </summary>
        /// <param name="question">Question/prompt.</param>
        /// <param name="defaultAnswer">The default answer to the question/prompt.</param>
        /// <param name="allowNull">Flag to indicate if null values are allowed.</param>
        /// <returns>String.</returns>
        public static string GetString(string question, string defaultAnswer, bool allowNull)
        {
            while (true)
            {
                Console.Write(question);

                if (!String.IsNullOrEmpty(defaultAnswer))
                {
                    Console.Write(" [" + defaultAnswer + "]");
                }

                Console.Write(" ");

                string userInput = Console.ReadLine();

                if (String.IsNullOrEmpty(userInput))
                {
                    if (!String.IsNullOrEmpty(defaultAnswer)) return defaultAnswer;
                    if (allowNull) return null;
                    else continue;
                }

                return userInput;
            }
        }

        /// <summary>
        /// Retrieve an integer value from the console.
        /// </summary>
        /// <param name="question">Question/prompt.</param>
        /// <param name="defaultAnswer">The default answer to the question/prompt.</param>
        /// <param name="positiveOnly">Flag to indicate if only positive values are allowed.</param>
        /// <param name="allowZero">Flag to indicate if zero values are allowed.</param>
        /// <returns>Integer.</returns>
        public static int GetInteger(string question, int defaultAnswer, bool positiveOnly, bool allowZero)
        {
            while (true)
            {
                Console.Write(question);
                Console.Write(" [" + defaultAnswer + "] ");

                string userInput = Console.ReadLine();

                if (String.IsNullOrEmpty(userInput))
                {
                    return defaultAnswer;
                }

                int ret = 0;
                if (!Int32.TryParse(userInput, out ret))
                {
                    Console.WriteLine("Please enter a valid integer.");
                    continue;
                }

                if (ret == 0)
                {
                    if (allowZero)
                    {
                        return 0;
                    }
                }

                if (ret < 0)
                {
                    if (positiveOnly)
                    {
                        Console.WriteLine("Please enter a value greater than zero.");
                        continue;
                    }
                }

                return ret;
            }
        }

        /// <summary>
        /// Retrieve a string list value from the console.
        /// </summary>
        /// <param name="question">Question/prompt.</param>
        /// <param name="allowEmpty">Flag to indicate if null or empty values should be allowed.</param>
        /// <returns>List of String.</returns>
        public static List<string> GetStringList(string question, bool allowEmpty)
        {
            List<string> ret = new List<string>();

            while (true)
            {
                Console.Write(question);

                Console.Write(" ");

                string userInput = Console.ReadLine();

                if (String.IsNullOrEmpty(userInput))
                {
                    if (ret.Count < 1 && !allowEmpty) continue;
                    return ret;
                }

                ret.Add(userInput);
            }
        }

        /// <summary>
        /// Retrieve a decimal value from the console.
        /// </summary>
        /// <param name="question">Question/prompt.</param>
        /// <param name="defaultAnswer">The default answer to the question/prompt.</param>
        /// <param name="positiveOnly">Flag to indicate if only positive values are allowed.</param>
        /// <param name="allowZero">Flag to indicate if zero values are allowed.</param>
        /// <returns>Decimal.</returns>
        public static decimal GetDecimal(string question, decimal defaultAnswer, bool positiveOnly, bool allowZero)
        {
            while (true)
            {
                Console.Write(question);
                Console.Write(" [" + defaultAnswer + "] ");

                string userInput = Console.ReadLine();

                if (String.IsNullOrEmpty(userInput))
                {
                    return defaultAnswer;
                }

                decimal ret = 0;
                if (!Decimal.TryParse(userInput, out ret))
                {
                    Console.WriteLine("Please enter a valid decimal.");
                    continue;
                }

                if (ret == 0)
                {
                    if (allowZero)
                    {
                        return 0;
                    }
                }

                if (ret < 0)
                {
                    if (positiveOnly)
                    {
                        Console.WriteLine("Please enter a value greater than zero.");
                        continue;
                    }
                }

                return ret;
            }
        }

        /// <summary>
        /// Retrieve a double value from the console.
        /// </summary>
        /// <param name="question">Question/prompt.</param>
        /// <param name="defaultAnswer">The default answer to the question/prompt.</param>
        /// <param name="positiveOnly">Flag to indicate if only positive values are allowed.</param>
        /// <param name="allowZero">Flag to indicate if zero values are allowed.</param>
        /// <returns>Double.</returns>
        public static double GetDouble(string question, double defaultAnswer, bool positiveOnly, bool allowZero)
        {
            while (true)
            {
                Console.Write(question);
                Console.Write(" [" + defaultAnswer + "] ");

                string userInput = Console.ReadLine();

                if (String.IsNullOrEmpty(userInput))
                {
                    return defaultAnswer;
                }

                double ret = 0;
                if (!double.TryParse(userInput, out ret))
                {
                    Console.WriteLine("Please enter a valid double.");
                    continue;
                }

                if (ret == 0)
                {
                    if (allowZero)
                    {
                        return 0;
                    }
                }

                if (ret < 0)
                {
                    if (positiveOnly)
                    {
                        Console.WriteLine("Please enter a value greater than zero.");
                        continue;
                    }
                }

                return ret;
            }
        }

        /// <summary>
        /// Retrieve a dictionary from the console.
        /// </summary>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <param name="keyQuestion">Question prompt for the key.</param>
        /// <param name="valQuestion">Question prompt for the value.</param>
        /// <returns>Dictionary.</returns>
        public static Dictionary<TKey, TValue> GetDictionary<TKey, TValue>(string keyQuestion, string valQuestion)
        {
            Dictionary<TKey, TValue> d = new Dictionary<TKey, TValue>();

            while (true)
            {
                string keyStr = GetString(keyQuestion, null, true);
                if (String.IsNullOrEmpty(keyStr)) break;

                string valStr = GetString(valQuestion, null, true);

                TKey key = (TKey)Convert.ChangeType(keyStr, typeof(TKey));

                TValue val = default(TValue);
                if (!String.IsNullOrEmpty(valStr)) val = (TValue)Convert.ChangeType(valStr, typeof(TValue));

                d.Add(key, val);
            }

            return d;
        }

        /// <summary>
        /// Retrieve a name value collection from the console.
        /// </summary>
        /// <param name="keyQuestion">Question prompt for the key.</param>
        /// <param name="valQuestion">Question prompt for the value.</param>
        /// <param name="caseInsensitive">Flag indicating if the NameValueCollection should be case insensitive.</param>
        /// <returns>NameValueCollection.</returns>
        public static NameValueCollection GetNameValueCollection(string keyQuestion, string valQuestion, bool caseInsensitive = true)
        {
            NameValueCollection nvc;
            if (caseInsensitive) nvc = new NameValueCollection(StringComparer.InvariantCultureIgnoreCase);
            else nvc = new NameValueCollection();

            while (true)
            {
                string key = GetString(keyQuestion, null, true);
                if (String.IsNullOrEmpty(key)) break;

                string val = GetString(valQuestion, null, true);

                nvc.Add(key, val);
            }

            return nvc;
        }

        /// <summary>
        /// Retrieve a DateTime from the console.  The formats specified in DateTimeFormats will be evaluated.
        /// </summary>
        /// <param name="question">Question prompt for the DateTime.</param>
        /// <returns>DateTime.</returns>
        public static DateTime GetDateTime(string question)
        {
            DateTime ret;

            string timestamp = GetString(question, null, false);

            if (DateTime.TryParse(timestamp, out ret)) return ret;

            foreach (string format in _DateTimeFormats)
            {
                if (DateTime.TryParseExact(timestamp, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out ret))
                    return ret;
            }

            throw new FormatException("The specified timestamp could not be parsed from any supplied formats.");
        }

        /// <summary>
        /// Retrieve a DateTime from the console.  The formats specified in DateTimeFormats will be evaluated.
        /// </summary>
        /// <param name="question">Question prompt for the DateTime.</param>
        /// <param name="defaultAnswer">Default answer.</param>
        /// <returns>DateTime.</returns>
        public static DateTime GetDateTime(string question, DateTime defaultAnswer)
        {
            DateTime ret;

            string timestamp = GetString(question, defaultAnswer.ToString(), false);

            if (DateTime.TryParse(timestamp, out ret)) return ret;

            foreach (string format in _DateTimeFormats)
            {
                if (DateTime.TryParseExact(timestamp, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out ret))
                    return ret;
            }

            throw new FormatException("The specified timestamp could not be parsed from any supplied formats.");
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}
