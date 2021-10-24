using System;

namespace FileCabinetApp
{
    public static class FileCabinetConsts
    {
        // General constants
        public const string DeveloperName = "Maksim Prudnikau";

        public const string ValidationRulesFileName = "validation-rules.json";
        public const string FileSystemFileName = "cabinet-records.db";

        // Constants in charge of running the application
        public const int CommandHelpIndex = 0;
        public const int ExplanationHelpIndex = 2;
        
        public static readonly string[][] HelpMessages = {
            new[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };
        
        // Constants for 'SetValidationRule'
        public const string CustomValidationRuleFullForm = "--validation-rules=custom";
        public const string CustomValidationRuleShortForm = "-vcustom";
        public const string ServiceStorageFileFullForm = "--storage=file";
        public const string ServiceStorageFileShortForm = "-sfile";
        public const string UseStopWatch = "--use-stopwatch";

        public const string InputDateFormat = "dd/MM/yyyy";
        public const string OutputDateFormat = "dd-MMM-yyyy";
        
        // Constants for 'ReadParameters'
        public const int MinimalJobExperience = 0;
        public const int MinimalSalary = 250;

        public static readonly DateTime MinimalDateTime = new (1950, 1, 1);
        public static readonly DateTime MaximalDateTime = DateTime.Now;

        public const char CsvDelimiter = ',';
        public static readonly char[] Grades = {'F', 'D', 'C', 'B', 'A'};
    }
}