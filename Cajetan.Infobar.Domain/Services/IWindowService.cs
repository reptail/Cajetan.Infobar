using Cajetan.Infobar.Domain.ViewModels;

namespace Cajetan.Infobar.Domain.Services
{
    public interface IWindowService
    {
        bool Alert(string title, string message);

        string ShowColorDialog(string title, string currentColorHex);

        bool? OpenDialog(IWindowViewModel viewModel);
        bool? OpenDialog(IWindowViewModel viewModel, bool allowResize);
        bool? OpenDialog(IWindowViewModel viewModel, bool allowResize, double? width, double? height);

        void OpenWindow(IWindowViewModel viewModel);
        void OpenWindow(IWindowViewModel viewModel, bool allowResize);
        void OpenWindow(IWindowViewModel viewModel, bool allowResize, double? width, double? height);

        void CloseWindow(IWindowViewModel viewModel);
        void CloseWindow(IWindowViewModel viewModel, bool result);
    }
}
