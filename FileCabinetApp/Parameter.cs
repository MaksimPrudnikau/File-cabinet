using System;

namespace FileCabinetApp
{
    public struct Parameter : IEquatable<Parameter>
    {
        public Parameter(int id, string firstName, string lastName, string dateOfBirth, string jobExperience, string wage,
            string rank)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            JobExperience = jobExperience;
            Wage = wage;
            Rank = rank;
        }
        
        public int Id { get; set; }
        public string FirstName { get; }
        public string LastName { get; }
        public string DateOfBirth { get; }
        public string JobExperience { get; }
        public string Wage { get; }
        public string Rank { get; }

        public override bool Equals(object obj)
        {
            if (obj is null or not Parameter)
            {
                return false;
            }

            return Equals(obj);
        }

        bool IEquatable<Parameter>.Equals(Parameter source)
        {
            return FirstName == source.FirstName && LastName == source.LastName && DateOfBirth == source.DateOfBirth && JobExperience == source.JobExperience && Wage == source.Wage && Rank == source.Rank;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FirstName, LastName, DateOfBirth, JobExperience, Wage, Rank);
        }


        public static bool operator ==(Parameter left, Parameter right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Parameter left, Parameter right)
        {
            return !(left == right);
        }
    }
}