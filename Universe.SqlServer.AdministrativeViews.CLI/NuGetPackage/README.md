# SQL Server Administrative Veiws CLI
The intended use of the CLI tool is to provide **metrics** and **execution plans** for SQL Server queries for Continuous Integration (CI) scenarios as offline interactive report.

It supports SQL Server 2005...2025. This dotnet tool is built for .NET 6.0, 8.0, and 10.0 runtime.

To install and configure SQL Server on Azure DevOps pipeline, Github Actions, etc please take a look at powershell **[SqlServer-Version-Management](https://www.powershellgallery.com/packages/SqlServer-Version-Management)** module. 



# UI Highlights
*  Execution plan link to SSMS per query,
*  Sql syntax highlighter,
*  Light and dark theme support,
*  Metrics sorting,
*  Database filter,
*  Columns chooser.


## Installation
```dotnet tool install --global SqlServer.AdministrativeViews```

or 

```dotnet tool update --global SqlServer.AdministrativeViews```

## Example
```
SqlServer.AdministrativeViews -o "%SYSTEM_ARTIFACTSDIRECTORY%\Reports\{InstanceName}" -all -av
```

## Options

```-o "Reports\{InstanceName}"```  ⇢  Write a report to a file named as sql server or local db instance in the relative folder Report. {InstanceName} placeholder is useful if multiple SQL Servers are passed. Full path is also allowed. Missing folders will be created.

```--append-version```  ⇢  Append the instance version to the above file(s) name.

```--all-local-servers```  ⇢  Include all local sql servers and all Local DB instances. Sql Server Browser service is not required. All instances are discovered by registry and SQL Local DB management API.

```-s "(local)\SQLEXPRESS"```  ⇢  Include local SQLEXPRESS instance.

```-cs "TrustServerCertificate=True;Data Source=127.0.0.1,1433;User ID=sa;Password=p@assw0rd!"```  ⇢  Include local SQL Server on Linux, on a network, or in the cloud.

Parameters ```-s``` (server instance), ```-cs``` (connection string) may be included multiple times.

```
-o, --output=VALUE             Optional 'Reports\SQL Server' file name
-av, --append-version          Append SQL Server version to the above file name
-s, --server=VALUE             Specify local or remote SQL Server instance, allow multiple
-cs, --ConnectionString=VALUE  Specify connection string, allow multiple
-all, --all-local-servers      Include all local SQL Servers and all Local DB instances
-h, -?, --help
```
