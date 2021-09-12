namespace FileCabinetApp
{
    public interface IFileCabinetService
    {
        public int CreateRecord(Parameter parameters);

        public void EditRecord(Parameter parameters);
    }
}