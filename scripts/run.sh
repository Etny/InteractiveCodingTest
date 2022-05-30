#! /bin/bash

SCRIPTPATH="$( cd -- "$(dirname "$0")" >/dev/null 2>&1 ; pwd -P )"
WORKSPACE="../workspace"
PROJECT_FILE="../src/DynamicCheck.csproj"

cd $SCRIPTPATH

mkdir -p "$WORKSPACE" || (echo Failed to create workspace folder ; exit)

if [ ! -d "$WORKSPACE" ]; then 
	echo Failed to find workspace folder
	exit
fi

cd "$WORKSPACE"

if [ ! -f "$PROJECT_FILE" ]; then 
	echo Failed to find test program project file \(\'src/DynamicCheck.csproj\'\). Did you move it?
	exit
fi

dotnet run -c release --project "$PROJECT_FILE"

