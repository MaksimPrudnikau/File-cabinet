namespace FileCabinetApp
{
    public abstract class OperationResult
    {
        public bool Parsed { get; init; }

        public string StringRepresentation { get; init; }
    }
}