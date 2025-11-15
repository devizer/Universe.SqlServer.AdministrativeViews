# SQL Server Administrative Veiws
MS SQL Server Administrative System Views Visualisation as interactive single-file html report. Supports SQL Server 2005...2025.
This dotnet tool is built for NET 6.0, 8.0, and 10.0 runtime.

## Installation
```dotnet tool install --global SqlServer.AdministrativeViews```

or 

```dotnet tool update --global SqlServer.AdministrativeViews```

## Example
```
SqlServer.AdministrativeViews -o "%SYSTEM_ARTIFACTSDIRECTORY%\\Reports\\{InstanceName}" -all -av
```
## Options
```
-o, --output=VALUE             Optional 'Reports\SQL Server' file name
-av, --append-version          Append SQL Server version to the above file name
-s, --server=VALUE             Specify local or remote SQL Server instance, allow multiple
-cs, --ConnectionString=VALUE  Specify connection string, allow multiple
-all, --all-local-servers      Include all local SQL Servers and all Local DB instances
-h, -?, --help
```

```--all-local-servers```<br/>Include all local sql servers and all Local DB instances. Sql Server Browser service is not required. All instances are discovered by registry and ```SQLLocalDB.exe info``` API.

```-s "(local)\SQLEXPRESS"```<br/>Include local SQLEXPRESS instance.

```-cs "TrustServerCertificate=True;Data Source=127.0.0.1,1433;User ID=sa;Password=p@assw0rd!"```<br/>Include local SQL Server on linux.

Parameters ```-s```, ```-cs``` may be included multiple times.

If multiple SQL Servers are passed an instance name can be inlined into file name as ```{InstanceName}``` placeholder. 



