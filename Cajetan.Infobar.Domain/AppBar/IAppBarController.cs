namespace Cajetan.Infobar.Domain.AppBar
{
    public interface IAppBarController
    {
        void DockBottom();
        void Undock();
        void Reset();
        void Shutdown();
    }
}
