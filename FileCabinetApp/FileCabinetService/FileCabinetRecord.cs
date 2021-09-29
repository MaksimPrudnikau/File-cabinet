using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    [Serializable]
    public class FileCabinetRecord
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }
        
        public short JobExperience { get; set; }
        
        public decimal Wage { get; set; }
        
        public char Rank { get; set; }

        public void Print()
        {
            Console.WriteLine(EnglishSource.print_record,
                Id,
                FirstName,
                LastName,
                DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture),
                JobExperience,
                Wage,
                Rank);
        }
        
        private static FileCabinetRecord Read(FileStream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            
            var buffer = new byte[FilesystemRecord.Size];
            stream.Read(buffer, 0, buffer.Length);
            stream.Seek(1, SeekOrigin.Current);
            return new FilesystemRecord(buffer).ToFileCabinetRecord();
        }

        public static FileCabinetRecord[] Deserialize(FileStream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            
            var array = new List<FileCabinetRecord>();

            var currentIndex = 0;
            stream.Seek(currentIndex, SeekOrigin.Begin);
            
            while (currentIndex < stream.Length)
            {
                array.Add(Read(stream));
                currentIndex += FilesystemRecord.Size + 1;
            }

            return array.ToArray();
        }
    }
}