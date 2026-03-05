using Universe.SqlServer.AdministrativeViews.SqlDataAccess;

namespace Universe.SqlServer.AdministrativeViews.Exporter;

public interface ISqlCacheHtmlExporterPredicate
{
    bool ShouldExport(string server, QueryCacheRow row);
}

public static class ISqlCacheHtmlExporterPredicateExtensions
{
    public static ISqlCacheHtmlExporterPredicate And(this IEnumerable<ISqlCacheHtmlExporterPredicate> predicates)
    {
        return new AndPredicate(predicates);
    }
    private class AndPredicate : ISqlCacheHtmlExporterPredicate
    {
        private IEnumerable<ISqlCacheHtmlExporterPredicate> predicates;

        public AndPredicate(IEnumerable<ISqlCacheHtmlExporterPredicate> predicates)
        {
            if (predicates == null) throw new ArgumentNullException(nameof(predicates));
            this.predicates = predicates;
        }

        public bool ShouldExport(string server, QueryCacheRow row)
        {
            var ret = true;
            foreach (var predicate in predicates)
            {
                if (predicate != null && !predicate.ShouldExport(server, row))
                {
                    ret = false;
                    break;
                }
            }
            return ret;
        }
    }
}

public class AlwaysExportPredicate : ISqlCacheHtmlExporterPredicate
{
    public bool ShouldExport(string server, QueryCacheRow row) => true;
}

public class ExportNonMasterQueriesPredicate : ISqlCacheHtmlExporterPredicate
{
    public bool ShouldExport(string server, QueryCacheRow row)
    {
        return row.DatabaseName != "master";
    }
}

public class ExportNonSystemQueriesPredicate : ISqlCacheHtmlExporterPredicate
{
    public bool ShouldExport(string server, QueryCacheRow row)
    {
        var sqlCode = row.SqlStatement;
        if (sqlCode == null) return true;
        if (sqlCode == SqlQueryObjectsReader.SqlQuery) return false;
        if (sqlCode == SqlIndexStatsReader.SqlSelectIndexes) return false;
        const string q1 = "if exists (select * from sys.objects where (is_published = 1 or is_schema_published = 1  ))";
        if (sqlCode.StartsWith(q1, StringComparison.OrdinalIgnoreCase)) return false;
        const string q2 = "WHEN CAST(ISNULL(bset.compressed_backup_size, 0) / 1048576 AS FLOAT) BETWEEN";
        if (sqlCode.IndexOf(q2, StringComparison.OrdinalIgnoreCase) >= 0) return false;

        const string q3a = "AND HAS_DBACCESS(@db_curr) = 1";
        const string q3b = "AND not exists(select * from sys.databases where db_id(@db_curr) = database_id and is_cdc_enabled = 1)";
        if (sqlCode.IndexOf(q3a, StringComparison.InvariantCultureIgnoreCase) >= 0
            && sqlCode.IndexOf(q3b, StringComparison.InvariantCultureIgnoreCase) >= 0) return false;

        const string q4 = "if (select value_in_use from sys.configurations where configuration_id =";
        if (sqlCode.IndexOf(q4, StringComparison.OrdinalIgnoreCase) >= 0) return false;

        const string q5a = "db_id() AS [database_id]";
        const string q5b = "db_id() AS database_id";
        const string q5c = "db_id() [database_id]";
        const string q5d = "db_id() database_id";
        if (sqlCode.IndexOf("\"", StringComparison.OrdinalIgnoreCase) < 0
            && sqlCode.IndexOf("sp_executesql", StringComparison.OrdinalIgnoreCase) < 0
            && sqlCode.IndexOf("SELECT", StringComparison.InvariantCultureIgnoreCase) >= 0
            &&
            (sqlCode.IndexOf(q5a, StringComparison.OrdinalIgnoreCase) >= 0
             || sqlCode.IndexOf(q5b, StringComparison.OrdinalIgnoreCase) >= 0
             || sqlCode.IndexOf(q5c, StringComparison.OrdinalIgnoreCase) >= 0
             || sqlCode.IndexOf(q5d, StringComparison.OrdinalIgnoreCase) >= 0)) return false;

        return true;
    }
}
