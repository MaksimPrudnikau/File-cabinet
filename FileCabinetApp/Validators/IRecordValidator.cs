using System;

namespace FileCabinetApp
{
    public interface IRecordValidator
    {
        public void Validate(FileCabinetRecord record);
    }
}