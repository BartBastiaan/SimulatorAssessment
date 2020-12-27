using Microsoft.AspNetCore.Mvc;
using Simulator.Domain;
using Simulator.Services;
using Simulator.Services.Factories;
using Simulator.Web.Mapper;
using Simulator.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Simulator.Web.Controllers
{
    public class MatchController : Controller
    {
        private IMatchSimulatorService _matchSimulatorService;
        private IMatchGeneratorService _matchGeneratorService;
        private ITeamGeneratorService _teamGeneratorService;
        private IPoolFinalizeService _poolFinalizeService;
        private ITeamFactory _teamFactory;
        private IPlayerFactory _playerFactory;
        private IMatchOverViewMapper _matchOverViewMapper;
        private IPoolModelMapper _PoolOverViewMapper;

        public MatchController(
            IMatchSimulatorService matchSimulatorService,
             IMatchGeneratorService matchGeneratorService,
             ITeamGeneratorService teamGeneratorService,
             IPoolFinalizeService poolFinalizeService,
             ITeamFactory teamFactory,
            IPlayerFactory playerFactory,
             IMatchOverViewMapper matchOverViewMapper,
             IPoolModelMapper PoolOverViewMapper)
        {
            _matchSimulatorService = matchSimulatorService;
            _matchGeneratorService = matchGeneratorService;
            _teamGeneratorService = teamGeneratorService;
            _poolFinalizeService = poolFinalizeService;
            _teamFactory = teamFactory;
            _playerFactory = playerFactory;
            _matchOverViewMapper = matchOverViewMapper;
            _PoolOverViewMapper = PoolOverViewMapper;
        }

        public IActionResult StartFixedMatches()
        {
            var teams = _teamGeneratorService.GenerateFixedTeams();
            var matches = _matchGeneratorService.GenerateMatches(teams);
            // Add secend set of matches
            matches.AddRange(_matchGeneratorService.GenerateMatches(teams));

            matches = matches.Select(x => _matchSimulatorService.RunSimulation(x)).ToList();
            var viewmodel = _matchOverViewMapper.MapToMatchesOverView(matches);
            viewmodel.PoolOverView = _PoolOverViewMapper.MapToPoolOverView(_poolFinalizeService.DeterminePoolResults(matches).Teams);

            return PartialView("_matchesOverView", viewmodel);
        }

        public IActionResult StartCustomMatches(TeamOverView teamoverview)
        {
            List<Team> teams = new List<Team>();
            foreach (var team in teamoverview.Teams)
            {
                var tempTeamp = _teamFactory.CreateTeam(team.Name);
                tempTeamp.Players = team.Players.Select(x => _playerFactory.CreatePlayer(tempTeamp.Id, x.Posistion, x.Attack, x.Defense)).ToList();

                teams.Add(tempTeamp);
            }
            
            //var teams = _teamGeneratorService.GenerateFixedTeams();
            var matches = _matchGeneratorService.GenerateMatches(teams);
            // Add secend set of matches
            matches.AddRange(_matchGeneratorService.GenerateMatches(teams));

            matches = matches.Select(x => _matchSimulatorService.RunSimulation(x)).ToList();
            var viewmodel = _matchOverViewMapper.MapToMatchesOverView(matches);
            viewmodel.PoolOverView = _PoolOverViewMapper.MapToPoolOverView(_poolFinalizeService.DeterminePoolResults(matches).Teams);

            return PartialView("_matchesOverView", viewmodel);
        }
    }
}
