Setting up a Jenkins Build agent for Azure Functions (C# v1)
====

Create Jenkins agent vm
----

C# v1 functions only run on .Net Framework, so you need a Windows VM. This doc is written for Windows. V2 functions run on .Net Core, so you can use a Linux build server. The steps are pretty similar.

You can spin up a Windows VM in Azure using the Windows Server 2016 Datacenter image. You will need to install all the tooling specified below. Alternatively, if you use the Windows 10 dev VM, most of the tooling will already be installed.

Install tools
----

You will need to install the following tools on the server.

1. [Java 8 JDK](https://www.oracle.com/technetwork/java/javase/downloads/jdk8-downloads-2133151.html)
2. [Jenkins Build Agent](https://wiki.jenkins.io/display/JENKINS/Installing+Jenkins+as+a+Windows+service)
3. [git](https://git-scm.com/download/win)
4. [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-windows?view=azure-cli-latest)
5. [nuget](https://dist.nuget.org/win-x86-commandline/latest/nuget.exe)
6. [Build tools for VS2017](https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=BuildTools&rel=15)

Ensure that you have the following paths setup after installation is complete, corresponding with the actual locaiton of tools:

* C:\Program Files (x86)\Microsoft SDKs\Azure\CLI2\wbin;
* C:\Program Files (x86)\Common Files\Oracle\Java\javapath;
* C:\Program Files\dotnet\;
* C:\Program Files (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\;
* C:\Program Files\Git\cmd;
* C:\Program Files (x86)\nuget\;
* C:\Program Files\nodejs\;
* C:\Users\jenkinsbuildadmin\AppData\Local\Microsoft\WindowsApps;
* C:\Users\jenkinsbuildadmin\.dotnet\tools;
* C:\Users\jenkinsbuildadmin\AppData\Roaming\npm;
* C:\Program Files (x86)\Microsoft Visual Studio\2017\BuildTools\Common7\IDE\CommonExtensions\Microsoft\TestWindow\

Build script commands
----

Create a pipeline for the echo app located in this [public git repo](https://github.com/hannesne/EchoFunction/tree/master/CSharp/V1)

We will use the `$env:WORKSPACE` environment variable that Jenkins sets in our pipeline to determine the phycisal path of our files, as described [here](https://wiki.jenkins.io/display/JENKINS/Building+a+software+project#Buildingasoftwareproject-below)

You can use the following powershell commands to build this function app:

```powershell
nuget restore $env:WORKSPACE\EchoFunction\CSharp\V1\Echo\Echo.csproj
msbuild.exe "$env:WORKSPACE\EchoFunction\CSharp\V1\Echo\Echo.csproj" /p:OutDir="$env:WORKSPACE\publish" /p:Configuration=Release /t:Publish
```

Also build the tests:

```powershell
nuget restore $env:WORKSPACE\EchoFunction\CSharp\V1\EchoUnitTest\EchoUnitTest.csproj
msbuild.exe $env:WORKSPACE\EchoFunction\CSharp\V1\EchoUnitTest\EchoUnitTest.csproj /p:Platform=AnyCPU
```

Running the tests
----

Run the following command:

```powershell
vstest.console.exe C:\Jenkins\workspace\MyFunctionApp\Services\src\Echo\EchoUnitTests\bin\Debug\EchoUnitTests.dll /Logger:"trx;LogFileName=$env:WORKSPACE\TestResults.trx"
```

Alternatively you can use the [VS Test Runner plugin](https://wiki.jenkins.io/display/JENKINS/VsTestRunner+Plugin)

The TestResults.trx file can be transformed from TRX to a JUnit XML file using the [MS Test plugin](https://wiki.jenkins.io/display/JENKINS/MSTest+Plugin) in your pipeline. The resulting file can be published as part of your build.

Deployment script
----

Create a service principal to do your deployment, as described [here](https://docs.microsoft.com/en-us/cli/azure/create-an-azure-service-principal-azure-cli?view=azure-cli-latest).

You can deploy your app using the [Jenkins Azure Functions plugin](https://wiki.jenkins.io/display/JENKINS/Azure+Function+Plugin)
This plugin will 

1. connect to your functino app's git deployment endpoint
2. clone it to the local machine
3. over write the file contents
4. push the changes back

Alternatively, follow the steps below to script a deployment using the Azure CLI.

Set the following environment variables in your deployment pipeline to store the credentials of you service principal (there's a better way of dealing with credentials described [here](https://jenkins.io/doc/book/pipeline/jenkinsfile/#using-environment-variables).

1. `$env:service_principal_appid`
2. `$env:service_principal_password`
3. `$env:service_principal_tenant`

Create a resource group and a function app, as described [here](https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-function-app-portal)

Set the following evnironment variables

1. `$env:functionapp_resourcegroup`
2. `$env:functionapp_name`

Now use the following commands in your deployment script to perform a deployment:

```powershell
Compress-Archive -Path $env:workspace\publish -DestinationPath $env:workspace\package.zip
az login --service-principal -u $env:service_principal_appid -p $env:service_principal_password --tenant $env:service_principal_tenant
az functionapp deployment source config-zip -g $env:functionapp_resourcegroup -n $env:functionapp_name --src $env:workspace\package.zip
```
