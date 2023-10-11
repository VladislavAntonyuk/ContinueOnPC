namespace ContinueOnPC.iOS.ShareExtension;

using Foundation;
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

		return ExtensionContext.InputItems
			.Where(extensionItem => extensionItem.Attachments is not null)
			.SelectMany(extensionItem => extensionItem.Attachments!)
			.Any(attachment => attachment.HasItemConformingTo(UTTypes.Url.Identifier));
	}

	public override void DidSelectPost()
	{
		if (ExtensionContext is null)
		{
			return;
		}

		foreach (var attachment in ExtensionContext.InputItems		
			.Where(extensionItem => extensionItem.Attachments is not null)
			.SelectMany(extensionItem => extensionItem.Attachments!)
			.Where(attachment => attachment.HasItemConformingTo(UTTypes.Url.Identifier)))
		{
			attachment.LoadItem(UTTypes.Url.Identifier, null, async (data, error) =>
			{
				var nsUrl = data as NSUrl;
				var url = nsUrl?.AbsoluteString;
				if (url is not null)
				{
					await firebaseService.PublishDataAsync(url);
				}
			});
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