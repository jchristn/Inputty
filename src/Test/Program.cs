﻿namespace Test
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using GetSomeInput;

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("");
            string answer1 = Inputty.GetString("What's your name?", null, false);

            Console.WriteLine("");
            int answer2 = Inputty.GetInteger("What's your age?", 42, true, true);

            Console.WriteLine("");
            Console.WriteLine("Building dictionary");
            Dictionary<string, string> dict = Inputty.GetDictionary<string, string>(
                "Key  :",
                "Value:");

            foreach (KeyValuePair<string, string> kvp in dict)
            {
                Console.WriteLine(kvp.Key + ": " + kvp.Value);
            }

            Console.WriteLine("");
            Console.WriteLine("Building name value collection");
            NameValueCollection nvc = Inputty.GetNameValueCollection(
                "Key  :",
                "Value:",
                true);

            for (int i = 0; i < nvc.AllKeys.Count(); i++)
            {
                string key = nvc.AllKeys[i];
                Console.WriteLine(key + ": " + nvc.Get(key));
            }

            Console.WriteLine("");
            DateTime dt = Inputty.GetDateTime("What time is it?");
            Console.WriteLine("You said: " + dt.ToString());

            Console.WriteLine("");
            dt = Inputty.GetDateTime("What time is it?", DateTime.UtcNow);
            Console.WriteLine("You said: " + dt.ToString());

            Console.WriteLine("");
            DateTime? dtn = Inputty.GetNullableDateTime("What time is it, or null?");
            Console.WriteLine("You said: " + (dtn == null ? "(null)" : dtn.Value.ToString()));

            Console.WriteLine("");
            dtn = Inputty.GetNullableDateTime("What time is it, or null?", DateTime.UtcNow);
            Console.WriteLine("You said: " + (dtn == null ? "(null)" : dtn.Value.ToString()));

            Console.WriteLine("");
            Guid guid = Inputty.GetGuid("Please provide a GUID:");
            Console.WriteLine("GUID: " + guid.ToString());
            Console.WriteLine("");
        }
    }
}
