namespace FileCabinetApp
{
    public class FileCabinetCustomService : FileCabinetService
    {
        protected static DefaultValidator CreateValidator(IRecordValidator validator)
        {
            return new DefaultValidator();
        }

        protected override IRecordValidator CreateValidator()
        {
            return new CustomValidator();
        }
    }
}