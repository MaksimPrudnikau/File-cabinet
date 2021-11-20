using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using FileCabinetApp;
using FileCabinetApp.Export;
using FileCabinetApp.FileCabinetService;
using FileCabinetGenerator.RecordGenerator;

namespace FileCabinetGenerator.Generator
{
    public class RecordGenerator
    {
        private const string FirstNamesFile = "FirstNames.txt";
        private const string LastNamesFile = "LastNames.txt";
        
        private readonly Hashtable _firstNames;
        private readonly Hashtable _lastNames;
        
        public RecordGenerator()
        {
            var fileCabinetDir = new DirectoryInfo(Environment.CurrentDirectory);
            var fileCabinetGeneratorDir = fileCabinetDir.Parent?.Parent?.Parent?.FullName;
            
            var firstNamesFilePath = Path.Combine(fileCabinetGeneratorDir!, "Generator", FirstNamesFile);
            var lastNamesFilePath = Path.Combine(fileCabinetGeneratorDir, "Generator", LastNamesFile);

            try
            {
                _firstNames = ReadHash(firstNamesFilePath);
                _lastNames = ReadHash(lastNamesFilePath);
            }
            catch (Exception exception) when (exception is ArgumentException or IOException)
            {
                Console.Error.WriteLine(exception.Message);
            }
        }

        /// <summary>
        /// Read content from file with first or last names and create hashtable from it
        /// </summary>
        /// <param name="filename">Full path of file to read</param>
        /// <returns><see cref="Hashtable"/> of read file</returns>
        private Hashtable ReadHash(string filename)
        {
            var table = new Hashtable();
            var lines = File.ReadAllLines(filename);
            for (var i = 0; i < lines.Length; i++)
            {
                table.Add(i + 1, lines[i]);
            }

            return table;
        }

        /// <summary>
        /// Generate <see cref="FileCabinetRecord"/> array
        /// </summary>
        /// <param name="startId">The initial id from which the numbering begins</param>
        /// <param name="amount">Total amount of records to generate</param>
        /// <returns><see cref="FileCabinetRecord"/> array where firstname, lastname and date of birth
        /// are created randomly. Other properties are set as default</returns>
        private IEnumerable<FileCabinetRecord> Generate(int startId, long amount)
        {
            var records = new List<FileCabinetRecord>();
            for (int i = 0, id = startId; i < amount; i++, id++)
            {
                records.Add(GetRandomRecord(id));
            }

            return records;
        }

        /// <summary>
        /// Create <see cref="FileCabinetRecord"/> with random first name, last name and date of birth.
        /// Other properties are set as default
        /// </summary>
        /// <param name="id">Source id for created record</param>
        /// <returns></returns>
        private FileCabinetRecord GetRandomRecord(int id)
        {
            var firstName = GenerateFirstName();

            var lastName = GenerateLastName();

            var dateOfBirth = GenerateDate();

            return new FileCabinetRecord
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth
            };
        }

        /// <summary>
        /// Get random first name from read firs names <see cref="Hashtable"/>
        /// </summary>
        /// <returns><see cref="string"/> contains first name</returns>
        private string GenerateFirstName()
        {
            var randomLineNumber = new Random().Next(0, _firstNames.Count);
            return $"{_firstNames[randomLineNumber]}";
        }

        /// <summary>
        /// Get random last name from read last names <see cref="Hashtable"/>
        /// </summary>
        /// <returns><see cref="string"/> contains first name</returns>
        private string GenerateLastName()
        {
            var randomLineNumber = new Random().Next(0, _lastNames.Count);
            return $"{_lastNames[randomLineNumber]}";
        }

        /// <summary>
        /// Get random date of birth from period of minimal and maximal date time specified in <see cref="FileCabinetConsts"/>
        /// </summary>
        /// <returns>Generated <see cref="DateTime"/> object</returns>
        private DateTime GenerateDate()
        {
            var randomDate = FileCabinetConsts.MinimalDateOfBirth;
            var range = (FileCabinetConsts.MaximalDateOfBirth - randomDate).Days;           
            return randomDate.AddDays(new Random().Next(range));
        }

        /// <summary>
        /// Export generated <see cref="FileCabinetRecord"/> array to source path
        /// </summary>
        /// <param name="options">Read options</param>
        public void Export(Options options)
        {
            try
            {
                OptionsValidator.Validate(options);
                using var outputFile = new StreamWriter(options.FileName);
                            
                var snapshot = new FileCabinetServiceSnapshot(Generate(options.StartId, options.Count));
            
                switch (options.Type)
                {
                    case OutputType.Csv:
                        snapshot.SaveToCsv(outputFile);
                        break;
                    case OutputType.Xml:
                        snapshot.SaveToXml(outputFile);
                        break;
                }
            }
            catch (Exception exception) when (exception is ArgumentException or IOException)
            {
                Console.Error.WriteLine(exception.Message);
                return;
            }

            Console.WriteLine(EnglishSource.records_were_written_to, options.Count, options.FileName);
        }
    }
}