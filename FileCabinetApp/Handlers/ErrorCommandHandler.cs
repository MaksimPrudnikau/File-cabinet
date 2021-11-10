using System;
using System.Collections.Generic;
using System.Text;

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
                var isPart = IsPartOfCommand(command, item);
                if (isPart)
                {
                    closestCommands.Clear();
                    maxMatch = 0;
                    closestCommands.Add(new Tuple<int, string>(maxMatch, item));
                    break;
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

            foreach (var (match, closestCommand) in closestCommands)
            {
                if (match == maxMatch)
                {
                    Console.WriteLine(closestCommand);
                }
            }
        }

        private static bool IsPartOfCommand(string source, string command)
        {
            return command.Contains(source, StringComparison.InvariantCultureIgnoreCase);
        }

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