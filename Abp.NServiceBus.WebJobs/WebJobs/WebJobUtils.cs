using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Abp.NServiceBus.WebJobs
{
    public static class WebJobUtils
    {
        private static bool _running = true;
        private static string _shutdownFile;

        public static void RunAndWait(TextWriter log)
        {
            if (!Debugger.IsAttached)
            {
                // Get the shutdown file path from the environment
                _shutdownFile = Environment.GetEnvironmentVariable("WEBJOBS_SHUTDOWN_FILE");

                // Setup a file system watcher on that file's directory to know when the file is created
                var fileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(_shutdownFile));
                fileSystemWatcher.Created += OnChanged;
                fileSystemWatcher.Changed += OnChanged;
                fileSystemWatcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.LastWrite;
                fileSystemWatcher.IncludeSubdirectories = false;
                fileSystemWatcher.EnableRaisingEvents = true;
            }

            // Run as long as we didn't get a shutdown notification
            while (_running)
            {
                // Here is my actual work
                log.WriteLine("WebJobUtils: Running and waiting " + DateTime.UtcNow);
                Thread.Sleep(3000);
            }

            log.WriteLine("WebJobUtils: Stopped " + DateTime.UtcNow);
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath.IndexOf(Path.GetFileName(_shutdownFile), StringComparison.OrdinalIgnoreCase) >= 0)
            {
                // Found the file mark this WebJob as finished
                _running = false;
            }
        }
    }
}
