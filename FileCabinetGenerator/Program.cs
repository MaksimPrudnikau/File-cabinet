
namespace FileCabinetGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options(args);
            var generator = new RecordGenerator();
            var record = generator.Generate(options.StartId, options.Count);
        }
    }
}
