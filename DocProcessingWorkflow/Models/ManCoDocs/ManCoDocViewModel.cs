// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManCoDocViewModel.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   View model for mancodoc
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocProcessingWorkflow.Models.ManCoDocs
{
    public class ManCoDocViewModel
    {
        public ManCoDocViewModel(Entities.ManCoDoc manCoDoc)
        {
            this.MancoDocId = manCoDoc.ManCoDocID;
            this.ManCoId = manCoDoc.ManCoID;
            this.DocumentTypeId = manCoDoc.DocumentTypeID;
            this.ManCoDocName = manCoDoc.PubFileName;
        }

        public Int32 MancoDocId { get; set; }

        public Int32 ManCoId { get; set; }

        public Int32 DocumentTypeId { get; set; }

        public Int32 EnvironmentId { get; set; }

        public String ManCoDocName { get; set; }
    }
}