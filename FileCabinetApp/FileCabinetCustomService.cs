namespace FileCabinetApp
{
    public class FileCabinetCustomService : FileCabinetService
    {
        public FileCabinetCustomService() : base(new CustomValidator()) { }
        
        protected override IRecordValidator CreateValidator()
        {
            return new CustomValidator();
        }
    }
}