using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Universe.SqlServer.AdministrativeViews.McpServer.Tools;

var builder = Host.CreateApplicationBuilder(args);



// Does not work
Environment.SetEnvironmentVariable("NO_COLOR", "1");

// Configure all logs to go to stderr (stdout is used for the MCP protocol messages).
builder.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace);

// Add the MCP services: the transport to use (stdio) and the tools to register.
builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<SqlServerAdministrativeViewsMcpServer>();

IReadOnlyCollection<RemoteSqlServerArgument> remoteSqlServerArguments = RemoteSqlServerArgumentParser.Parse(args);
builder.Services.AddSingleton<IReadOnlyCollection<RemoteSqlServerArgument>>(provider => remoteSqlServerArguments);


await builder.Build().RunAsync();