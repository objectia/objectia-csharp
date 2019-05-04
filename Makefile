.PHONY: test build pack

test:
	dotnet test ./Objectia.Tests/

build:
	dotnet build ./Objectia/

pack:
	dotnet pack ./Objectia/ 

clean:
	dotnet clean ./Objectia/
	dotnet clean ./Objectia.Tests/
