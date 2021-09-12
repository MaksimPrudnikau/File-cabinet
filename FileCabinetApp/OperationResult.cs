namespace FileCabinetApp
{
    public abstract class OperationResult
    {
        public bool Parsed { get; protected init; }
        
        public string StringRepresentation { get; protected init; }
    }
}