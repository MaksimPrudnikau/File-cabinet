
namespace FileCabinetGenerator
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var options = new Options(args);
            var generator = new Generator.RecordGenerator();
            generator.Export(options);
        }
    }
}
