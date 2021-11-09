using System;
using System.Collections.Generic;

namespace FileCabinetApp.Handlers
{
    public static class ErrorCommandHandler
    {
        public static void Handle(string command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            Console.WriteLine(EnglishSource.ErrorCommandHandler_Handle_Is_Not_A_Command, command);
            Console.WriteLine(EnglishSource.The_most_similar_commands_are);

            var matches = new List<Tuple<int, string>>();
            var maxMatch = 0;

            foreach (var item in Enum.GetNames(typeof(RequestCommand)))
            {
                var match = GetMatchPercent(command, item);
                if (match > maxMatch)
                {
                    maxMatch = match;
                }

                if (match > 0)
                {
                    matches.Add(new Tuple<int, string>(match, item));   
                }
            }

            foreach (var item in matches)
            {
                if (item.Item1 == maxMatch)
                {
                    Console.WriteLine(item.Item2);
                }
            }
        }

        private static int GetMatchPercent(string source, string command)
        {
            var matches = 0;

            foreach (var item in source)
            {
                if (command.Contains(item, StringComparison.InvariantCultureIgnoreCase))
                {
                    matches++;
                }
            }

            return matches;
        }
    }
}