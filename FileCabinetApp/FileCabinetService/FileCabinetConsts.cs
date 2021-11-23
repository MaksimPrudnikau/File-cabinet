using System;

namespace FileCabinetApp.FileCabinetService
{
    public static class FileCabinetConsts
    {
        // General constants
        public const string DeveloperName = "Maksim Prudnikau";

        public const string FileSystemFileName = "cabinet-records.db";
        public const string LogsFileName = "Logs.txt";

        // Constants for 'SetValidationRule'
        public const string CustomValidationRuleFullForm = "--validation-rules=custom";
        public const string CustomValidationRuleShortForm = "-vcustom";
        public const string ServiceStorageFileFullForm = "--storage=file";
        public const string ServiceStorageFileShortForm = "-sfile";
        public const string UseStopWatch = "--use-stopwatch";
        public const string UseLogger = "--use-logger";

        public const string InputDateFormat = "dd/MM/yyyy";
        public const string OutputDateFormat = "dd-MMM-yyyy";
        
        // Constants for 'ReadParameters'
        public const int MinimalJobExperience = 0;
        public const int MinimalSalary = 250;
        public static readonly DateTime MinimalDateOfBirth = new (1950, 1, 1);
        public static readonly DateTime MaximalDateOfBirth = DateTime.Now;
        public static readonly char[] Grades = {'F', 'D', 'C', 'B', 'A'};
    }
}