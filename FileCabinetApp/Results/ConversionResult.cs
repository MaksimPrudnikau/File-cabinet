namespace FileCabinetApp.Results
{
    public class ConversionResult<T> : OperationResult
    {
        public T Result { get; init; }
    }
}