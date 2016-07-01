using System;
namespace TrapezeFlyoutLibrary
{
	/// <summary>
	/// Flyout manager configuration class.
	/// </summary>
	public class FlyoutManagerConfiguration
	{
		/// <summary>
		/// Between 40 - 60 degrees.
		/// </summary>
		public nfloat Degrees { get; private set; }

		/// <summary>
		/// Between 5 - 30. (5 means that view controller is 5% smaller when flyout is shown)
		/// </summary>
		public int Scale { get; private set; }

		/// <summary>
		/// Between 30 - 60.
		/// </summary>
		/// <value>The pan threshold.</value>
		public int PanThreshold { get; private set; }

		/// <summary>
		/// Between 0.1 - 0.6.
		/// </summary>
		/// <value>The duration of the animation.</value>
		public double AnimationDuration { get; private set; }

		public FlyoutManagerConfiguration(nfloat degrees, int scale, 
		                                  int panThreshold, double animationDuration)
		{
			Degrees = degrees;
			Scale = scale;
			PanThreshold = panThreshold;
			AnimationDuration = animationDuration;
		}
	}
}

