using System;

namespace FileCabinetApp
{
    public class CustomLastNameValidator : IRecordValidator
    {
        /// <summary>
        /// Validate name either first or last
        /// </summary>
        /// <param name="name">first or last name</param>
        /// <exception cref="ArgumentException">Entered name is null or whitespace or it`s length is less than 2 or greater than 60</exception>
        public static ValidationResult Validate(string name)
        {
            var result = new ValidationResult {Parsed = false, StringRepresentation = name};
            
            if (string.IsNullOrWhiteSpace(name))
            {
                result.Message = RecordValidatorConsts.NameIsNullOrWhiteSpace;
                return result;
            }

            if (name.Length is < 2 or > 60)
            {
                result.Message = RecordValidatorConsts.NameWrongLength;
                return result;
            }

            foreach (var item in name)
            {
                if (!char.IsLetter(item))
                {
                    result.Message = RecordValidatorConsts.TheNameIsNotLettersOnly;
                    return result;
                }
            }

            result.Parsed = true;
            return result;
        }
        
        public void Validate(FileCabinetRecord record)
        {
            if (string.IsNullOrWhiteSpace(record.FirstName))
            {
                throw new ArgumentException(RecordValidatorConsts.NameIsNullOrWhiteSpace);
            }

            if (record.FirstName.Length is < 2 or > 60)
            {
                throw new ArgumentException(RecordValidatorConsts.NameWrongLength);
            }

            foreach (var item in record.FirstName)
            {
                if (!char.IsLetter(item))
                {
                    throw new ArgumentException(RecordValidatorConsts.TheNameIsNotLettersOnly);
                }
            }
        }
    }
}