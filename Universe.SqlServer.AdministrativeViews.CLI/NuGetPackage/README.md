# SQL Server Administrative Veiws CLI
* Visualizes SQL Server Administrative Views for **queries**, **metrics**, and **execution plans**.
* Generates a single-file **offline** interactive HTML report for one or multiple SQL Servers.
* Designed for Continuous Integration (CI) scenarios.
* Supports SQL Server 2005...2025 including Local DB.

# UI Highlights
*  One-click link to execution plan diagram per query,
*  Sql syntax highlighter,
*  Light and dark theme support,
*  Metrics sorting,
*  Database filter,
*  Column chooser.

## [Demo🔗](https://devizer.github.io/SQL-Server-Performance-by-System-Administrative-Views-Demo/)


## Installation
```
dotnet tool install --global SqlServer.AdministrativeViews
```

## Example
```
$report = "$($ENV:SYSTEM_ARTIFACTSDIRECTORY)\Reports\{InstanceName} {Version} on {Platform}"
SqlServer.AdministrativeViews -o $report -all

# or 

$report = "$($ENV:GITHUB_TEMP)\Reports\{InstanceName} {Version} on {Platform}"
dotnet dnx SqlServer.AdministrativeViews -- -o $report -all
```

GITHUB_TEMP and SYSTEM_ARTIFACTSDIRECTORY are directories that are emptied at the beginning and end of each job on github actions workfkow and azure devops pipeline.

## Options

**```-o "Reports\{InstanceName}"```**  ⇢ 
Write a report to a file named as sql server or local db instance in the relative folder Report. Full path is also allowed. Missing folders will be created. **{InstanceName}** placeholder is useful if multiple SQL Servers are passed. Additional file name placeholders are **{Version}** and **{Platform}**.

**```--all-local-servers```**, **`-all`**  ⇢ 
Include all local sql servers and all Local DB instances. Sql Server Browser service is not required. All instances are discovered by registry and SQL Local DB management API.

**```-s "(local)\SQLEXPRESS"```**  ⇢ 
Include local SQLEXPRESS instance.

**```-cs "TrustServerCertificate=True;Data Source=127.0.0.1,1433;User ID=sa;Password=p@assw0rd!"```**  ⇢ 
Include SQL Server on Linux, on a network, or in the cloud.

Parameters ```-s``` (server instance name), ```-cs``` (connection string) may be included multiple times.


## Build Notes
This dotnet tool is built for .NET 6.0, 8.0, and 10.0 SDK.

Standalone cli tool is also available without SDK as .NET Core and .NET Framework builds for Windows, Linux, and macOS: **[github releases](https://github.com/devizer/Universe.SqlServer.AdministrativeViews/releases)** 

## See Also
To install and configure SQL Server on Azure DevOps pipeline, Github Actions workflow, etc please take a look at powershell **[SqlServer-Version-Management](https://www.powershellgallery.com/packages/SqlServer-Version-Management)** module. 
