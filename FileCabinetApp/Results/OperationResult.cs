namespace FileCabinetApp
{
    public abstract class OperationResult
    {
        public bool Parsed { get; set; } = true;

        public string StringRepresentation { get; init; }
    }
}