using Cajetan.Infobar.Services.Helpers;
using Serilog;
using Serilog.Events;
using System.Diagnostics;
using System.IO;

namespace Cajetan.Infobar.Config
{
    public static class SerilogConfig
    {
        const string LOG_FILE = "Errors.log";

        public static ILogger Initialize()
        {
            string appDataPath = AppDataHelper.GetAppDataPath();
            string logFilePath = Path.Combine(appDataPath, "Logs", LOG_FILE);

            LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File(
                    path: logFilePath,
                    restrictedToMinimumLevel: LogEventLevel.Warning,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 1048576
                );

            if (Debugger.IsAttached)
                loggerConfiguration.WriteTo.Debug(restrictedToMinimumLevel: LogEventLevel.Verbose);

            return Log.Logger = loggerConfiguration.CreateLogger();
        }
    }
}
