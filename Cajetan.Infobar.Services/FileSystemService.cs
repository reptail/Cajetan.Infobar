using Cajetan.Infobar.Domain.Services;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Cajetan.Infobar.Services
{
    public class FileSystemService : IFileSystemService
    {
        public string GetAppDataPath()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string assemblyName = GetAssembly().GetName().Name;
            return Path.Combine(appDataPath, assemblyName);

        }

        public string GetAssemblyPath()
        {
            Assembly assembly = GetAssembly();
            return Path.GetDirectoryName(assembly.Location);

        }

        public string PathCombineWithAppData(string segment)
        {
            string appDataPath = GetAppDataPath();
            string combined = PathCombine(appDataPath, segment);
            return combined;
        }

        public string PathCombine(params string[] segments)
        {
            return Path.Combine(segments);
        }

        public void Copy(string originalPath, string targetPath)
        {
            if (!File.Exists(originalPath)) return;

            File.Copy(originalPath, targetPath);
        }

        public string Read(string path)
        {
            if (!File.Exists(path))
                return string.Empty;

            using FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            using StreamReader sr = new StreamReader(fs);

            return sr.ReadToEnd();
        }

        public void Write(string path, string content)
        {
            if (!File.Exists(path))
                Directory.CreateDirectory(Path.GetDirectoryName(path));

            using FileStream fs = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.None);
            using StreamWriter sw = new StreamWriter(fs);

            sw.Write(content);
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
