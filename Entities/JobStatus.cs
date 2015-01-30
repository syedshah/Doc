// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobStatus.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   JobStatu object 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class JobStatus
    {
        [Key]
      public Int32 JobStatusID { get; set; }

        public Int32? JobID { get; set; }

        public DateTime? DateTime { get; set; }

        public Int32? JobStatusTypeID { get; set; }

        public virtual Job Job { get; set; }

        public virtual JobStatusType JobStatusType { get; set; }
    }
}
