using System;
using AppKit;

#if __IOS__
using UNStoryboard = AppKit.UIStoryboard;
using UNViewController = AppKit.UIViewController;
#endif

#if !__IOS__
using UNStoryboard = AppKit.NSStoryboard;
using UNViewController = AppKit.NSViewController;
#endif

namespace Nomads.Utilities.Extensions
{
	public static class StoryboardExtensions
	{
		public static T Instantiate<T> (this UNStoryboard storyboard) where T : UNViewController
		{
			#if __IOS__
			return storyboard.InstantiateViewController(typeof(T).Name) as T;
			#endif

			#if !__IOS__
			return storyboard.InstantiateControllerWithIdentifier(typeof(T).Name) as T;
			#endif
		}

		public static T Instantiate<T> (this UNStoryboard storyboard, string name) where T : UNViewController
		{
			#if __IOS__
			return storyboard.InstantiateViewController(name) as T;
			#endif

			#if !__IOS__
			return storyboard.InstantiateControllerWithIdentifier(name) as T;
			#endif
		}

		public static UNViewController Instantiate (this UNStoryboard storyboard, string name)
		{
			#if __IOS__
			return storyboard.InstantiateViewController(name) as UIViewController;
			#endif

			#if !__IOS__
			return storyboard.InstantiateControllerWithIdentifier(name) as UNViewController;
			#endif
		}
	}
}