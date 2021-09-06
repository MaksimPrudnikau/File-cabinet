using System;

namespace FileCabinetApp
{
    public class FileCabinetCustomService : FileCabinetService
    {
        protected override void ValidateParameters(Parameter parameters)
        {
            JobExperienceValidator(parameters.JobExperience);
            WageValidator(parameters.Wage);
            RankValidator(parameters.Rank);
        }
        
         /// <summary>
        /// Get job experience from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Job experience is null.
        /// Job experience is not an integer or less than zero or greater than short.MaxValue.
        /// Job experience is less than zero or greater than 100.</exception>
        private static void JobExperienceValidator(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (!short.TryParse(source, out var jobExperience))
            {
                throw new ArgumentException(
                    "Job experience is not an integer or less than zero or greater than short.MaxValue");
            }

            if (jobExperience is < 0 or > 100)
            {
                throw new ArgumentException("Job experience is less than zero or greater than 100");
            }
        }
        
        /// <summary>
        /// Get wage from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Wage is null.
        /// Wage is not an integer or greater than decimal.MaxValue.
        /// Wage is less than zero</exception>
        private static void WageValidator(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException(nameof(source));
            }
            

            if (!decimal.TryParse(source, out var wage))
            {
                throw new ArgumentException(
                    "Wage is not an integer or greater than decimal.MaxValue");
            }

            if (wage < 0)
            {
                throw new ArgumentException("Wage is less than zero");
            }
        }
        
        /// <summary>
        /// Get rank from keyboard
        /// </summary>
        /// <exception cref="ArgumentException">Rank is not in current rank system</exception>
        private static void RankValidator(string source)
        {
            var grades = new []{'F', 'D', 'C', 'B', 'A'};
            
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException(nameof(source));
            }
            
            if (source.Length != 1 || Array.IndexOf(grades, source[0]) == -1)
            {
                throw new ArgumentException("Rank is not defined in current rank system [F..A]");
            }
        }
    }
}