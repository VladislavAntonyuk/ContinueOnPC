namespace ContinueOnPC.iOS.ShareExtension;

using System;
using Foundation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;
using Services;
using Social;
using UniformTypeIdentifiers;

public partial class ShareViewController : SLComposeServiceViewController
{
	private readonly IFirebaseService firebaseService;

	protected ShareViewController(IntPtr handle) : base(handle)
	{
		var services = new ServiceCollection();
		services.AddSingleton(Preferences.Default);
		services.AddSingleton(DeviceInfo.Current);
		services.AddSingleton<IPreferencesService, PreferencesService>();
		services.AddSingleton<IFirebaseService, FirebaseService>();
		var serviceProvider = services.BuildServiceProvider();
		firebaseService = serviceProvider.GetRequiredService<IFirebaseService>();
	}

	public override bool IsContentValid()
	{
		if (ExtensionContext is null)
		{
			return false;
		}

		foreach (var extensionItem in ExtensionContext.InputItems)
		{
			if (extensionItem.Attachments != null)
			{
				foreach (var attachment in extensionItem.Attachments)
				{
					return attachment.HasItemConformingTo(UTTypes.Url.Identifier);
				}
			}
		}

		return false;
	}

	public override void DidSelectPost()
	{
		if (ExtensionContext is null)
		{
			return;
		}

		foreach (var extensionItem in ExtensionContext.InputItems)
		{
			if (extensionItem.Attachments != null)
			{
				foreach (var attachment in extensionItem.Attachments)
				{
					if (attachment.HasItemConformingTo(UTTypes.Url.Identifier))
					{
						attachment.LoadItem(UTTypes.Url.Identifier, null, async (data, error) =>
						{
							var nsUrl = data as NSUrl;
							if (nsUrl != null)
							{
								var url = nsUrl.AbsoluteString;
								await firebaseService.PublishDataAsync(url);
							}
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