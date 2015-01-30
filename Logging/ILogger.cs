// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogger.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the ILogger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Logging
{
	using System;

	public interface ILogger
	{
		void Info(String message);

		void Warn(String message);

		void Debug(String message);

		void Error(String message);

		void Error(String message, Exception ex);

		void Error(Exception x, String path, String url);

		void Fatal(String message);

		void Fatal(String message, Exception ex);

		void Fatal(Exception ex, String path, String url);
	}
}
