using System;
using System.Text;

namespace FileCabinetApp
{
    public class FilesystemRecord
    {
        private const int NameCapacity = 120;
        public const int Size = 269;

        private const ushort IdIndex = 2;
        private const ushort FirstNameIndex = 6;
        private const ushort LastNameIndex = 126;
        private const ushort YearIndex = 246;
        private const ushort MonthIndex = 250;
        private const ushort DayIndex = 254;
        private const ushort JobExperienceIndex = 258;
        private const ushort WageIndex = 260;
        private const ushort RankIndex = 268;
        
        public byte[] Status { get; }
        public byte[] Id { get; }
        public byte[] FirstName { get; }
        public byte[] LastName { get; }
        public byte[] Year { get; }
        public byte[] Month { get; }
        public byte[] Day { get; }
        public byte[] JobExperience { get; }
        public byte[] Wage { get; }
        public byte[] Rank { get; }

        public FilesystemRecord(Parameter parameter)
        {
            if (parameter is null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            const short status = 1;
            Status = BitConverter.GetBytes(status);
            Id = BitConverter.GetBytes(parameter.Id);
            FirstName = ToBytes(parameter.FirstName, NameCapacity);
            LastName = ToBytes(parameter.LastName, NameCapacity);
            Year = BitConverter.GetBytes(parameter.DateOfBirth.Year);
            Month = BitConverter.GetBytes(parameter.DateOfBirth.Month);
            Day = BitConverter.GetBytes(parameter.DateOfBirth.Day);
            JobExperience = BitConverter.GetBytes(parameter.JobExperience);
            Wage = BitConverter.GetBytes(decimal.ToDouble(parameter.Wage));
            Rank = BitConverter.GetBytes(parameter.Rank);
        }

        public FilesystemRecord(byte[] source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            
            Status = source[..IdIndex];
            
            Id = source[IdIndex..FirstNameIndex];

            FirstName = source[FirstNameIndex..LastNameIndex];
            
            LastName = source[LastNameIndex..YearIndex];
            
            Year = source[YearIndex..MonthIndex];
            
            Month = source[MonthIndex..DayIndex];
            
            Day = source[DayIndex..JobExperienceIndex];

            JobExperience = source[JobExperienceIndex..WageIndex];

            Wage = source[WageIndex..RankIndex];

            Rank = source[RankIndex..];
        }

        private static byte[] ToBytes(string value, int capacity)
        {
            var encoded = Encoding.UTF8.GetBytes(value);
            var byteArray = new byte[capacity];
            for (var i = 0; i < encoded.Length; i++)
            {
                byteArray[i] = encoded[i];
            }

            return byteArray;
        }
    }
}