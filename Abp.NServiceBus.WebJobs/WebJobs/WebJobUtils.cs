using Abp.Dependency;
using Castle.Core.Logging;
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
    public class WebJobUtils : ISingletonDependency
    {
        private static bool _running = true;
        private static string _shutdownFile;

        public ILogger Logger { get; set; }

        private readonly AbpBootstrapper _bootstrapper;

        public WebJobUtils(AbpBootstrapper bootstrapper)
        {
            Logger = NullLogger.Instance;
            _bootstrapper = bootstrapper;
        }

        public void RunAndWait()
        {
            if (!Debugger.IsAttached)
            {
                Logger.Debug("WebJobUtils: Running in production environment");

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
                Logger.DebugFormat("WebJobUtils: Running and waiting {0}", DateTime.UtcNow);
                Thread.Sleep(3000);
            }

            Logger.InfoFormat("WebJobUtils: Stop requested {0}", DateTime.UtcNow);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            Logger.InfoFormat("WebJob Directory changed detected, looking for shutdown file...");

            if (e.FullPath.IndexOf(Path.GetFileName(_shutdownFile), StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Logger.InfoFormat("Shutdown file detected, stopping...");

                // Found the file mark this WebJob as finished
                _running = false;
            }

            Logger.InfoFormat("Shutdown file not detected, WebJob still running");
        }
    }
}
