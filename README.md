#  File Cabinet

Консольное приложение, которое принимает команды пользователя и управляет пользовательскими данными.

### Step 5 - Refactoring. Таблица связей связей is-a и has-a:
| Class 1                    | Class 2             | Relationship |
|----------------------------|---------------------|--------------|
| FileCabinetService         | IRecordValidator    | has-a        |
| FileCabinetDefaultService  | IFileCabinetService | is-a         |
| FileCabinetCustomService   | IFileCabinetService | is-a         |
| FileCabinetDefaultService  | DefaultValidator    | has-a        |
| FileCabinetCustomService   | CustomValidator     | has-a        |
| DefaultValidator           | IRecordValidator    | is-a         |
| CustomValidator            | IRecordValidator    | is-a         |
