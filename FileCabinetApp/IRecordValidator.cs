namespace FileCabinetApp
{
    public interface IRecordValidator
    {
        public void NameValidator(string name);

        public void DateOfBirthValidator(string source);
    }
}