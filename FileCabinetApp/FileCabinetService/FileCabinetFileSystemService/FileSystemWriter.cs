using System;
using System.Collections.Generic;
using System.IO;

namespace FileCabinetApp
{
    public class FileSystemWriter
    {
        private readonly FileCabinetFilesystemService _service;
        private readonly FileStream _file;

        public FileSystemWriter(FileCabinetFilesystemService service, FileStream stream)
        {
            _service = service;
            _file = stream;
        }

        public void Seek(long offset, SeekOrigin origin = SeekOrigin.Begin)
        {
            _file.Seek(offset, origin);
        }
        
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
                
                Rewrite(item);
                records.Remove(item);
                break;
            }
        }

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
                _ => throw new ArgumentException($"Error isDeleted flat ({deletedFlag})")
            };

            _file.Write(buffer);
        }

        private void Rewrite(FileCabinetRecord record)
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