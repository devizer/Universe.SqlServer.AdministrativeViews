#if NET35 || true
using System.Data.Common;
using Dapper;

namespace Universe.SqlServer.AdministrativeViews.SqlDataAccess;

public static class DapperExtensionsLevel2
{
    
    public static IEnumerable<T> Query<T>(this DbConnection dbConnection, string sql, object pa, int? commandTimeout = null)
    {
        return dbConnection.Query<T>(sql, pa, null, false, commandTimeout, null);
    }

}
#endif