// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace TrapezeFlyout
{
	[Register ("ViewController")]
	partial class FlyoutViewController
	{
		[Outlet]
		UIKit.UIView mainContainer { get; set; }

		[Outlet]
		UIKit.UIView vcContainerView { get; set; }

		[Action ("onItem1Touched:")]
		partial void onItem1Touched (UIKit.UIButton sender);

		[Action ("onItem2Touched:")]
		partial void onItem2Touched (Foundation.NSObject sender);

		[Action ("onItem3Touched:")]
		partial void onItem3Touched (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (vcContainerView != null) {
				vcContainerView.Dispose ();
				vcContainerView = null;
			}

			if (mainContainer != null) {
				mainContainer.Dispose ();
				mainContainer = null;
			}
		}
	}
}
