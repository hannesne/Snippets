# Powershell utilities for downloading dependency trees for nuget packages

Nuget CLI Reference available [here](https://docs.microsoft.com/en-us/nuget/tools/nuget-exe-cli-reference).


First, download nuget cli tool to your working directory. Alternatively  setup a path environment variable to make it accessible from where you're executing your command.

    wget https://dist.nuget.org/win-x86-commandline/latest/nuget.exe -OutFile nuget.exe

Now use nuget utlity to download your package. The [install](https://docs.microsoft.com/en-us/nuget/tools/cli-ref-install) command will download all the dependencies. In this case I'm downloading the .Net Standard 2.0 client libraries of the Microsoft.AspNetCore.SignalR.Client package, and it's dependencies.

    .\nuget.exe install Microsoft.AspNetCore.SignalR.Client -Framework netstandard2.0

Now copy all the relevant dll's to the local folder. For some reason, dependencies are downloaded for all frameworks, ignoring the framework I specified. Don't know why. Also, some dependencies may have two dll's with the same name. One in a ref folder, to be used as a platform independent metadata reference when coding, and another in a lib folder, which actually contains the implementation for a given cpu architecture. Discussion [here](https://stackoverflow.com/questions/34611991/how-to-package-a-multi-architecture-net-library-that-targets-the-universal-wind). 

    Get-ChildItem -Path .\ -Filter *.dll -Recurse -File | Where { $_.Directory -like "*lib\netstandard2.0*" } | ForEach-Object { copy $_.FullName .}
