using UIKit;

namespace TrapezeFlyoutLibrary
{
	/// <summary>
	/// Interface for flyout manager class (part responsible for managing sub ViewControllers)
	/// </summary>
	public interface IFlyoutSubViewControllerManager
	{
		void HandleHamburgerMenuTouch();
		void OnPanGestureDetect(UIPanGestureRecognizer gestureRecognizer);
	}
}

