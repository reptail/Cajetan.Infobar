using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Cajetan.Infobar.Domain.Services
{
    public interface IWindowService
    {
        bool Alert(string title, string message);

        bool? OpenDialog(ObservableObject viewModel);
        bool? OpenDialog(ObservableObject viewModel, bool allowResize);
        bool? OpenDialog(ObservableObject viewModel, bool allowResize, double? width, double? height);

        void OpenWindow(ObservableObject viewModel);
        void OpenWindow(ObservableObject viewModel, bool allowResize);
        void OpenWindow(ObservableObject viewModel, bool allowResize, double? width, double? height);

        void CloseWindow(ObservableObject viewModel);
        void CloseWindow(ObservableObject viewModel, bool result);
    }
}
