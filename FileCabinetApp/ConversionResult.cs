namespace FileCabinetApp
{
    public class ConversionResult<T> : OperationResult
    {
        public T Result { get; init; }
    }
}