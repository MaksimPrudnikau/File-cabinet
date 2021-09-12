namespace FileCabinetApp
{
    public struct ValidationResult
    {
        public bool Parsed { get; }
        public string StringRepresentation { get; }

        public ValidationResult(bool parsed, string stringRepresentation)
        {
            Parsed = parsed
        }
    }
}