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

        [Conditional("DEBUG")]
        public void AddJsonLogArtifact(string artifactName, object content)
        {
            string json = JsonConvert.SerializeObject(content, SerializerSettings);
            AddLogArtifact(artifactName, json);
        }

        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Include,
            MaxDepth = 128,
            Formatting = Formatting.Indented,
        };

        [Conditional("DEBUG")]
        public void AddLogArtifact(string artifactName, string content)
        {
            var fullName = Path.Combine(LogFolder, _AppStartedAt.ToString("yyyy-MM-dd HH꞉mm꞉ss") + " " + ActionTitle + " " + artifactName);
            CreateDirectoryIfNotExists(Path.GetDirectoryName(fullName));

            using FileStream fs = new FileStream(fullName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            using StreamWriter wr = new StreamWriter(fs, new UTF8Encoding(false));
            wr.WriteLine(content);
        }

        private static Lazy<string> _LogFolder = new(() =>
        {
            var pathBinaries = Path.GetDirectoryName(Path.GetFullPath(Assembly.GetExecutingAssembly().Location));
            var ret = Path.Combine(pathBinaries, $"{AppName}.Traces");
            CreateDirectoryIfNotExists(ret);
            Console.Error.WriteLine($"{nameof(DebuggerLog)} folder is {ret}");
            return ret;
        });

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
    }
}
