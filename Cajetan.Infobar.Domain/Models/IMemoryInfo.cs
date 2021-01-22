namespace Cajetan.Infobar.Domain.Models
{
    public interface IMemoryInfo : IUpdatableInfo
    {
        double Total { get; }
        double Used { get; }
        double Available { get; }
        string Unit { get; }
        double Percentage { get; }
    }
}
