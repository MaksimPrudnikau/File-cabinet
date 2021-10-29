namespace FileCabinetApp.Handlers
{
    public class AppCommandRequest
    {
        public RequestCommand Command { get; set; }
        
        public string Parameters { get; set; }
    }
}