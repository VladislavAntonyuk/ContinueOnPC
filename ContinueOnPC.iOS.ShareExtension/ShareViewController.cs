using System;
using Foundation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;
using MobileCoreServices;
using Social;

namespace ContinueOnPC.iOS.ShareExtension;

public partial class ShareViewController : SLComposeServiceViewController
{
    IFirebaseService firebaseService;
    protected ShareViewController(IntPtr handle) : base(handle)
    {
        var services = new ServiceCollection();
        services.AddSingleton<IPreferences>(Preferences.Default);
        services.AddSingleton<IDeviceInfo>(DeviceInfo.Current);
        services.AddSingleton<IPreferencesService, PreferencesService>();
        services.AddSingleton<IFirebaseService, FirebaseService>();
        var serviceProvider = services.BuildServiceProvider();
        firebaseService = serviceProvider.GetRequiredService<IFirebaseService>();
    }

    public override bool IsContentValid()
    {
        foreach (var extensionItem in ExtensionContext.InputItems)
        {
            if (extensionItem.Attachments != null)
            {
                foreach (var attachment in extensionItem.Attachments)
                {
                    return attachment.HasItemConformingTo(UTType.URL);
                }
            }
        }

        return false;
    }

    public override void DidSelectPost()
    {
        foreach (var extensionItem in ExtensionContext.InputItems)
        {
            if (extensionItem.Attachments != null)
            {
                foreach (var attachment in extensionItem.Attachments)
                {
                    if (attachment.HasItemConformingTo(UTType.URL))
                    {
                        attachment.LoadItem(UTType.URL, null, async (data, error) =>
                        {
                            var nsUrl = data as NSUrl;
                            var url = nsUrl.AbsoluteString;
                            await firebaseService.PublishDataAsync(url);
                        });
                    }
                }
            }
        }

        // Inform the host that we're done, so it un-blocks its UI. Note: Alternatively you could call super's -didSelectPost, which will similarly complete the extension context.
        ExtensionContext.CompleteRequest(Array.Empty<NSExtensionItem>(), null);
    }

    public override SLComposeSheetConfigurationItem[] GetConfigurationItems()
    {
        // To add configuration options via table cells at the bottom of the sheet, return an array of SLComposeSheetConfigurationItem here.
        return Array.Empty<SLComposeSheetConfigurationItem>();
    }
}