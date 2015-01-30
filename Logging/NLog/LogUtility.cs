// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogUtility.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the LogUtility type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Logging.NLog
{
	using System;

	public static class LogUtility
	{
		public static String BuildExceptionMessage(this Exception exception, String path, String url)
		{
			String strErrorMsg = "Error in Path :" + path;
			strErrorMsg += Environment.NewLine + "Raw Url :" + url;

			Exception logException = exception;

			while (logException != null)
			{
				// Get the error message
				strErrorMsg += Environment.NewLine + "Message :" + logException.Message;

				// Source of the message
				strErrorMsg += Environment.NewLine + "Source :" + logException.Source;

				// Stack Trace of the error
				strErrorMsg += Environment.NewLine + "Stack Trace :" + logException.StackTrace;

				// Method where the error occurred
				strErrorMsg += Environment.NewLine + "TargetSite :" + logException.TargetSite;

				logException = logException.InnerException;
				if (logException != null)
				{
					strErrorMsg += Environment.NewLine + Environment.NewLine + "    ==== Inner Exception" + Environment.NewLine;
				}
			}

			return strErrorMsg;
		}
	}
}
