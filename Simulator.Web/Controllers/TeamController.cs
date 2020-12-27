using Microsoft.AspNetCore.Mvc;
using Simulator.Services;
using Simulator.Web.Mapper;
using Simulator.Web.ViewModels;
using System.Linq;

namespace Simulator.Web.Controllers
{
    public class TeamController : Controller
    {
        private ITeamGeneratorService _teamGeneratorService;
        
        private ITeamModelMapper _teamModelMapper;

        public TeamController(
            ITeamGeneratorService teamGeneratorService,
            ITeamModelMapper teamModelMapper)
        {
            _teamGeneratorService = teamGeneratorService;
            _teamModelMapper = teamModelMapper;
        }

        public IActionResult GetTeams()
        {
            var teams = _teamGeneratorService.GenerateFixedTeams();
            var teamOverview = new TeamOverView();
            teamOverview.Teams = teams.Select(x => _teamModelMapper.MapToTeamViewModel(x)).ToList();
            
            return PartialView("Team/_teamOverView", teamOverview);
        }
    }
}
