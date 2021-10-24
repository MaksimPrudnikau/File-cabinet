using System;
using System.Diagnostics;
using FileCabinetApp.Handlers;

namespace FileCabinetApp
{
    public static class ServiceMeter
    {
        private static readonly Stopwatch Stopwatch = new();

        public static void TakeTime(CommandHandlerBase command, AppCommandRequest request)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            
            Stopwatch.Start();
            command.Handle(request);
            Stopwatch.Stop();

            Console.WriteLine(EnglishSource.method_execution_duration, request.Command, Stopwatch.Elapsed.Ticks);
            Stopwatch.Reset();
        }
    }
}