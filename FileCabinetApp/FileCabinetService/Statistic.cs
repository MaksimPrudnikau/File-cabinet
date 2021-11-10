namespace FileCabinetApp.FileCabinetService
{
    public class Statistic
    {
        public int Count { get; set; }
        public int Deleted { get; set; }

        public void Clear()
        {
            Count = 0;
            Deleted = 0;
        }
    }
}