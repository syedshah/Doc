// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnclosingJob.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   EnclosingJob object 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EnclosingJob")]
    public partial class EnclosingJob
    {
        public EnclosingJob()
        {
            MaterialConsumptions = new HashSet<MaterialConsumption>();
        }

        public Int32 EnclosingJobID { get; set; }

        public Int32 FBE_ID { get; set; }

        public Int32 JobID { get; set; }

        public Int32 MediaDefID { get; set; }

        public Int32? Packs { get; set; }

        public Int32? Pages { get; set; }

        public Int32? Sheets { get; set; }

        [StringLength(250)]
        public String Filename { get; set; }

        [StringLength(250)]
        public String DigiQGRID { get; set; }

        [StringLength(25)]
        public String PostalDocketNumber { get; set; }

        public virtual Job Job { get; set; }

        public virtual MediaDefinition MediaDefinition { get; set; }

        public virtual ICollection<MaterialConsumption> MaterialConsumptions { get; set; }
    }
}
