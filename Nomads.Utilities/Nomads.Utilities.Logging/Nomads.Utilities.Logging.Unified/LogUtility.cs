using System;
using System.Collections.Generic;
using Nomads.Utilities.Logging;

#if INSIGHTS
using Xamarin;
#endif

namespace Nomads.Utilities.Logging.Unified
{
	public class LogUtility : LogUtilityBase
	{
		public LogUtility (LogLevel level) : base(level)
		{
			this.ConsoleLogLevel = level;
		}

		#if DEBUG
		
		public LogUtility () : this(LogLevel.Debug)
		{

		}

		#endif

		#if !DEBUG
		
		public LogUtility () : this(LogLevel.Info)
		{
		
		}

		#endif

		protected override void ConsoleLog (string message)
		{
			Console.WriteLine(message);
		}

		protected override void ConsoleLog (string format, params object[] args)
		{
			Console.WriteLine(format, args);
		}

		protected override void LogRemoteException (Exception ex)
		{
			#if INSIGHTS

			Insights.Report(ex);

			#endif
			// #if !DEBUG

			// #endif
		}

		protected override void LogRemoteException (Exception ex, string message)
		{	
			#if INSIGHTS

			if (String.IsNullOrEmpty(message)) {

				Insights.Report(ex);
			
			} else {
			
				Insights.Report(ex, "Details", message);
			}

			#endif

			Console.WriteLine("{0}\n{1}", message ?? string.Empty, ex.Message);

			// #if !DEBUG

			// #endif
		}

		#if INSIGHTS
		
		protected override void InsightsIdentify (string uid, IDictionary<string, string> table)
		{
			Insights.Identify(uid, table);
		}

		protected override void InsightsIdentify (string uid, string key, string value)
		{
			Insights.Identify(uid, key, value);
		}

		protected override void InsightsTrack (string trackIdentifier, IDictionary<string, string> table = null)
		{
			Insights.Track(trackIdentifier, table);
		}

		protected override void InsightsTrack (string trackIdentifier, string key, string value)
		{
			Insights.Track(trackIdentifier, key, value);
		}

		protected override ITrackHandle InsightsTrackTime (string identifier, IDictionary<string, string> table = null)
		{
			return Insights.TrackTime(identifier, table);
		}

		protected override ITrackHandle InsightsTrackTime (string identifier, string key, string value)
		{
			return Insights.TrackTime(identifier, key, value);
		}

		#endif

		protected void SetReportParam (string key, string value)
		{
		}
	}
}