using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;

namespace ContinueOnPC.Windows;

internal class NetLauncherImplementation : ILauncher
{
    public Task<bool> CanOpenAsync(string uri)
    {
        return Task.FromResult(true);
    }

    public Task<bool> CanOpenAsync(Uri uri)
    {
        return Task.FromResult(true);
    }

    public Task OpenAsync(string uri)
    {
        StartProcess(uri);
        return Task.CompletedTask;
    }

    public Task OpenAsync(Uri uri)
    {
        StartProcess(uri.ToString());
        return Task.CompletedTask;
    }

    public Task OpenAsync(OpenFileRequest request)
    {
        StartProcess(request.File.FullPath);
        return Task.CompletedTask;
    }

    public Task<bool> TryOpenAsync(string uri)
    {
        return Task.FromResult(true);
    }

    public Task<bool> TryOpenAsync(Uri uri)
    {
        return Task.FromResult(true);
    }

    private static void StartProcess(string url)
    {
        try
        {
            Process.Start(new ProcessStartInfo(url)
            {
                CreateNoWindow = true,
                UseShellExecute = true
            });
        }
        catch
        {
            // hack because of this: https://github.com/dotnet/corefx/issues/10361
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}")
                {
                    CreateNoWindow = true,
                    UseShellExecute = true
                });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
            else
            {
                throw;
            }
        }
    }
}