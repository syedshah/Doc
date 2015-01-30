// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the ErrorViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DocProcessingWorkflow.Models
{
	using System;

	public enum ErrorSeverity
	{
		Error,

		Warning,

		Information
	}

	public enum ErrorCode
	{
		Unknown,

		GetMeeting,

		GetMeetingList,

		CreateMeeting,

		ReferenceData,

		AddMeetingAlternateOrganizers,

		DeleteMeetingAlternateOrganizers,

		ChangeMeetingPrimaryOrganizer,

		AddMeetingParticipants,

		DeleteMeetingParticipants,

		DeleteMeeting
	}

	public class ErrorViewModel
	{
		public ErrorCode ErrorCode { get; set; }

		public ErrorSeverity Severity { get; set; }

		public String ErrorMessage { get; set; }

		public String DisplayMessage { get; set; }
	}
}