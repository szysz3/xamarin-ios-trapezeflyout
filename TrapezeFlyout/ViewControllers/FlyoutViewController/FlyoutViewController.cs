using System;
using Foundation;
using TrapezeFlyoutLibrary;
using UIKit;

namespace TrapezeFlyout
{
	public partial class FlyoutViewController : UIViewController, IFlyoutViewController<UIViewController>
	{
		private IFlyoutViewControllerManager flyoutManager;

		public UIView SubViewControllerContainer
		{
			get
			{
				return vcContainerView;
			}
		}

		protected FlyoutViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			FlyoutManagerConfiguration configuration = new FlyoutManagerConfiguration(
				new nfloat(55), 5, 60, 0.3d);
			flyoutManager = new FlyoutManager(this, configuration);
			flyoutManager.Setup(FirstViewController.Tag);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
		}

		partial void onItem1Touched(UIButton sender)
		{
			flyoutManager.ShowViewController(FirstViewController.Tag);
		}

		partial void onItem2Touched(NSObject sender)
		{
			flyoutManager.ShowViewController(SecondViewController.Tag);
		}

		partial void onItem3Touched(NSObject sender)
		{
			flyoutManager.ShowViewController(ThirdViewController.Tag);
		}
	}
}

