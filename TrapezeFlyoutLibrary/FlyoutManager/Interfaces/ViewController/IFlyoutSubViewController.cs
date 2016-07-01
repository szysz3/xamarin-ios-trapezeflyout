namespace TrapezeFlyoutLibrary
{
	/// <summary>
	/// Every sub ViewController accessible via Flyout should implement this interface.
	/// </summary>
	public interface IFlyoutSubViewController
	{
		IFlyoutSubViewControllerManager FlyoutManager { get; set; }
	}
}

