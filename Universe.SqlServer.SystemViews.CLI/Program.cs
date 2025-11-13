// See https://aka.ms/new-console-template for more information

using Universe.SqlServer.SystemViews.External;

namespace Universe.SqlServer.SystemViews.CLI;

public class Program
{
    public static int Main(string[] args)
    {
        try
        {
            return MainProgram.Run(args);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SQL Server Query Cache CLI Error{Environment.NewLine}{ex.GetExceptionDigest()}");
            return 42;
        }
    }
}