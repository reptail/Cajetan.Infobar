namespace Cajetan.Infobar.Domain.Models
{
    public interface IProcessorInfo : IUpdatableInfo
    {
        double Percentage { get; }
    }
}
