using System;
using System.Collections.Generic;
using System.IO;
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

        private readonly byte[] _status;
        private readonly byte[] _id;
        private readonly byte[] _firstName;
        private readonly byte[] _lastName;
        private readonly byte[] _year;
        private readonly byte[] _month;
        private readonly byte[] _day;
        private readonly byte[] _jobExperience;
        private readonly byte[] _wage;
        private readonly byte[] _rank;

        public byte[] GetStatus() => _status;

        public byte[] GetId() => _id;

        public byte[] GetFirstName() => _firstName;

        public byte[] GetLastName() => _lastName;

        public byte[] GetYear() => _year;

        public byte[] GetMonth() => _month;

        public byte[] GetDay() => _day;

        public byte[] GetJobExperience() => _jobExperience;

        public byte[] GetWage() => _wage;

        public byte[] GetRank() => _rank;


        public FilesystemRecord(Parameter parameter)
        {
            if (parameter is null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            const short status = 1;
            _status = BitConverter.GetBytes(status);
            _id = BitConverter.GetBytes(parameter.Id);
            _firstName = ToBytes(parameter.FirstName, NameCapacity);
            _lastName = ToBytes(parameter.LastName, NameCapacity);
            _year = BitConverter.GetBytes(parameter.DateOfBirth.Year);
            _month = BitConverter.GetBytes(parameter.DateOfBirth.Month);
            _day = BitConverter.GetBytes(parameter.DateOfBirth.Day);
            _jobExperience = BitConverter.GetBytes(parameter.JobExperience);
            _wage = BitConverter.GetBytes(decimal.ToDouble(parameter.Wage));
            _rank = BitConverter.GetBytes(parameter.Rank);
        }

        public FilesystemRecord(byte[] source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            _id = source[IdIndex..FirstNameIndex];

            _firstName = source[FirstNameIndex..LastNameIndex];

            _lastName = source[LastNameIndex..YearIndex];

            _year = source[YearIndex..MonthIndex];

            _month = source[MonthIndex..DayIndex];

            _day = source[DayIndex..JobExperienceIndex];

            _jobExperience = source[JobExperienceIndex..WageIndex];

            _wage = source[WageIndex..RankIndex];

            _rank = source[RankIndex..];
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

        public void Serialize(FileStream stream)
        {
            if (stream is null)
            {
                return;
            }

            stream.Write(GetStatus(), 0, GetStatus().Length);

            stream.Write(GetId(), 0, GetId().Length);

            stream.Write(GetFirstName(), 0, GetFirstName().Length);

            stream.Write(GetLastName(), 0, GetLastName().Length);

            stream.Write(GetYear(), 0, GetYear().Length);

            stream.Write(GetMonth(), 0, GetMonth().Length);

            stream.Write(GetDay(), 0, GetDay().Length);

            stream.Write(GetJobExperience(), 0, GetJobExperience().Length);

            stream.Write(GetWage(), 0, GetWage().Length);

            stream.Write(GetRank(), 0, GetRank().Length);

            stream.Flush();
        }

        public FileCabinetRecord ToFileCabinetRecord()
        {
            return new FileCabinetRecord
            {
                Id = BitConverter.ToInt32(GetId()),
                FirstName = Encoding.UTF8.GetString(GetFirstName()),
                LastName = Encoding.UTF8.GetString(GetLastName()),
                DateOfBirth = new DateTime(
                    BitConverter.ToInt32(GetYear()),
                    BitConverter.ToInt32(GetMonth()),
                    BitConverter.ToInt32(GetDay())
                ),
                JobExperience = BitConverter.ToInt16(GetJobExperience()),
                Wage = new decimal(BitConverter.ToDouble(GetWage())),
                Rank = Encoding.UTF8.GetString(GetRank())[0]
            };
        }
    }
}