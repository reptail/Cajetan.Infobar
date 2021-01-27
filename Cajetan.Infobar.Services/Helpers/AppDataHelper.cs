using System;
using System.IO;
using System.Reflection;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Cajetan.Infobar")]
namespace Cajetan.Infobar.Services.Helpers
{
    internal static class AppDataHelper
    {
        public static string GetAppDataPath()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string assemblyName = GetAssembly().GetName().Name;
            return Path.Combine(appDataPath, assemblyName);

        }

        public static string GetAssemblyPath()
        {
            Assembly assembly = GetAssembly();
            return Path.GetDirectoryName(assembly.Location);

        }

        private static Assembly GetAssembly()
        {
            Assembly assembly =
                Assembly.GetEntryAssembly()
                ?? Assembly.GetExecutingAssembly()
                ?? throw new InvalidOperationException("Unable to get Entry or Executing Assembly!");
            return assembly;
        }
    }
}
