using Cajetan.Infobar.Domain.Services;
using Cajetan.Infobar.Services.Helpers;
using System.IO;

namespace Cajetan.Infobar.Services
{
    public class FileSystemService : IFileSystemService
    {
        public string PathCombineWithAppData(string segment)
        {
            string appDataPath = AppDataHelper.GetAppDataPath();
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

            File.Copy(originalPath, targetPath, true);
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
    }
}
