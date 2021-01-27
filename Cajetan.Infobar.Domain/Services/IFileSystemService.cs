using System.Threading.Tasks;

namespace Cajetan.Infobar.Domain.Services
{
    public interface IFileSystemService
    {
        string PathCombineWithAppData(string segment);
        string PathCombine(params string[] segments);

        string Read(string path);
        void Write(string path, string content);
        void Copy(string originalPath, string targetPath);
    }
}
