using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DocProcessingWorkflow.Models.Environment;
using DocProcessingWorkflow.Models.ManCo;
using DocProcessingWorkflow.Models.ManCoDocs;

namespace DocProcessingWorkflow.Models.PromoteManCoDocs
{
    public class PromoteManCoDocViewModel
    {
        public PromoteManCoDocViewModel()
        {
            this.ManCos = new List<ManCoViewModel>();
            this.ManCoDocs = new List<ManCoDocViewModel>();
        }

        [Required(ErrorMessage = @"Please select ManCo.")]
        public String SelectedManCoCode { get; set; }

        public List<ManCoViewModel> ManCos { get; set; }

        public List<String> SourceEnvironments { get; set; }

        public List<String> TargetEnvironments { get; set; }

        [Required(ErrorMessage = @"Please select source AppEnvironment.")]
        public String SelectedSourceEnvironment { get; set; }

        [Required(ErrorMessage = @"Please select target AppEnvironment.")]
        public String SelectedTargetEnvironment { get; set; }

        public List<ManCoDocViewModel> ManCoDocs { get; set; }

        public List<Int32> AvailableManCoDocs { get; set; }

        [Required(ErrorMessage = @"Please select atleast one ManCoDoc.")]
        public List<Int32> SelectedManCoDocs { get; set; }

        public String Comment { get; set; }

        public void AddManCos(IList<Entities.ManagementCompany> manCos)
        {
            foreach (var mvm in manCos.Select(manCo => new ManCoViewModel(manCo)))
            {
                this.ManCos.Add(mvm);
            }
        }

        public void AddManCoDocs(IList<Entities.ManCoDoc> manCoDocs)
        {
            foreach (var mdvm in manCoDocs.Select(md => new ManCoDocViewModel(md)))
            {
                this.ManCoDocs.Add(mdvm);
            }
        }
    }
}