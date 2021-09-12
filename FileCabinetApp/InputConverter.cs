using System;
using System.Globalization;

namespace FileCabinetApp
{
    public static class InputConverter
    {
        /// <summary>
        /// Convert source string to its string representation
        /// </summary>
        /// <param name="source">Source name either first or last</param>
        /// <returns>Parsed <see cref="string"/> object</returns>
        public static ConversionResult<string> NameConverter(string source)
        {
            return new ConversionResult<string> 
                {Parsed = true, Result = source, StringRepresentation = source};
        }

        /// <summary>
        /// Converts source date of birth representation into it`s <see cref="DateTime"/> equivalent 
        /// </summary>
        /// <param name="dateOfBirth">Source date of birth</param>
        /// <returns>Parsed <see cref="DateTime"/> object</returns>
        public static ConversionResult<DateTime> DateOfBirthConverter(string dateOfBirth)
        {
            const string inputDateTimeFormat = "dd/MM/yyyy";
            
            var parsed = DateTime.TryParseExact(
                dateOfBirth,
                inputDateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var result);

            return new ConversionResult<DateTime>
                {Parsed = parsed, StringRepresentation = $"{result}", Result = result};
        }
        
        /// <summary>
        /// Converts source job experience representation into it`s <see cref="short"/> equivalent 
        /// </summary>
        /// <param name="jobExperience">Source job experience</param>
        /// <returns>Parsed <see cref="short"/> object</returns>
        public static ConversionResult<short> JobExperienceConverter(string jobExperience)
        {
            var parsed = short.TryParse(
                jobExperience,
                NumberStyles.None,
                CultureInfo.InvariantCulture,
                out var result);
            
            return new ConversionResult<short>
                {Parsed = parsed, StringRepresentation = $"{result}", Result = result};
        }

        /// <summary>
        /// Converts source wage representation into it`s <see cref="decimal"/> equivalent 
        /// </summary>
        /// <param name="wage">Source wage</param>
        /// <returns>Parsed <see cref="decimal"/> object</returns>
        public static ConversionResult<decimal> WageConverter(string wage)
        {
            var parsed = decimal.TryParse(
                wage,
                NumberStyles.None,
                CultureInfo.InvariantCulture,
                out var result);
            
            return new ConversionResult<decimal>
                {Parsed = parsed, StringRepresentation = $"{result}", Result = result};
        }
        
        /// <summary>
        /// Converts source rank representation into it`s <see cref="char"/> equivalent
        /// </summary>
        /// <param name="rank">Source rank</param>
        /// <returns>Parsed <see cref="char"/> object</returns>
        public static ConversionResult<char> RankConverter(string rank)
        {
            if (rank is null || rank.Length == 0)
            {
                return new ConversionResult<char>();
            }

            return new ConversionResult<char> {Parsed = true, StringRepresentation = $"{rank[0]}", Result = rank[0]};
        }
    }
}