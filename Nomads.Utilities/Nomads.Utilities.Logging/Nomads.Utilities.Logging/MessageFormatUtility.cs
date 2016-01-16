using System;

namespace Nomads.Utilities.Logging
{
	public static class MessageFormatUtility
	{
		const string PRE_INFO = "INFO:: {0}";

		const string PRE_WARN = "WARNING:: {0}";

		const string PRE_ERROR = "ERROR:: {0}";

		const string PRE_DEBUG = "DEBUG:: {0}";

		const string PRE_REPORT = "REPORT:: {0}";

		const string SenderFormat = "[{0} line:{1}] [{2}]: {3}";

		public static string FormatMsg (LogLevel level, string format, params object[] args)
		{
			switch (level) {
			case LogLevel.None:
				return String.Empty;
			case LogLevel.Info:
				return String.Format(PRE_INFO, String.Format(format, args));
			case LogLevel.Warn:
				return String.Format(PRE_WARN, String.Format(format, args));
			case LogLevel.Error:
				return String.Format(PRE_ERROR, String.Format(format, args));
			case LogLevel.Debug:
				return String.Format(PRE_DEBUG, String.Format(format, args));
			case LogLevel.Report:
				return String.Format(PRE_REPORT, String.Format(format, args));
			default: //none
				return String.Empty;
			}
		}

		public static string FormatMsg (LogLevel level, string message)
		{
			switch (level) {
			case LogLevel.None:
				return String.Empty;
			case LogLevel.Info:
				return String.Format(PRE_INFO, message);
			case LogLevel.Warn:
				return String.Format(PRE_WARN, message);
			case LogLevel.Error:
				return String.Format(PRE_ERROR, message);
			case LogLevel.Debug:
				return String.Format(PRE_DEBUG, message);
			case LogLevel.Report:
				return String.Format(PRE_REPORT, message);
			default://none
				return String.Empty;
			}
		}
	}
}