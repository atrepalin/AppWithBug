using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Microsoft.Maui.LifecycleEvents;

#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
#endif

namespace AppWithBug;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseSkiaSharp(true)
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

#if WINDOWS
		builder.ConfigureLifecycleEvents(events =>
        {
            events.AddWindows(wndLifeCycleBuilder =>
            {
                wndLifeCycleBuilder.OnWindowCreated(window =>
                {
                    var nativeWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                    var win32WindowsId = Win32Interop.GetWindowIdFromWindow(nativeWindowHandle);
                    var winuiAppWindow = AppWindow.GetFromWindowId(win32WindowsId);

                    if (winuiAppWindow.Presenter is OverlappedPresenter p)
                    {
                        window.ExtendsContentIntoTitleBar = false;
                        p.SetBorderAndTitleBar(false, false);
                    }

                    var containerForm = WinLib.ContainerForm.CreateContainerForm(
                        800,
                        500,
                        true);

                    containerForm.Enclose(nativeWindowHandle);
                    containerForm.TitleText = "Example app";
                    containerForm.FormClosedSimple += (sender, args) => Application.Current!.Quit();
                    containerForm.BorderStyle = WinLib.FormBorderStyle.FixedToolWindow;
                });
            });
        });
#endif
        return builder.Build();
	}
}
