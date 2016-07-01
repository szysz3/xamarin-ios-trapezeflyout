using System;
using CoreGraphics;
using TrapezeFlyoutLibrary;
using UIKit;

namespace TrapezeFlyout
{
	public partial class FirstViewController : UIViewController, IFlyoutSubViewController
	{
		public static string Tag = "FirstViewController";

		private IFlyoutSubViewControllerManager flyoutManager;
		public IFlyoutSubViewControllerManager FlyoutManager
		{
			get
			{
				return flyoutManager;
			}

			set
			{
				flyoutManager = value;
			}
		}

		public FirstViewController() : base(Tag, null)
		{
		}

		protected FirstViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Perform any additional setup after loading the view, typically from a nib.
			View.AddGestureRecognizer(new UIPanGestureRecognizer(OnPanGesturedetect));
		}

		private void SetupSubViewContainer()
		{
			var applicationFrame = UIApplication.SharedApplication.Windows[0].Frame;
			var width = applicationFrame.Width;
			var height = applicationFrame.Height;

			var oldFrame = View.Frame;
			View.Frame = new CGRect(oldFrame.X, oldFrame.Y, width, height);
		}

		partial void onFlyoutTouched(UIKit.UIButton sender)
		{
			flyoutManager.HandleHamburgerMenuTouch();
		}

		private void OnPanGesturedetect(UIPanGestureRecognizer sender)
		{
			flyoutManager.OnPanGestureDetect(sender);
		}
	}
}


