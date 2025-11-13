using Universe.SqlServer.SystemViews.External;

namespace Universe.SqlServer.SystemViews.Tests
{
    internal class TestEnvironment
    {
        private static Lazy<string> _DumpFolder = new Lazy<string>(() =>
        {
            var raw = Environment.GetEnvironmentVariable("SYSTEM_ARTIFACTSDIRECTORY");
            raw = raw ?? Environment.CurrentDirectory;
            var ret = Path.Combine(Path.GetFullPath(raw), "QueryCacheReports");
            TryAndForget.Execute(() => Directory.CreateDirectory(ret));
            return ret;
        }, LazyThreadSafetyMode.ExecutionAndPublication);

        public static string DumpFolder => _DumpFolder.Value;
    }
}
