using System;
using System.Collections.Generic;

#if INSIGHTS
using Xamarin;
#endif

namespace Nomads.Utilities.Logging
{
	public enum LogLevel
	{
		//ordered from LEAST to MOST verbose/informative
		None,
		Error,
		Report,
		Warn,
		Info,
		Debug,
		Verbose
	}

	public interface ILogUtility
	{
		LogLevel ConsoleLogLevel { get; set; }

		void Log (string message);

		void Log (string format, params object[] args);

		void Info (string message);

		void Info (string format, params object[] args);

		void Warn (string message);

		void Warn (string format, params object[] args);

		void Debug (string message);

		void Debug (string format, params object[] args);

		void Error (string message);

		void Error (string format, params object[] args);

		void Error (Exception e);

		void Error (Exception e, string message);

		void Error (Exception e, string format, params object[] args);

		#if INSIGHTS
		
		void Identify (string uid, IDictionary<string, string> table);

		void Identify (string uid, string key, string value);

		void Track (string trackIdentifier, IDictionary<string, string> table = null);

		void Track (string trackIdentifier, string key, string value);
		
		ITrackHandle TrackTime (string identifier, IDictionary<string, string> table = null);

		ITrackHandle TrackTime (string identifier, string key, string value);

		#endif
	}

	public static class LogLevelExtensions
	{
		public static bool AtLeast (this LogLevel thisLevel, LogLevel level)
		{
			var intLevel = (int)level;
			var current = (int)thisLevel;

			return intLevel <= current;
		}

		public static bool AtMost (this LogLevel thisLevel, LogLevel level)
		{
			var intLevel = (int)level;
			var current = (int)thisLevel;

			return intLevel >= current;
		}
	}
}