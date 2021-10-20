using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FileCabinetApp
{
    public class FileCabinetRecord
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public short JobExperience { get; set; } = FileCabinetConsts.MinimalJobExperience;

        public decimal Salary { get; set; } = FileCabinetConsts.MinimalSalary;

        public char Rank { get; set; } = FileCabinetConsts.Grades[0];

        /// <summary>
        /// Read one record from base file consistently
        /// </summary>
        /// <param name="stream">Source file stream</param>
        /// <returns>Read <see cref="FileCabinetRecord"/> object</returns>
        /// <exception cref="ArgumentNullException">Source stream is null</exception>
        public static FilesystemRecord ReadRecord(FileStream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            
            var buffer = new byte[FilesystemRecord.Size];
            stream.Read(buffer, 0, buffer.Length);
            return new FilesystemRecord(buffer);
        }

        /// <summary>
        /// Deserialize all content from source file into <see cref="FilesystemRecord"/> array
        /// </summary>
        /// <param name="stream">Source file stream </param>
        /// <returns><see cref="FileCabinetRecord"/> array</returns>
        /// <exception cref="ArgumentNullException">stream is null</exception>
        /// <exception cref="ArgumentException">The file size does not correspond to the integer number of occurrences of the records</exception>
        public static FileCabinetRecord[] Deserialize(FileStream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (stream.Length % FilesystemRecord.Size != 0)
            {
                throw new ArgumentException(
                    "The file size does not correspond to the integer number of record occurrences.");
            }
            
            var array = new List<FileCabinetRecord>();

            var currentIndex = 0;
            stream.Seek(currentIndex, SeekOrigin.Begin);
            
            while (currentIndex < stream.Length)
            {
                var read = ReadRecord(stream);
                
                if (!read.IsDeleted)
                {
                    array.Add(read.ToFileCabinetRecord());
                }

                currentIndex += FilesystemRecord.Size;
            }

            return array.ToArray();
        }
    }
}