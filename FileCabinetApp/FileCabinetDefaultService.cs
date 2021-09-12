namespace FileCabinetApp
{
    public class FileCabinetDefaultService : FileCabinetService
    {
        protected static DefaultValidator CreateValidator(IRecordValidator validator)
        {
            return new DefaultValidator();
        }

        protected override IRecordValidator CreateValidator()
        {
            return new DefaultValidator();
        }
    }
}