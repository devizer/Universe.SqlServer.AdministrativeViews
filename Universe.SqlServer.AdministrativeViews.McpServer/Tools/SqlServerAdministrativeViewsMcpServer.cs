using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data.SqlClient;
using Universe.SqlServer.AdministrativeViews.Exporter;
using Universe.SqlServer.AdministrativeViews.External;
using Universe.SqlServer.AdministrativeViews.SqlDataAccess;
using Universe.SqlServerJam;

namespace Universe.SqlServer.AdministrativeViews.McpServer.Tools;

/// <summary>
/// The XML Summary for SQL Server Administrative Views Mcp Server
/// </summary>
[Description("Provides access to queries, metrics and execution plans of SQL Servers and SQL Local DB instances.")]
internal class SqlServerAdministrativeViewsMcpServer
{
    private ILogger<SqlServerAdministrativeViewsMcpServer> _Logger;

    static SqlServerAdministrativeViewsMcpServer()
    {
        DebuggerLog.AppName = "SqlServer.AdministrativeViews";
    }

    public SqlServerAdministrativeViewsMcpServer(ILogger<SqlServerAdministrativeViewsMcpServer> logger)
    {
        _Logger = logger;
    }

    [McpServerTool]
    [Description(@"Get queries with execution plan and metrics for specified SQL Server or LocalDB instance.
Duration and CPU Time are measured in microseconds.
Columns of CPU Time are suffixed with WorkerTime for result collection, but corresponding sortBy parameter is either 'Avg CPU Time' or 'Total CPU Time'.")]
    public List<QueryCacheRow> Get_Queries_With_Execution_Plan_and_Metrics(
        [Description("Should be used as parameter value of the ServerInstance property returned by Get_Online_Sql_Servers. For example '(local)', or '(local)\\SQLEXPRESS'")]
        string sql_server_or_localdb_instance,
        [Description("Optional database name, if omitted get ordered queries of any database")]
        string optional_database,
        [Description("Amount of queries to return, default is 12")]
        int topN = 12,
        [Description("Sorting column: Count|Avg Duration|Total Duration|Avg CPU Time|Total CPU Time|Avg Reads|Total Reads|Avg Writes|Total Writes|Avg Rows|Total Rows|Avg Memory")]
        string sortBy = "Total Duration"
        )
    {
        DebuggerLog debuggerLog = new DebuggerLog("Get_Queries_With_Execution_Plan_and_Metrics");
        debuggerLog.AddJsonLogArtifact("Arguments", new
        {
            sql_server_or_localdb_instance,
            optional_database,
            topN,
            sortBy
        });

        SqlServerRef[] onlineSqlServers = GetOnlineSqlServerReferences();
        var found = onlineSqlServers
            .Where(x => x.DataSource?.Equals(sql_server_or_localdb_instance, StringComparison.CurrentCultureIgnoreCase) == true)
            .FirstOrDefault();

        if (found == null)
            throw new ArgumentException($@"Specified SQL Server '{sql_server_or_localdb_instance}' is missing, pr is not running, or is not accessible.
Currently {onlineSqlServers.Count()} online SQL Servers available: {string.Join(", ", onlineSqlServers.Select(x => $"'{x.DataSource}'"))}");

        var dbProvider = SqlClientFactory.Instance;
        var connectionString = found.ConnectionString;
        QueryCacheReader reader = new QueryCacheReader(SqlClientFactory.Instance, connectionString);
        var b = dbProvider.CreateConnectionStringBuilder();
        b.ConnectionString = connectionString;
        var server = b["Data Source"]?.ToString();
        
        ExportNonMasterQueriesPredicate nonMasterPredicate = new ExportNonMasterQueriesPredicate();
        ExportNonSystemQueriesPredicate nonSystemQueryPredicate = new ExportNonSystemQueriesPredicate();
        var predicate = new ISqlCacheHtmlExporterPredicate[] { nonMasterPredicate, nonSystemQueryPredicate }.And();
        var ret = reader.Read().Where(row => predicate == null || predicate.ShouldExport(server, row)).ToList();
        var columnsSchema = reader.ColumnsSchema;
        IEnumerable<QueryCacheRow> filteredResult = ret;
        if (!string.IsNullOrEmpty(optional_database))
        {
            filteredResult = filteredResult.Where(x => x.DatabaseName?.Equals(optional_database, StringComparison.CurrentCultureIgnoreCase) == true);
        }

        sortBy ??= "Avg Duration";

        bool IsSortBy(params string[] sortKeys)
        {
            return sortKeys.Any(x => sortBy.Equals(x, StringComparison.CurrentCultureIgnoreCase));
        }

        if (IsSortBy("Avg Duration"))
            filteredResult = filteredResult.OrderByDescending(x => x.AvgElapsedTime);
        else if (IsSortBy("Total Duration"))
            filteredResult = filteredResult.OrderByDescending(x => x.TotalElapsedTime);
        else if (IsSortBy("Avg CPU Time"))
            filteredResult = filteredResult.OrderByDescending(x => x.AvgWorkerTime);
        else if (IsSortBy("Total CPU Time"))
            filteredResult = filteredResult.OrderByDescending(x => x.TotalWorkerTime);
        else if (IsSortBy("Count"))
            filteredResult = filteredResult.OrderByDescending(x => x.ExecutionCount);
        else if (IsSortBy("Avg Reads"))
            filteredResult = filteredResult.OrderByDescending(x => x.AvgLogicalReads);
        else if (IsSortBy("Total Reads"))
            filteredResult = filteredResult.OrderByDescending(x => x.TotalLogicalReads);
        else if (IsSortBy("Avg Writes"))
            filteredResult = filteredResult.OrderByDescending(x => x.AvgLogicalWrites);
        else if (IsSortBy("Total Writes"))
            filteredResult = filteredResult.OrderByDescending(x => x.TotalLogicalWrites);

        else if (IsSortBy("Avg Rows"))
            filteredResult =
                columnsSchema.HasRows
                    ? filteredResult.OrderByDescending(x => x.AvgRows)
                    : throw new Exception($"The version of SQL Server '{sql_server_or_localdb_instance}' does not support the Rows metric");

        else if (IsSortBy("Total Rows"))
            filteredResult =
                columnsSchema.HasRows
                    ? filteredResult.OrderByDescending(x => x.TotalRows)
                    : throw new Exception($"The version of SQL Server '{sql_server_or_localdb_instance}' does not support the Rows metric");

        else if (IsSortBy("Memory", "Avg Memory"))
            filteredResult =
                columnsSchema.HasUsedGrantKb
                    ? filteredResult.OrderByDescending(x => x.AvgUsedGrantKb)
                    : throw new Exception($"The version of SQL Server '{sql_server_or_localdb_instance}' does not support the Memory metric");

        var result = filteredResult.Take(topN).ToList();
        debuggerLog.AddJsonLogArtifact("Result", result);
        return result;
    }


    [McpServerTool]
    [Description(@"Get List of Online Local SQL Servers and Local DB Servers.
If SQL Server is not running it is not returned. SQL Browser Service is not invoked by this tool, because local registry is only source of SQL Servers.
Azure SQL, SQL Server on the network or in a container can be added using environment variable SQLSERVER_WELLKNOWN_***.")]
    public List<SqlServerDto> Get_Online_Sql_Servers(
        [Description("Timeout to validate connectivity of SQL Server instance in seconds")]
        int timeoutSeconds = 30
    )
    {
        timeoutSeconds = Math.Max(1, timeoutSeconds);
        DebuggerLog debuggerLog = new DebuggerLog("Get_Online_Sql_Servers");
        debuggerLog.AddJsonLogArtifact("Arguments", new { timeoutSeconds });

        var onlineServers = GetOnlineSqlServerReferences();
        debuggerLog.AddJsonLogArtifact("Online Server Refs", new { onlineServers });

        ConcurrentBag<SqlServerDto> ret = new ConcurrentBag<SqlServerDto>();
        Parallel.ForEach(onlineServers, sqlRef =>
        {
            var man = sqlRef.CreateConnection().Manage();
            Version shortServerVersion = null;
            try
            {
                shortServerVersion = man.GetShortServerVersion(timeoutSeconds);
            }
            catch (Exception ex)
            {
                _Logger.LogWarning($"SQL Server is expected online, but it does not respond: [{sqlRef}]. {ex.GetExceptionDigest()}");
            }

            if (shortServerVersion != null)
            {
                var mediumVersion = man.ServerTitle;
                ret.Add(new SqlServerDto()
                {
                    InstallerVersion = sqlRef.InstallerVersion,
                    Kind = sqlRef.Kind.ToString().Replace("LocalDB", "LocalDB"),
                    ServerInstance = sqlRef.DataSource,
                    Version = mediumVersion
                });
            }
        });

        var materializedRet = ret.ToList().OrderByDescending(x => x.Version).ThenByDescending(x => x.Version).ToList();
        debuggerLog.AddJsonLogArtifact("Result", materializedRet);
        return materializedRet;
    }

    private static SqlServerRef[] GetOnlineSqlServerReferences()
    {
        List<SqlServerRef>? servers = SqlDiscovery.GetLocalDbAndServerList();
        var localServers = servers
            .Where(x => x.ServiceStartup != LocalServiceStartup.Disabled)
            .ToArray();


        var onlineServers = localServers
            // Service should be running
            .Where(x => x.Kind == SqlServerDiscoverySource.WellKnown || x.ToSqlServerDataSource().IsLocalDb || x.ToSqlServerDataSource().CheckLocalServiceStatus()?.State == SqlServiceStatus.ServiceState.Running)
            .ToArray();

        return onlineServers;
    }
}