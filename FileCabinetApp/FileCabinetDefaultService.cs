namespace FileCabinetApp
{
    public class FileCabinetDefaultService : FileCabinetService
    {
        public FileCabinetDefaultService() : base(new DefaultValidator()) { }
        
        protected override IRecordValidator CreateValidator()
        {
            return new DefaultValidator();
        }
    }
}