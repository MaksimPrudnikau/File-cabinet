using System;
using System.Collections.Generic;
using System.IO;

namespace FileCabinetApp.FileCabinetService.FileSystemService
{
    public class FileSystemWriter
    {
        private readonly FileCabinetFilesystemService _service;
        private readonly FileStream _file;
        private readonly Index _index;

        public FileSystemWriter(FileCabinetFilesystemService service, FileStream stream, Index index)
        {
            _service = service;
            _file = stream;
            _index = index;
        }

        /// <summary>
        /// Add records to the end of data base
        /// </summary>
        /// <param name="records"></param>
        public void AppendRange(IReadOnlyCollection<FileCabinetRecord> records)
        {
            if (records is null || records.Count == 0)
            {
                return;
            }
            
            foreach (var item in records)
            {
                _service.CreateRecord(item);
            }
        }

        /// <summary>
        /// Overwrite the existing record by any one from records. After overwriting record is removed from array.
        /// </summary>
        /// <param name="read">Read from database record</param>
        /// <param name="records">Source records</param>
        /// <exception cref="ArgumentNullException">Record or records array are empty</exception>
        public void RewriteWithAny(FileCabinetRecord read, ICollection<FileCabinetRecord> records)
        {
            if (read is null)
            {
                throw new ArgumentNullException(nameof(read));
            }

            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            foreach (var item in records)
            {
                if (item.Id != read.Id) continue;
                
                _index.Edit(read, item, _file.Position);
                Write(item);
                records.Remove(item);
                break;
            }
        }

        /// <summary>
        /// Mark record with source id as deleted
        /// </summary>
        /// <param name="id">Source record's id</param>
        /// <exception cref="ArgumentException">Record is already marked as deleted. The flag's value is wrong initially</exception>
        public void MarkAsDeleted(int id)
        {
            var buffer = new byte[FilesystemRecord.Size];
                    
            _file.Read(buffer);
            _file.Position -= FilesystemRecord.Size;

            var deletedFlag = buffer[FilesystemRecord.IsDeletedIndex];
                    
            buffer[FilesystemRecord.IsDeletedIndex] = deletedFlag switch
            {
                0 => 1,
                1 => throw new ArgumentException($"Record #{id} is already deleted"),
                _ => throw new ArgumentException($"Error isDeleted flag ({deletedFlag})")
            };

            _file.Write(buffer);
        }

        /// <summary>
        /// Write the record to data base
        /// </summary>
        /// <param name="record">Record to write</param>
        /// <exception cref="ArgumentNullException">Record is null</exception>
        private void Write(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            
            _service.CreateRecord(record);
            _file.Position -= FilesystemRecord.Size;
        }
    }
}