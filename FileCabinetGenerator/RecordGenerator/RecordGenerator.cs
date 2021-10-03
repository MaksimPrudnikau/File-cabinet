using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using FileCabinetApp;

namespace FileCabinetGenerator
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
            
            var firstNamesFilePath = Path.Combine(fileCabinetGeneratorDir!, "RecordGenerator", FirstNamesFile);
            var lastNamesFilePath = Path.Combine(fileCabinetGeneratorDir, "RecordGenerator", LastNamesFile);

            _firstNames = ReadHash(firstNamesFilePath);
            _lastNames = ReadHash(lastNamesFilePath);
        }

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

        public IReadOnlyCollection<FileCabinetRecord> Generate(int startId, long amount)
        {
            var records = new List<FileCabinetRecord>();
            for (int i = 0, id = startId; i < amount; i++, id++)
            {
                records.Add(GetRandomRecord(id));
            }

            return records;
        }

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

        private string GenerateFirstName()
        {
            var randomLineNumber = new Random().Next(0, _firstNames.Count);
            return $"{_firstNames[randomLineNumber]}";
        }

        private string GenerateLastName()
        {
            var randomLineNumber = new Random().Next(0, _lastNames.Count);
            return $"{_lastNames[randomLineNumber]}";
        }

        private DateTime GenerateDate()
        {
            var randomDate = FileCabinetConsts.MinimalDateTime;
            var range = (FileCabinetConsts.MaximalDateTime - randomDate).Days;           
            return randomDate.AddDays(new Random().Next(range));
        }
    }
}