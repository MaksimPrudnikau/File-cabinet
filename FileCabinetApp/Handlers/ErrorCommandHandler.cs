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

            var closestCommands = new List<Tuple<int, string>>();
            var maxMatch = 0;

            foreach (var item in Enum.GetNames(typeof(RequestCommand)))
            {
                if (IsPartOfCommand(command, item))
                {
                    Console.WriteLine(item);
                    return;
                }
                
                var match = GetStartWith(command, item);
                if (match > maxMatch)
                {
                    maxMatch = match;
                }
                
                if (match > 0)
                {
                    closestCommands.Add(new Tuple<int, string>(match, item));   
                }
            }
            
            PrintWithMaxMatch(closestCommands, maxMatch);
        }

        /// <summary>
        /// Prints all items from source record where match equals searchMatch
        /// </summary>
        /// <param name="matches">Source array of matches</param>
        /// <param name="searchMatch">Match to search</param>
        private static void PrintWithMaxMatch(IEnumerable<Tuple<int, string>> matches, int searchMatch)
        {
            foreach (var (match, closestCommand) in matches)
            {
                if (match == searchMatch)
                {
                    Console.WriteLine(closestCommand);
                }
            }
        }

        /// <summary>
        /// Determine if input command is a part of the source string
        /// </summary>
        /// <param name="source">Source string</param>
        /// <param name="command">Command to search</param>
        /// <returns>True if the input command is a part of the source</returns>
        private static bool IsPartOfCommand(string source, string command)
        {
            return command.Contains(source, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Counts the number of identical characters of command from the beginning of source 
        /// </summary>
        /// <param name="source">Source string</param>
        /// <param name="command">Source command</param>
        /// <returns>Total number of matches</returns>
        private static int GetStartWith(string source, string command)
        {
            var matches = 0;

            for (var i = 0; i < source.Length; i++)
            {
                if (i >= command.Length)
                {
                    break;
                }
            
                if (char.ToLowerInvariant(source[i]) == char.ToLowerInvariant(command[i]))
                {
                    matches++;
                }
            }

            return matches;
        }
    }
}