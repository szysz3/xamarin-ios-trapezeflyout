namespace TrapezeFlyoutLibrary
{
	/// <summary>
	/// Interface for flyout manager class (part responsible for managing main ViewController)
	/// </summary>
	public interface IFlyoutViewControllerManager
	{
		void Setup(string controllerId);
		void ShowViewController(string controllerId);
	}
}

