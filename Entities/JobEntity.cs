// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobEntity.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   JobEntity object
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Entities
{
    using System;

    public class JobEntity
    {
        public Int32 JobId { get; set; }

        public String JobReference { get; set; }

        public String Company { get; set; }

        public String Grid { get; set; }

        public String AllocatorGRID { get; set; }

        public String Document { get; set; }

        public String Version { get; set; }

        public String Owner { get; set; }

        public String SubmitDateTime { get; set; }

        public DateTime? FinishDate { get; set; }

        public String Status { get; set; }

        public Boolean? HoldAuthorisation { get; set; }

        public Boolean? Authorise { get; set; }

        public Boolean? ManualSubmissionOnly { get; set; }

        public String JobAuthorise { get; set; }

        public String Authoriser { get; set; }

        public Int32 EnvironmentId { get; set; }

        public Int32 ManCoDocID { get; set; }

        public Int32 DocumentTypeID { get; set; }

        public String DocTypeCode { get; set; }

        public String ManCoCode { get; set; }

        public String EnvType { get; set; }
    }
}
