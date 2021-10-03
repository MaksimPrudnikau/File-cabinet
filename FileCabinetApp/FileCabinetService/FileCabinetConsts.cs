using System;

namespace FileCabinetApp
{
    public static class FileCabinetConsts
    {
        // General constants
        public const string DeveloperName = "Maksim Prudnikau";

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

        public const string InputDateFormat = "dd/MM/yyyy";
        
        // Constants for 'ReadParameters'
        public const int MinimalJobExperience = 0;
        public const int MinimalWage = 250;
        public const char MinimalRank = 'F';
        
        public static readonly DateTime MinimalDateTime = new (1950, 1, 1);
        public static readonly DateTime MaximalDateTime = DateTime.Now;

        public const string FileSystemFileName = "cabinet-records.db";
    }
}