// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace IMageAppResizer
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSButton AndroidHCheckBox { get; set; }

		[Outlet]
		AppKit.NSButton AndroidLCheckBox { get; set; }

		[Outlet]
		AppKit.NSButton AndroidMCheckBox { get; set; }

		[Outlet]
		AppKit.NSButton AndroidXCheckBox { get; set; }

		[Outlet]
		AppKit.NSButton AndroidXxCheckBox { get; set; }

		[Outlet]
		AppKit.NSButton AndroidXxxCheckBox { get; set; }

		[Outlet]
		AppKit.NSTextField FolderTextBox { get; set; }

		[Outlet]
		AppKit.NSButton IOS1xCheckBox { get; set; }

		[Outlet]
		AppKit.NSButton IOS2xCheckBox { get; set; }

		[Outlet]
		AppKit.NSButton IOS3xCheckBox { get; set; }

		[Outlet]
		AppKit.NSImageView IOSImage { get; set; }

		[Action ("ChooseButtonClick:")]
		partial void ChooseButtonClick (Foundation.NSObject sender);

		[Action ("GenerateImages:")]
		partial void GenerateImages (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (AndroidHCheckBox != null) {
				AndroidHCheckBox.Dispose ();
				AndroidHCheckBox = null;
			}

			if (AndroidLCheckBox != null) {
				AndroidLCheckBox.Dispose ();
				AndroidLCheckBox = null;
			}

			if (AndroidMCheckBox != null) {
				AndroidMCheckBox.Dispose ();
				AndroidMCheckBox = null;
			}

			if (AndroidXCheckBox != null) {
				AndroidXCheckBox.Dispose ();
				AndroidXCheckBox = null;
			}

			if (AndroidXxCheckBox != null) {
				AndroidXxCheckBox.Dispose ();
				AndroidXxCheckBox = null;
			}

			if (AndroidXxxCheckBox != null) {
				AndroidXxxCheckBox.Dispose ();
				AndroidXxxCheckBox = null;
			}

			if (FolderTextBox != null) {
				FolderTextBox.Dispose ();
				FolderTextBox = null;
			}

			if (IOS1xCheckBox != null) {
				IOS1xCheckBox.Dispose ();
				IOS1xCheckBox = null;
			}

			if (IOS2xCheckBox != null) {
				IOS2xCheckBox.Dispose ();
				IOS2xCheckBox = null;
			}

			if (IOS3xCheckBox != null) {
				IOS3xCheckBox.Dispose ();
				IOS3xCheckBox = null;
			}

			if (IOSImage != null) {
				IOSImage.Dispose ();
				IOSImage = null;
			}
		}
	}
}
