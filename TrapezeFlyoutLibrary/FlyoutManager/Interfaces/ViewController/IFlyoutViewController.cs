using UIKit;

namespace TrapezeFlyoutLibrary
{
	/// <summary>
	/// Main ViewController holding flyout should implement this interface.
	/// </summary>
	public interface IFlyoutViewController<T> where T: UIViewController
	{
		UIView SubViewControllerContainer { get; }
	}
}

