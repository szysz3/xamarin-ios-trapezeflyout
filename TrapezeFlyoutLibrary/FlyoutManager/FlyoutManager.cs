using System;
using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace TrapezeFlyoutLibrary
{
	/// <summary>
	/// Class responsible for communication between main ViewController and all sub ViewControllers as well as
	/// for managing flyout state.
	/// </summary>
	public class FlyoutManager : IFlyoutSubViewControllerManager, IFlyoutViewControllerManager
	{
		private NavigationManager navigationManager;

		private UIViewController flyoutViewController;
		private FlyoutManagerConfiguration configuration;

		private UIView subViewControllerContainer;
		private nfloat subViewControllerContainerWidth;

		private nfloat lastLocation;
		private CGPoint startLocation;

		private bool flyoutShown;
		private static readonly nfloat PerspectiveCoeff = 1f / 500;

		public FlyoutManager(IFlyoutViewController<UIViewController> flyoutViewController, 
		                     FlyoutManagerConfiguration configuration)
		{
			this.configuration = configuration;
			this.subViewControllerContainer = flyoutViewController.SubViewControllerContainer;
			this.flyoutViewController = (UIViewController)flyoutViewController;

			navigationManager = new NavigationManager(this.flyoutViewController, 
			                                          subViewControllerContainer, 
			                                          this);
		}

		#region Methods
		/// <summary>
		/// Method used to initialize FyoutManager. Should be invoked in ViewController's ViewDidLoad().
		/// </summary>
		/// <param name="firstControllerId">Initial view controller identifier.</param>
		public void Setup(string firstControllerId)
		{
			SetupSubViewContainer();
			navigationManager.LoadInitialViewController(firstControllerId);
			SetupShadows();

			//center of right edge
			var anchorPoint = new CGPoint(1f, 0.5f);
			SetupAnchorPoint(anchorPoint, subViewControllerContainer);
		}

		/// <summary>
		/// Method handles flyout state. Should be invoked every time user touches hamburger menu. 
		/// </summary>
		public void HandleHamburgerMenuTouch()
		{
			if (!flyoutShown)
			{
				ShowFlyout();
			}
			else
			{
				HideFlyout();
			}
		}

		/// <summary>
		/// Method used to switch ViewControllers.
		/// </summary>
		/// <param name="controllerId">Controller identifier on Storyboard.</param>
		public void ShowViewController(string controllerId)
		{
			navigationManager.ShowViewController(controllerId);
			HideFlyout();
		}

		/// <summary>
		/// Method should be invoked in UIPanGestureRecognizer callback method.
		/// </summary>
		/// <param name="gestureRecognizer">Gesture recognizer.</param>
		public void OnPanGestureDetect(UIPanGestureRecognizer gestureRecognizer)
		{
			if (gestureRecognizer.State == UIGestureRecognizerState.Began)
			{
				HandleGestureBegin(gestureRecognizer);
			}
			else if (gestureRecognizer.State == UIGestureRecognizerState.Changed)
			{
				HandleGestureChanged(gestureRecognizer);
			}
			else if (gestureRecognizer.State == UIGestureRecognizerState.Recognized)
			{
				HandleGestureRecognized(gestureRecognizer);
			}
		}

		#region Private Setup
		private void SetupShadows()
		{
			subViewControllerContainer.Layer.ShadowOffset = new CGSize(3f, 3f);
			subViewControllerContainer.Layer.ShadowColor = UIColor.Black.CGColor;
			subViewControllerContainer.Layer.ShadowRadius = 5f;
			subViewControllerContainer.Layer.ShadowOpacity = 0.3f;

			subViewControllerContainer.TranslatesAutoresizingMaskIntoConstraints = true;
		}

		private void SetupSubViewContainer()
		{
			var applicationFrame = UIApplication.SharedApplication.Windows[0].Frame;
			var width = applicationFrame.Width;
			var height = applicationFrame.Height;

			var oldFrame = subViewControllerContainer.Frame;
			subViewControllerContainer.Frame = new CGRect(oldFrame.X, oldFrame.Y, width, height);
			subViewControllerContainerWidth = subViewControllerContainer.Frame.Width;
		}

		private void SetupAnchorPoint(CGPoint anchorPoint, UIView forView)
		{
			var newPoint = new CGPoint(forView.Bounds.Size.Width * anchorPoint.X,
									   forView.Bounds.Size.Height * anchorPoint.Y);
			var oldPoint = new CGPoint(forView.Bounds.Size.Width * forView.Layer.AnchorPoint.X,
									   forView.Bounds.Size.Height * forView.Layer.AnchorPoint.Y);

			newPoint = forView.Transform.TransformPoint(newPoint);
			oldPoint = forView.Transform.TransformPoint(oldPoint);

			var position = forView.Layer.Position;
			position.X -= oldPoint.X;
			position.X += newPoint.X;

			position.Y -= oldPoint.Y;
			position.Y += newPoint.Y;

			forView.Layer.Position = position;
			forView.Layer.AnchorPoint = anchorPoint;
		}
		#endregion

		#region Private Gesture Handling
		private void HandleGestureChanged(UIPanGestureRecognizer gestureRecognizer)
		{
			CGPoint location = gestureRecognizer.TranslationInView(flyoutViewController.View);
			var locX = lastLocation + location.X;

			var degrees = (locX * (configuration.Degrees / subViewControllerContainerWidth));
			var scale = ((locX * (configuration.Scale / subViewControllerContainerWidth)) / 100);

			if (degrees < configuration.Degrees + 10
			    && degrees > 0)
			{
				CATransform3D transform = CATransform3D.Identity;
				transform.m34 = PerspectiveCoeff;
				transform = transform.Rotate(new nfloat(degrees * Math.PI / 180f), 0, 1, 0);
				transform = transform.Scale(new nfloat(1 - scale));

				subViewControllerContainer.Layer.Transform = transform;
			}
		}

		private void HandleGestureRecognized(UIPanGestureRecognizer gestureRecognizer)
		{
			var endLocation = gestureRecognizer.LocationInView(flyoutViewController.View);
			nfloat diff = endLocation.X - startLocation.X;

			if (diff < 0)
			{
				if (Math.Abs(diff) > configuration.PanThreshold)
				{
					HideFlyout();
				}
				else 
				{
					ShowFlyout();
				}
			}
			else if (diff > 0)
			{
				if (Math.Abs(diff) > configuration.PanThreshold)
				{
					ShowFlyout();
				}
				else
				{
					HideFlyout();
				}
			}

			lastLocation = 0;		
		}

		private void HandleGestureBegin(UIPanGestureRecognizer gestureRecognizer)
		{
			if (flyoutShown)
			{
				var frame = subViewControllerContainer.Frame;
				lastLocation = frame.Right;
			}

			startLocation = gestureRecognizer.LocationInView(flyoutViewController.View);
		}

		#endregion

		#region Private Animations
		private void ShowFlyout()
		{
			flyoutShown = true;
			AnimateFlyout(configuration.Degrees, 
			              1 - (configuration.Scale / 100f), 
			              "ShowFlyoutAnimation");
		}

		private void HideFlyout()
		{
			flyoutShown = false;
			AnimateFlyout(0, 1, "HideFlyoutAnimation");
		}

		private void AnimateFlyout(nfloat animationDegrees, float animationScale, string animationName)
		{
			UIView.BeginAnimations(animationName);
			UIView.SetAnimationDuration(configuration.AnimationDuration);
			UIView.SetAnimationCurve(UIViewAnimationCurve.EaseInOut);

			CATransform3D transform = CATransform3D.Identity;
			transform.m34 = PerspectiveCoeff;

			transform = transform.Rotate(new nfloat(animationDegrees * Math.PI / 180f), 0, 1, 0);
			transform = transform.Scale(new nfloat(animationScale));

			subViewControllerContainer.Layer.Transform = transform;
			UIView.CommitAnimations();
		}
		#endregion
		#endregion
	}
}

