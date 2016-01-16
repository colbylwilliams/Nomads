using System.Collections.Generic;

namespace Nomads.Utilities.Settings
{
	public interface ISettings
	{
		#region Utilities

		//void RegisterDefaultSettings ();

		#endregion


		#region About

		string VersionNumber { get; }

		string BuildNumber { get; }

		string GitCommitHash { get; }

		string VersionBuildString { get; }

		#endregion


		#region Internal

		bool FirstLaunch { get; }

		#endregion


		#region Reporting

		string UserReferenceKey { get; }

		string UserIdentity { get; set; }

		IDictionary<string, string> UserTraits { get; set; }

		#endregion
	}
}