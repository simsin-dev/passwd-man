using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;
using Avalonia.Input.Platform;
using Avalonia.Controls.ApplicationLifetimes;

namespace passwd_man;

public class Clipboard {

    //yoinked from stack overflow
    public static IClipboard Get() {

        //Desktop
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: { } window }) {
            return window.Clipboard!;

        }
        //Android (and iOS?)
        else if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime { MainView: { } mainView }) {
            var visualRoot = mainView.GetVisualRoot();
            if (visualRoot is TopLevel topLevel) {
                return topLevel.Clipboard!;
            }
        }

        return null!;
    }
}