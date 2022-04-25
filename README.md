## **Interactive Coding Test**
A program that tasks the user with solving a set of coding problems designed to test their level of expertise with C#.

## **Usage**

### Running the program
For now, the best way to run the program is to juse use `dotnet run`. Since the program creates some files in the current directory, the best way to run it would be to navigate to an empty folder and then use `dotnet run --project project_path`, where `project_path` is the path to `DynamicCheck.csproj`.

### Arguments
By default the program will simply start the test. The flag `-p` will prompt an administator to enter a validation password at the beginning of the test, which will then be used to sign the test's results. The `-v` will also prompt an admin for the password, which will then be used to validate the result's signature. 