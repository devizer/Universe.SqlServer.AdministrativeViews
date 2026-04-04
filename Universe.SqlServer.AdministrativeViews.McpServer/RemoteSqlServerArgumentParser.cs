using System.Collections.Immutable;
using Universe.SqlServer.AdministrativeViews.CLI.External;

public static class RemoteSqlServerArgumentParser {
    public static IReadOnlyCollection<RemoteSqlServerArgument> Parse(string[] args2)
    {
        List<RemoteSqlServerArgument> ret = new List<RemoteSqlServerArgument>();

        string alias = null;

        void AddConnectionString(string connectionString)
        {
            RemoteSqlServerArgument next = new RemoteSqlServerArgument(alias, connectionString);
            alias = null;
            ret.Add(next);
        }

        OptionSet p = new OptionSet()
            .Add("a=|alias=", "Alias of following connection string", v => alias = v)
            .Add("cs=|connection-string=", "Add remote SQL Server, allow multiple", v => AddConnectionString(v));

        List<string> extra = p.Parse(args2);

        return ret.ToImmutableList();
    }
}