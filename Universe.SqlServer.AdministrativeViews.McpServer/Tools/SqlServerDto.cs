using System.ComponentModel;

namespace Universe.SqlServer.AdministrativeViews.McpServer.Tools;

public class SqlServerDto
{
    [Description("SQL Server or Local DB Instance (a value of Data Source parameter of connection string). This value should be passed as 'sql_server_or_localdb_instance' parameter into 'Get_Queries_With_Execution_Plan_and_Metrics' method")]
    public string ServerInstance { get; set; }


    [Description("Recommended for Remote. It can be assigned using command line. For example --alias Everest --connection-string 'Data Source = ...; User ID = ...''")]
    public string Alias { get; set; }

    [Description("Version of SQL Server including Edition, cumulative update or service pack. If SQL Server is online and healthy it is a main version source of truth. Actual SQL Server Version is inlined as Major.Minor.Build.Revision into this property.")]
    public string Version { get; set; }

    [Description(@"Type of SQL Server. Local SQL Server, Local DB, or Well Known SQL Server in the cloud, or on the network, or in a container.
Possible values:
  Local: Local SQL server instance (express, advanced, developer, enterprise)
  LocalDB: Local DB instance (localdb)
  Remote: SQL Server in the cloud, or on the network, or in a container.")]
    public string Kind { get; set; }
    
    [Description("Version of SQL Installer. It may differ from actual 'Version' property if Service Pack or Cumulative update was applied later after SQL Server was installed. Thus 'InstallerVersion' is provided for troubleshooting only and it should not be visualized without explicit request for 'Get_Online_Sql_Servers' method.")]
    public Version InstallerVersion { get; set; }
}