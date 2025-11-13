using Universe.SqlServer.SystemViews.External;

namespace Universe.SqlServer.SystemViews.Exporter;

partial class SqlCacheHtmlExporter
{
    private StepsLogger _StepsLogger;

    public SqlCacheHtmlExporter()
    {
        StepsLogger.TakeOwnership();
        _StepsLogger = StepsLogger.Instance;
    }

    StepsLogger.MeasureStepImplementation LogStep(string title)
    {
        return _StepsLogger?.LogStep(title);
    }

    public string GetLogsAsString()
    {
        return _StepsLogger?.GetLogsAsString();
    }

}