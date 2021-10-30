using System;
using System.Collections.Generic;
using System.IO;

namespace FileCabinetApp.FileCabinetService.FileSystemService
{
    public class FileSystemReader
    {
        private readonly FileStream _file;
        
        public FileSystemReader(FileStream stream)
        {
            _file = stream;
        }

        /// <summary>
        /// Read the record and move cursor to it's beginning
        /// </summary>
        /// <returns></returns>
        public FileCabinetRecord ReadAndMoveCursorBack()
        {
            var read = ReadRecord().ToFileCabinetRecord();
            _file.Position -= FilesystemRecord.Size;
            return read;
        }
        
        /// <summary>
        /// Read one record from base file consistently
        /// </summary>
        /// <returns>Read <see cref="FileCabinetRecord"/> object</returns>
        public FilesystemRecord ReadRecord()
        {
            var buffer = new byte[FilesystemRecord.Size];
            _file.Read(buffer, 0, buffer.Length);
            return new FilesystemRecord(buffer);
        }
        
        /// <summary>
        /// Deserialize all content from source file into <see cref="FilesystemRecord"/> array
        /// </summary>
        /// <returns><see cref="FileCabinetRecord"/> array</returns>
        /// <exception cref="ArgumentNullException">stream is null</exception>
        /// <exception cref="ArgumentException">The file size does not correspond to the integer number of occurrences of the records</exception>
        public FileCabinetRecord[] Deserialize()
        {
            var array = new List<FileCabinetRecord>();

            _file.Seek(0, SeekOrigin.Begin);
            while (_file.Position < _file.Length)
            {
                var read = ReadRecord();
                
                if (!read.IsDeleted)
                {
                    array.Add(read.ToFileCabinetRecord());
                }
            }

            return array.ToArray();
        }
    }
}