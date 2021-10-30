using FileCabinetApp.FileCabinetService;

namespace FileCabinetApp.Validators
{
    public interface IRecordValidator
    {
        public void Validate(FileCabinetRecord record);
    }
}