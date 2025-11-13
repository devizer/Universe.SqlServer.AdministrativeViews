using Universe.SqlServer.SystemViews.Exporter;
using Universe.SqlServer.SystemViews.External;

namespace Universe.SqlServer.SystemViews.Tests;

public class TestCustomSummaryProperties
{
    [Test]
    public void ShowCustomProperties()
    {
        var customSummary = CustomSummaryRowReader.GetCustomSummary();
        Console.WriteLine($"CUSTOM SUMMARY:{customSummary.ToJsonString()}");
        /*
        var allEnvVars = Environment.GetEnvironmentVariables().Keys
            .OfType<object>()
            .Select(x => Convert.ToString(x))
            .OrderBy(x => x)
            .Select(x => new { Var = x, Val = Environment.GetEnvironmentVariable(x) })
            .ToArray();

        Console.WriteLine($"All Env Vars:{allEnvVars.ToJsonString()}");
        */

    }
}