using System;
using System.Collections.Generic;
using System.IO;

namespace FileCabinetApp.FileCabinetService.FileSystemService
{
    public class FileSystemReader
    {
        public FileStream BaseFile { get; }

        public FileSystemReader(FileStream stream)
        {
            BaseFile = stream;
        }

        /// <summary>
        /// Read the record and move cursor to it's beginning
        /// </summary>
        /// <returns></returns>
        public FileCabinetRecord ReadAndMoveCursorBack()
        {
            var read = ReadRecord().ToFileCabinetRecord();
            BaseFile.Position -= FilesystemRecord.Size;
            return read;
        }
        
        /// <summary>
        /// Read one record from base file consistently
        /// </summary>
        /// <returns>Read <see cref="FileCabinetRecord"/> object</returns>
        public FilesystemRecord ReadRecord()
        {
            var buffer = new byte[FilesystemRecord.Size];
            BaseFile.Read(buffer, 0, buffer.Length);
            return new FilesystemRecord(buffer);
        }
        
        /// <summary>
        /// Deserialize all content from source file into <see cref="FilesystemRecord"/> array
        /// </summary>
        /// <returns><see cref="FileCabinetRecord"/> array</returns>
        /// <exception cref="ArgumentNullException">stream is null</exception>
        /// <exception cref="ArgumentException">The file size does not correspond to the integer number of occurrences of the records</exception>
        public IEnumerable<FileCabinetRecord> Deserialize()
        {
            var array = new List<FileCabinetRecord>();

            BaseFile.Seek(0, SeekOrigin.Begin);
            while (BaseFile.Position < BaseFile.Length)
            {
                var read = ReadRecord();
                
                if (!read.IsDeleted)
                {
                    array.Add(read.ToFileCabinetRecord());
                }
            }

            foreach (var item in array.ToArray())
            {
                yield return item;
            }
        }
    }
}