namespace Cajetan.Infobar.Domain.Models
{
    public interface INetworkInfo : IUpdatableInfo
    {
        double DownloadRate { get; }
        double UploadRate { get; }

        (double rate, string unit) GetDownloadRate(ENetworkDisplayFormat displayFormat);
        (double rate, string unit) GetUploadRate(ENetworkDisplayFormat displayFormat);
    }
}
