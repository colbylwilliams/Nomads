using System;
using System.IO;

using Foundation;

using System.Collections.Generic;
using System.Linq;

//TODO: Remove dependency on ServiceStack
using ServiceStack;
using ServiceStack.Text;

using UIKit;

namespace Nomads.Utilities.Settings
{
	public class Settings : ISettings
	{
		#region Utilities

		public static void RegisterDefaultSettings ()
		{
			var path = Path.Combine(NSBundle.MainBundle.PathForResource("Settings", "bundle"), "Root.plist");

			using (NSString keyString = new NSString ("Key"), defaultString = new NSString ("DefaultValue"), preferenceSpecifiers = new NSString ("PreferenceSpecifiers"))
			using (var settings = NSDictionary.FromFile(path))
			using (var preferences = (NSArray)settings.ValueForKey(preferenceSpecifiers))
			using (var registrationDictionary = new NSMutableDictionary ()) {
				for (nuint i = 0; i < preferences.Count; i++)
					using (var prefSpecification = preferences.GetItem<NSDictionary>(i))
					using (var key = (NSString)prefSpecification.ValueForKey(keyString))
						if (key != null)
							using (var def = prefSpecification.ValueForKey(defaultString))
								if (def != null)
									registrationDictionary.SetValueForKey(def, key);

				NSUserDefaults.StandardUserDefaults.RegisterDefaults(registrationDictionary);

				#if DEBUG
				SetSetting(SettingsKeys.UserReferenceKey, debugReferenceKey);
				#else
				SetSetting(SettingsKeys.UserReferenceKey, UIDevice.CurrentDevice.IdentifierForVendor.AsString());
				#endif

				Synchronize();
			}
		}

		public static void Synchronize () => NSUserDefaults.StandardUserDefaults.Synchronize();

		public static void SetSetting (string key, string value) => NSUserDefaults.StandardUserDefaults.SetString(value, key);

		public static void SetSetting (string key, bool value) => NSUserDefaults.StandardUserDefaults.SetBool(value, key);

		public static void SetSetting (string key, int value) => NSUserDefaults.StandardUserDefaults.SetInt(value, key);

		public static void SetSetting (string key, DateTime value) => SetSetting(key, value.ToString());

		public static void SetSetting (string key, IDictionary<string, string> value) => SetSetting(key, value.ToJson());

		public static int Int32ForKey (string key) => Convert.ToInt32(NSUserDefaults.StandardUserDefaults.IntForKey(key));

		public static bool BoolForKey (string key) => NSUserDefaults.StandardUserDefaults.BoolForKey(key);

		public static string StringForKey (string key) => NSUserDefaults.StandardUserDefaults.StringForKey(key);

		public static DateTime DateTimeForKey (string key)
		{
			DateTime outDateTime;

			return DateTime.TryParse(NSUserDefaults.StandardUserDefaults.StringForKey(key), out outDateTime) ? outDateTime : DateTime.MinValue;
		}

		public static IDictionary<string, string> DictionaryForKey (string key) => JsonObject.Parse(StringForKey(key));

		#endregion


		#region About

		public string VersionNumber {
			get{ return StringForKey(SettingsKeys.VersionNumber); }
		}

		public string BuildNumber {
			get{ return StringForKey(SettingsKeys.BuildNumber); }
		}

		public string GitCommitHash {
			get{ return StringForKey(SettingsKeys.GitCommitHash); }
		}

		public string VersionBuildString {
			get{ return string.Format($"v{VersionNumber} b{BuildNumber}");} 
		}

		#endregion


		#region Internal

		public bool FirstLaunch {
			get {
				// this is actually false if it's the first time the app is launched
				var firstL = !BoolForKey(SettingsKeys.FirstLaunch);

				if (firstL) {
					SetSetting(SettingsKeys.FirstLaunch, true);

					// Give these some good defaults
				}

				return firstL;
			}
		}

		#endregion


		#region Reporting

		const string debugReferenceKey = "DEBUG";

		public string UserReferenceKey {
			get {
				var key = StringForKey(SettingsKeys.UserReferenceKey);

				if (string.IsNullOrEmpty(key) || key.Equals(debugReferenceKey, StringComparison.OrdinalIgnoreCase)) {

					#if DEBUG
					key = debugReferenceKey;
					#else
					key = UIDevice.CurrentDevice.IdentifierForVendor.AsString();
					#endif

					SetSetting(SettingsKeys.UserReferenceKey, key);
				}

				return key;
			}
		}

		public string UserIdentity {
			get { return StringForKey(SettingsKeys.UserIdentity); }
			set { SetSetting(SettingsKeys.UserIdentity, value); }
		}

		public IDictionary<string, string> UserTraits {
			get { return DictionaryForKey(SettingsKeys.UserTraits); }
			set { SetSetting(SettingsKeys.UserTraits, value); }
		}

		#endregion
	}
}