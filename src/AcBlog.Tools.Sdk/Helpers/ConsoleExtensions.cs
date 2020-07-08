using System;
using System.Text;

namespace AcBlog.Tools.Sdk.Helpers
{

    static class ConsoleExtensions
    {
        public static string Input(string prompt = "")
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        public static string InputPassword(string prompt = "")
        {
            Console.Write(prompt);
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                sb.Append(key.KeyChar);
            }
            return sb.ToString();
        }
    }
}
