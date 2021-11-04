using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using FileCabinetApp.FileCabinetService.FileSystemService;

namespace FileCabinetApp.FileCabinetService.Iterators
{
    public class FilesystemIterator : IRecordIterator
    {
        private readonly FileSystemReader _reader;
        private readonly IList<long> _positions;
        private int _currentPosition;
        
        public FilesystemIterator(FileSystemReader reader, IList<long> positions)
        {
            if (positions is null)
            {
                throw new ArgumentNullException(nameof(positions));
            }
            
            _reader = reader;
            _positions = positions;
            _currentPosition = 0;
        }

        public FileCabinetRecord GetNext()
        {
            _reader.BaseFile.Seek(_positions[_currentPosition++], SeekOrigin.Begin);
            var readRecord = _reader.ReadRecord();
            if (!readRecord.IsDeleted)
            {
                return readRecord.ToFileCabinetRecord();
            }

            return null;
        }

        public bool HasMore()
        {
            return _currentPosition < _positions.Count;
        }

        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            while (HasMore())
            {
                yield return GetNext();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}