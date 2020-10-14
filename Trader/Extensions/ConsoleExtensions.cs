using System;
using System.Linq;

namespace Trader
{
    static class ConsoleExtensions
    {
        public static void WithColor(string text, ConsoleColor consoleColor)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(text);
            Console.ForegroundColor = currentColor;
        }

        public static void WriteTable<T>(T[] rows, string[] columnNames, Func<T, string[]> getRow)
        {
            var data = rows.Select(r => getRow(r)).ToArray();
            var maxLengths = columnNames.Select(x => x.Length).ToList();
            foreach(var dataRow in data)
            {
                var i = 0;
                foreach(var value in dataRow)
                {
                    maxLengths[i] = Math.Max(maxLengths[i], value.Length);
                    i++;
                }
            }

            for(var i = 0; i < columnNames.Length; i++)
            {
                if (i == 0)
                {
                    Console.Write(columnNames[i].PadRight(maxLengths[i]));
                }
                else
                {
                    Console.Write(' ');
                    Console.Write(columnNames[i].PadRight(maxLengths[i]));
                }
            }
            Console.WriteLine();

            foreach (var dataRow in data) {
                for (var i = 0; i < dataRow.Length; i++)
                {
                    if (i == 0)
                    {
                        Console.Write(dataRow[i].PadRight(maxLengths[i]));
                    }
                    else
                    {
                        Console.Write(' ');
                        Console.Write(dataRow[i].PadRight(maxLengths[i]));
                    }
                }
                Console.WriteLine();
            }

        }
    }
}
