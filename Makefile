.PHONY: test build pack

test:
	dotnet test ./Objectia.Tests/

build:
	dotnet build ./Objectia/

pack:
	dotnet pack -c Release ./Objectia/ 

clean:
	dotnet clean ./Objectia/
	dotnet clean ./Objectia.Tests/
