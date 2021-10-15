using System;

namespace FileCabinetApp
{
    public class DefaultNameValidator : IRecordValidator
    {
        /// <summary>
        /// Validate name either first or last
        /// </summary>
        /// <param name="name">first or last name</param>
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