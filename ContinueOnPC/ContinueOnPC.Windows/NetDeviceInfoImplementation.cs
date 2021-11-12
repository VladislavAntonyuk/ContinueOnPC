using System;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;

namespace ContinueOnPC.Windows;

internal class NetDeviceInfoImplementation : IDeviceInfo
{
    public DevicePlatform Platform { get; } = DevicePlatform.Create("Windows");
    public DeviceIdiom Idiom { get; } = DeviceIdiom.Desktop;
    public DeviceType DeviceType { get; } = DeviceType.Physical;
    public string Model { get; } = Environment.OSVersion.Platform.ToString();
    public string Manufacturer => Environment.OSVersion.VersionString;
    public string Name => Environment.MachineName;
    public string VersionString => Environment.OSVersion.VersionString;
    public Version Version { get; } = Environment.OSVersion.Version;
}