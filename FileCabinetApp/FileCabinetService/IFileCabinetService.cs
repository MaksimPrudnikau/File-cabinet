namespace FileCabinetApp
{
    public interface IFileCabinetService
    {
        public Parameter ReadParameters(int id = -1);
        
        public int CreateRecord(Parameter parameters);

        public void EditRecord(Parameter parameters);
    }
}