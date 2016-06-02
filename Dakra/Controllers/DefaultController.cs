using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Dakra.Models;
using Dakra.Models.Default;
using KRD.RestApi;
using KRD.RestApi.TeamCity;
using KRD.RestApi.TeamCity.Entity;


namespace Dakra.Controllers
{
  public class DefaultController : Controller
  {
    private readonly TeamCityClient _teamCityClient;
    private readonly List<CodeCoverageModel> _codeCoverageList = new List<CodeCoverageModel>();

    public DefaultController()
    {
      _teamCityClient = TeamCityClient.Create("teamcity", 90, new LoginData("guest", "guest"));
    }

    // GET: Default
    public ActionResult Index()
    {
      var projectsList = _teamCityClient.Projects.GetAll().ToList();

      return View(projectsList);
    }

    public ActionResult ProjectParameters(string projectId)
    {
      ProjectDetails projectDetails = _teamCityClient.Projects.GetProjectDetailsById(projectId);

      var codeCoverageModel = new CodeCoverageModel()
      {
        ProjectName = projectDetails.Name,
        Branches = projectDetails.ConfigurationsList,
        Before = string.Empty,
        After = string.Empty
      };

      return View("Statistics", codeCoverageModel);
    }

    public ActionResult ShowBuilds(CodeCoverageModel codeCoverageModel)
    {
      // codeCoverageModel.SelectedBranch = projectConfiguration;
      codeCoverageModel.BuildsCollection = 
        _teamCityClient
        .Projects.BuildTypes.Builds
        .GetBuildsCollection(codeCoverageModel.ProjectName, codeCoverageModel.SelectedBranch.Name, BuildStatus.SUCCESS, null, 2, 1);

      return View("Statistics", codeCoverageModel);
    }

    public ActionResult ShowStatistics(CodeCoverageModel codeCoverageModel)
    {
      var statisticsBefore = _teamCityClient.Projects.BuildTypes.Builds.GetBuildStatisticsById(codeCoverageModel.SelectedBuildBeforeId);
      var statisticsAfter = _teamCityClient.Projects.BuildTypes.Builds.GetBuildStatisticsById(codeCoverageModel.SelectedBuildAfterId);
      
      if (statisticsBefore != null)
      {
        codeCoverageModel.Before = statisticsBefore.Property.Where(x => x.Name == "CodeCoverageS").Select(y => y.Value).FirstOrDefault();
      }

      if (statisticsAfter != null)
      {
        codeCoverageModel.After = statisticsAfter.Property.Where(x => x.Name == "CodeCoverageS").Select(y => y.Value).FirstOrDefault();
      }

      return View("Statistics", codeCoverageModel);
    }
  }
}