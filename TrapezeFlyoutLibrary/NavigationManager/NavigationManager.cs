using Foundation;
using UIKit;

namespace TrapezeFlyoutLibrary
{
	/// <summary>
	/// Class used to swap ViewControllers.
	/// </summary>
	public class NavigationManager
	{
		private UIViewController flyoutViewController;
		private FlyoutManager flyoutManager;

		private UIView subViewControllerContainer;
		private UIViewController currentViewController;

		public NavigationManager(UIViewController flyoutViewController, 
		                         UIView subViewControllerContainer,
		                         FlyoutManager flyoutManager)
		{
			this.flyoutViewController = flyoutViewController;
			this.subViewControllerContainer = subViewControllerContainer;
			this.flyoutManager = flyoutManager;
		}

		#region Methods
		/// <summary>
		/// Method used to switch ViewControllers.
		/// </summary>
		/// <param name="controllerId">Controller identifier on Storyboard.</param>
		public void ShowViewController(string controllerId)
		{
			var newViewController = flyoutViewController.Storyboard.InstantiateViewController(controllerId);
			CycleFromViewController(currentViewController, newViewController);
			currentViewController = newViewController;

			var flyoutController = currentViewController as IFlyoutSubViewController;
			flyoutController.FlyoutManager = flyoutManager;
		}

		/// <summary>
		/// Loads the initial view controller.
		/// </summary>
		/// <param name="controllerId">Controller identifier.</param>
		public void LoadInitialViewController(string controllerId)
		{
			currentViewController = flyoutViewController.Storyboard.InstantiateViewController(controllerId);

			var flyoutController = currentViewController as IFlyoutSubViewController;
			flyoutController.FlyoutManager = flyoutManager;

			flyoutViewController.AddChildViewController(currentViewController);
			AddSubView(currentViewController.View, subViewControllerContainer);

			subViewControllerContainer.SetNeedsLayout();
			subViewControllerContainer.LayoutIfNeeded();
		}

		#region Private ViewController Swap
		private void CycleFromViewController(UIViewController oldVC, UIViewController newVC)
		{
			oldVC.WillMoveToParentViewController(null);
			flyoutViewController.AddChildViewController(newVC);
			AddSubView(newVC.View, subViewControllerContainer);

			UIView.Animate(0.5, () =>
			{
				newVC.View.Alpha = 1;
				oldVC.View.Alpha = 0;

			}, () =>
			{
				oldVC.View.RemoveFromSuperview();
				oldVC.RemoveFromParentViewController();
				newVC.DidMoveToParentViewController(flyoutViewController);
			});
		}

		private void AddSubView(UIView subView, UIView parentView)
		{
			parentView.AddSubview(subView);
			var viewBindingDict = new NSMutableDictionary();
			viewBindingDict.Add(new NSString("subView"), subView);
			var hCst = NSLayoutConstraint.FromVisualFormat("H:|[subView]",
												0,
												null,
												viewBindingDict);
			var vCst = NSLayoutConstraint.FromVisualFormat("V:|[subView]",
												0,
												null,
												viewBindingDict);

			parentView.AddConstraints(hCst);
			parentView.AddConstraints(vCst);
		}
		#endregion
		#endregion
	}
}

