// See https://aka.ms/new-console-template for more information

using Universe.SqlServer.AdministrativeViews.External;

namespace Universe.SqlServer.AdministrativeViews.CLI;

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
            Console.Error.WriteLine($"SQL Server Administrative Views CLI Error{Environment.NewLine}{ex.GetExceptionDigest()}");
            return 42;
        }
    }
}