# SQL Server Queries with metrics and execition plans MCP Server

## Prompt 

**List top 12 heaviest queries on SQL Server '(local)' of any database (or 'database AdventureWorks') ordered by average | cumulative IO reads. Explain execution plan, find potential problems with performance and scalability, suggest resolutions,  and highlight recommendations by engine.**

## Supported sorting 

| Sort option    | Description                                              |
|----------------|----------------------------------------------------------|
| Count          | Number of executions                                     |
| Avg Duration   | Average elapsed time per execution (μs)                  |
| Total Duration | Total elapsed time across all executions (μs) (default)  |
| Avg CPU Time   | Average CPU time / WorkerTime per execution (μs)         |
| Total CPU Time | Total CPU time / WorkerTime across executions (μs)       |
| Avg Reads      | Average logical reads per execution                      |
| Total Reads    | Total reads across all executions                        |
| Avg Writes     | Average writes per execution                             |
| Total Writes   | Total writes across all executions                       |
| Avg Rows       | Average rows returned/affected per execution             |
| Total Rows     | Total rows across all executions                         |
| Avg Memory     | Average memory used per execution (KB)                   |

## AI-Free CLI

This tool is also available for CI on ethemerial build agents as CLI tool:

[https://www.nuget.org/packages/SqlServer.AdministrativeViews](https://www.nuget.org/packages/SqlServer.AdministrativeViews#readme-body-tab)


## Build info
The MCP server is built as a self-contained application and does not require the .NET runtime to be installed on targets:
* win-x64
* win-arm64
* osx-arm64
* osx-x64
* linux-x64
* linux-arm64
* linux-musl-x64

## Developing locally

To test this MCP server from source code (locally) without using a built MCP server package, you can configure your IDE to run the project directly using `dotnet run`.

```json
{
  "servers": {
    "SqlServer.AdministrativeViews.McpServer": {
      "type": "stdio",
      "command": "dotnet",
      "args": [
        "run",
        "--project",
        "<PATH TO PROJECT DIRECTORY>"
      ]
    }
  }
}
```

Refer to the VS Code or Visual Studio documentation for more information on configuring and using MCP servers:

- [Use MCP servers in VS Code (Preview)](https://code.visualstudio.com/docs/copilot/chat/mcp-servers)
- [Use MCP servers in Visual Studio (Preview)](https://learn.microsoft.com/visualstudio/ide/mcp-servers)


## Using the MCP Server from NuGet.org

Once the MCP server package is published to NuGet.org, you can configure it in your preferred IDE. Both VS Code and Visual Studio use the `dnx` command to download and install the MCP server package from NuGet.org.

- **VS Code**: Create a `<WORKSPACE DIRECTORY>/.vscode/mcp.json` file
- **Visual Studio**: Create a `<SOLUTION DIRECTORY>\.mcp.json` file
- **Claude**: [Add local mcp server](https://code.claude.com/docs/en/mcp#option-3-add-a-local-stdio-server)

Example
```
:: Windows
claude mcp add --transport stdio local-sql-servers -- cmd /c dnx SqlServer.AdministrativeViews.McpServer --yes
# Linux and MAC
claude mcp add --transport stdio --env SQLSERVER_WELLKNOWN_1="<Connection String>" local-sql-servers -- dnx SqlServer.AdministrativeViews.McpServer --yes
``

For both VS Code and Visual Studio, the configuration file uses the following server definition:

```json
{
  "servers": {
    "Local SQL Servers": {
      "type": "stdio",
      "command": "dnx",
      "args": [
        "SqlServer.AdministrativeViews.McpServer",
        "--yes"
      ]
    }
  }
}
```

