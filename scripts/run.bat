@echo off

SET SCRIPTPATH=%~dp0
SET WORKSPACE=..\workspace
SET PROJECT_FILE=..\src\DynamicCheck.csproj

cd %SCRIPTPATH%

mkdir %WORKSPACE%

cd %WORKSPACE%

dotnet run -c release --project %PROJECT_FILE%