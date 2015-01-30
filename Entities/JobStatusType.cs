// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JobStatusType.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   JobStatusType object 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("JobStatusType")]
    public partial class JobStatusType
    {
        public JobStatusType()
        {
            JobStatus = new HashSet<JobStatus>();
        }

        public Int32 JobStatusTypeID { get; set; }

        [Required]
        [StringLength(20)]
        public String JobStatusDescription { get; set; }

        public virtual ICollection<JobStatus> JobStatus { get; set; }
    }
}
