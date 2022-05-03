using System;
using System.Collections.Generic;

namespace GetSomeInput
{
    /// <summary>
    /// Inputty is a helpful library for getting values from the console.
    /// </summary>
    public static class Inputty
    {
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
    }
}
