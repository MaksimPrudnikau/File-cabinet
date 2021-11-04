
namespace FileCabinetApp.FileCabinetService.Iterators
{
    public interface IRecordIterator
    {
        public FileCabinetRecord GetNext();

        public bool HasMore();
    }
}