namespace FileCabinetApp
{
    public interface IRecordValidator
    {
        /// <summary>
        /// Validate all parameters
        /// </summary>
        /// <param name="parameters">Entered parameters</param>
        public void ValidateParameters(Parameter parameters);
    }
}