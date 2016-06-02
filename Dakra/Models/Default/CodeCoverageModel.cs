using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KRD.RestApi.TeamCity.Entity;

namespace Dakra.Models.Default
{
  public class CodeCoverageModel
  {
    public string ProjectName { get; set; }

    public ProjectConfigurationsList Branches { get; set; } 

    public ProjectConfiguration SelectedBranch { get; set; }

    public string SelectedBuildBeforeId { get; set; }

    public string SelectedBuildAfterId { get; set; }

    public BuildsCollection BuildsCollection { get; set; }

    public string Before { get; set; }

    public string After { get; set; }
  }
}