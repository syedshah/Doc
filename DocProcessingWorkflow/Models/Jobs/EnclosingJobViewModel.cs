// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnclosingJobViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the EnclosingJobViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DocProcessingWorkflow.Models.Jobs
{
    using System.Text.RegularExpressions;

    public class EnclosingJobViewModel
    {
        public EnclosingJobViewModel()
        {

        }
        public EnclosingJobViewModel(EnclosingJob enclosingJobEntity)
        {
            this.EnclosingJobID = enclosingJobEntity.EnclosingJobID;
            this.FBE_ID = enclosingJobEntity.FBE_ID;
            this.JobID = enclosingJobEntity.JobID;
            this.MediDefID = enclosingJobEntity.MediaDefID;
            this.Packs = enclosingJobEntity.Packs;
            this.Pages = enclosingJobEntity.Pages;
            this.Sheets = enclosingJobEntity.Sheets;
            this.Filename = enclosingJobEntity.Filename;
            this.GRID = enclosingJobEntity.DigiQGRID;
            this.PostalDocketNumber = enclosingJobEntity.PostalDocketNumber;
        }


        public Int32 EnclosingJobID { get; set; }

        public Int32 FBE_ID { get; set; }

        public Int32 JobID { get; set; }

        public Int32 MediDefID { get; set; }

        public Int32? Packs { get; set; }

        public Int32? Pages { get; set; }

        public Int32? Sheets { get; set; }

        [StringLength(250)]
        public String Filename { get; set; }

        [StringLength(250)]
        public String GRID { get; set; }

        [StringLength(25)]
        [RegularExpression(@"[A-Za-z0-9]",ErrorMessage = "Accepts only aplhanumeric.")]
        public String PostalDocketNumber { get; set; }

        public Boolean PostalDocketNumberMissing
        {
            get
            {
                return String.IsNullOrWhiteSpace(this.PostalDocketNumber);
            }
        }

        public String Error { get; set; }
    }
}