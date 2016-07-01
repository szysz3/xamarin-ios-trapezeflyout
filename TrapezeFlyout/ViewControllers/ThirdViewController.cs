using System;
using TrapezeFlyoutLibrary;
using UIKit;

namespace TrapezeFlyout
{
	public partial class ThirdViewController : UIViewController, IFlyoutSubViewController
	{
		public static string Tag = "ThirdViewController";

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

		public ThirdViewController() : base(Tag, null)
		{
		}

		protected ThirdViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
			View.AddGestureRecognizer(new UIPanGestureRecognizer(OnPanGesturedetect));
		}

		partial void onFlyoutTouched(UIKit.UIButton sender)
		{
			FlyoutManager.HandleHamburgerMenuTouch();
		}

		private void OnPanGesturedetect(UIPanGestureRecognizer sender)
		{
			FlyoutManager.OnPanGestureDetect(sender);
		}
	}
}


