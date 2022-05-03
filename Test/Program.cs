using System;
using GetSomeInput;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string answer1 = Inputty.GetString("What's your name?", null, false);
            int answer2 = Inputty.GetInteger("What's your age?", 42, true, true);
        }
    }
}
