using System;
using System.Collections.Generic;

#if INSIGHTS
using Xamarin;
#endif

namespace Nomads.Utilities.Logging
{
	public abstract class LogUtilityBase : ILogUtility
	{
		public LogLevel ConsoleLogLevel { get; set; }

		protected LogUtilityBase (LogLevel level)
		{
			ConsoleLogLevel = level;
		}

		#if DEBUG

		protected LogUtilityBase () : this(LogLevel.Debug)
		{

		}

		#else
		
		protected LogUtilityBase () : this (LogLevel.Info)
		{

		}

		#endif

		protected abstract void ConsoleLog (string message);

		protected abstract void ConsoleLog (string format, params object[] args);

		protected abstract void LogRemoteException (Exception ex);

		protected abstract void LogRemoteException (Exception ex, string message);

		#if INSIGHTS
		
		protected abstract void InsightsIdentify (string uid, IDictionary<string, string> table);

		protected abstract void InsightsIdentify (string uid, string key, string value);

		protected abstract void InsightsTrack (string trackIdentifier, IDictionary<string, string> table = null);

		protected abstract void InsightsTrack (string trackIdentifier, string key, string value);

		protected abstract ITrackHandle InsightsTrackTime (string identifier, IDictionary<string, string> table = null);

		protected abstract ITrackHandle InsightsTrackTime (string identifier, string key, string value);


		public void Identify (string uid, IDictionary<string, string> table)
		{
			InsightsIdentify(uid, table);
		}

		public void Identify (string uid, string key, string value)
		{
			InsightsIdentify(uid, key, value);
		}

		public void Track (string trackIdentifier, IDictionary<string, string> table = null)
		{
			InsightsTrack(trackIdentifier, table);
		}

		public void Track (string trackIdentifier, string key, string value)
		{
			InsightsTrack(trackIdentifier, key, value);
		}


		public ITrackHandle TrackTime (string identifier, IDictionary<string, string> table = null)
		{
			return InsightsTrackTime(identifier, table);
		}


		public ITrackHandle TrackTime (string identifier, string key, string value)
		{
			return InsightsTrackTime(identifier, key, value);
		}

		#endif


		#region ILogUtility

		public void Log (string message)
		{
			var msg = MessageFormatUtility.FormatMsg(LogLevel.Info, message);

			if (ConsoleLogLevel.AtLeast(LogLevel.Info)) {
				ConsoleLog(msg);
			}
		}

		public void Log (string format, params object[] args)
		{
			var msg = MessageFormatUtility.FormatMsg(LogLevel.Info, format, args);

			if (ConsoleLogLevel.AtLeast(LogLevel.Info)) {
				ConsoleLog(msg);
			}
		}

		public void Info (string message)
		{
			var msg = MessageFormatUtility.FormatMsg(LogLevel.Info, message);

			if (ConsoleLogLevel.AtLeast(LogLevel.Info)) {
				ConsoleLog(msg);
			}
		}

		public void Info (string format, params object[] args)
		{
			var msg = MessageFormatUtility.FormatMsg(LogLevel.Info, format, args);

			if (ConsoleLogLevel.AtLeast(LogLevel.Info)) {
				ConsoleLog(msg);
			}
		}

		public void Warn (string message)
		{
			var msg = MessageFormatUtility.FormatMsg(LogLevel.Warn, message);

			if (ConsoleLogLevel.AtLeast(LogLevel.Warn)) {
				ConsoleLog(msg);
			}
		}

		public void Warn (string format, params object[] args)
		{
			var msg = MessageFormatUtility.FormatMsg(LogLevel.Warn, format, args);

			if (ConsoleLogLevel.AtLeast(LogLevel.Warn)) {
				ConsoleLog(msg);
			}
		}

		public void Debug (string message)
		{
			var msg = MessageFormatUtility.FormatMsg(LogLevel.Debug, message);

			if (ConsoleLogLevel.AtLeast(LogLevel.Debug)) {
				ConsoleLog(msg);
			}
		}

		public void Debug (string format, params object[] args)
		{
			var msg = MessageFormatUtility.FormatMsg(LogLevel.Debug, format, args);

			if (ConsoleLogLevel.AtLeast(LogLevel.Debug)) {
				ConsoleLog(msg);
			}
		}

		#endregion


		#region Error

		public void Error (string message)
		{
			var msg = MessageFormatUtility.FormatMsg(LogLevel.Error, message);

			if (ConsoleLogLevel.AtLeast(LogLevel.Error)) {
				ConsoleLog(msg);
			}
		}

		public void Error (string format, params object[] args)
		{
			var msg = MessageFormatUtility.FormatMsg(LogLevel.Error, format, args);

			if (ConsoleLogLevel.AtLeast(LogLevel.Error)) {
				ConsoleLog(msg);
			}
		}

		public void Error (Exception e)
		{
			string msg = null;

			if (ConsoleLogLevel.AtLeast(LogLevel.Verbose)) {
				msg = MessageFormatUtility.FormatMsg(LogLevel.Error, "{0}\n{1}", e.Message, e.StackTrace);
			} else {
				msg = MessageFormatUtility.FormatMsg(LogLevel.Error, "{0}", e.Message);
			}

			if (ConsoleLogLevel.AtLeast(LogLevel.Error)) {
				ConsoleLog(msg);
				LogRemoteException(e);
			}
		}

		public void Error (Exception e, string format, params object[] args)
		{
			string msg = null;

			if (ConsoleLogLevel.AtLeast(LogLevel.Verbose)) {
				msg = MessageFormatUtility.FormatMsg(LogLevel.Error, "{0} : {1}\n{2}", String.Format(format, args), e.Message, e.StackTrace);
			} else {
				msg = MessageFormatUtility.FormatMsg(LogLevel.Error, "{0}: {1}", String.Format(format, args), e.Message);
			}

			if (ConsoleLogLevel.AtLeast(LogLevel.Error)) {
				ConsoleLog(msg);
				LogRemoteException(e, msg);
			}
		}

		public void Error (Exception e, string message)
		{
			if (string.IsNullOrEmpty(message)) {
				Error(e);
				return;
			}

			string msg = null;

			if (ConsoleLogLevel.AtLeast(LogLevel.Verbose)) {
				msg = MessageFormatUtility.FormatMsg(LogLevel.Error, "{0} : {1}\n{2}", message, e.Message, e.StackTrace);
			} else {
				msg = MessageFormatUtility.FormatMsg(LogLevel.Error, "{0} : {1}", message, e.Message);
			}

			if (ConsoleLogLevel.AtLeast(LogLevel.Error)) {
				ConsoleLog(msg);
				LogRemoteException(e, msg);
			}
		}

		#endregion
	}
}