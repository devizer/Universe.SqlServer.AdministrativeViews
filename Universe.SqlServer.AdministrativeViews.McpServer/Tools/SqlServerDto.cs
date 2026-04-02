using System.ComponentModel;

namespace Universe.SqlServer.AdministrativeViews.McpServer.Tools;

public class SqlServerDto
{
    [Description(@"Type of SQL Server. Local SQL Server, Local DB, or Well Known SQL Server in the cloud, or on the network, or in a container.
Possible values:
  Local: Local SQL server instance
  LocalDB: Local DB instance
  WellKnown: SQL Server in the cloud, or on the network, or in a container")]
    public string Kind { get; set; }
    
    [Description("Version of Installer")]
    public Version InstallerVersion { get; set; }

    [Description("Version of SQL Server, including comulative update or service pack, and Edition")]
    public string Version { get; set; }
    
    [Description("SQL Server or Local DB Instance (a value of Data Source parameter of connection string)")]
    public string ServerInstance { get; set; }
}