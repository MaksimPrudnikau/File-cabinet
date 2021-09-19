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
        public const string DefaultValidationRuleFullForm = "--VALIDATION-RULES=DEFAULT";
        public const string CustomValidationRuleFullForm = "--VALIDATION-RULES=CUSTOM";
        public const int ParameterIndexFullForm = 0;
        public const int ParameterIndexShortForm = 1;

        // Constants for 'ReadParameters'
        public const int minimalJobExperience = 0;
        public const int minimalWage = 250;
        public const char minimalRank = 'F';
    }
}