using Universe.SqlServerJam;

namespace Universe.SqlServer.SystemViews.Tests
{
    internal class SqlServersTestCaseSource
    {
        static Lazy<List<SqlServerRef>> _SqlServers = new Lazy<List<SqlServerRef>>(SqlDiscovery.GetLocalDbAndServerList);

        public static List<SqlServerRef> SqlServers => _SqlServers.Value;
    }
}
