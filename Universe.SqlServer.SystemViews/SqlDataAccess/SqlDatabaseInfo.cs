namespace Universe.SqlServer.SystemViews.SqlDataAccess
{
    public class SqlDatabaseInfo
    {
        private static readonly string[] SystemDbNames = new string[] { "msdb", "master", "model", "resource" };
        public static bool IsSystemDatabase(string name)
        {
            var lowerName = name?.ToLower();
            return SystemDbNames.Any(x => x == lowerName);
        }
    }
}
