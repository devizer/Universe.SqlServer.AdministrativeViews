using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace Universe.SqlServer.AdministrativeViews.McpServer.Tools
{
    internal class DebuggerLog
    {
        public static volatile string AppName;
        public string ActionTitle { get; private set; }
        static string LogFolder => _LogFolder.Value;
        private static int _CounterStorage;
        private static DateTime _AppStartedAt;
        private int Counter;

        static DebuggerLog()
        {
            _AppStartedAt = DateTime.Now;
        }

        public DebuggerLog(string actionTitle)
        {
            ActionTitle = actionTitle;
            Counter = Interlocked.Increment(ref Counter);
        }

        // [Conditional("DEBUG")]
        public void AddJsonLogArtifact(string artifactName, object content)
        {
            string json = JsonConvert.SerializeObject(content, SerializerSettings);
            AddLogArtifact(artifactName, json);
        }

        // [Conditional("DEBUG")]
        public void AddLogArtifact(string artifactName, string content)
        {
            var fullName = Path.Combine(LogFolder, _AppStartedAt.ToString("yyyy-MM-dd HH꞉mm꞉ss"), $"{Counter:000} " + ActionTitle + " " + artifactName);
            CreateDirectoryIfNotExists(Path.GetDirectoryName(fullName));

            using FileStream fs = new FileStream(fullName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            using StreamWriter wr = new StreamWriter(fs, new UTF8Encoding(false));
            wr.WriteLine(content);
        }

        private static Lazy<string> _LogFolder = new(() =>
        {
            string tracesFolder = GetTracesRoot();
            var ret = Path.Combine(tracesFolder, $"{AppName}.Traces");
            CreateDirectoryIfNotExists(ret);
            Console.Error.WriteLine($"{nameof(DebuggerLog)} folder is {ret}");
            return ret;
        });

        static string GetTracesRoot()
        {
            var assemblyFullPath = Assembly.GetExecutingAssembly()?.Location;

            string tracesFolder = null;
            if (!string.IsNullOrEmpty(assemblyFullPath))
            {
                tracesFolder = Path.GetDirectoryName(Path.GetFullPath(assemblyFullPath));
                return tracesFolder;
            }

            if (CrossInfo.ThePlatform == CrossInfo.Platform.Windows)
            {
                var localAppData = Environment.GetEnvironmentVariable("LOCALAPPDATA");
                if (!string.IsNullOrEmpty(localAppData) && Directory.Exists(localAppData)) return localAppData;
                var appData = Environment.GetEnvironmentVariable("APPDATA");
                if (!string.IsNullOrEmpty(appData) && Directory.Exists(appData)) return appData;
            }
            else
            {
                var home = Environment.GetEnvironmentVariable("HOME");
                if (!string.IsNullOrEmpty(home) && Directory.Exists(home)) return home;
            }

            return Path.DirectorySeparatorChar + "tmp";
        }

        private static string CreateDirectoryIfNotExists(string folder)
        {
            try
            {
                Directory.CreateDirectory(folder);
            }
            catch
            {
            }

            return folder;
        }

        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Include,
            MaxDepth = 128,
            Formatting = Formatting.Indented,
        };


    }
}
