using Simulator.Domain;
using Simulator.Web.ViewModels;
using System.Linq;

namespace Simulator.Web.Mapper
{
    public class TeamModelMapper : ITeamModelMapper
    {
        private IPlayerModelMapper _playerModelMapper;

        public TeamModelMapper(IPlayerModelMapper playerModelMapper)
        {
            _playerModelMapper = playerModelMapper;
        }

        public TeamViewModel MapToTeamViewModel(Team team)
        {
            var teamViewModel = new TeamViewModel()
            {
                Id = team.Id,
                Name = team.Name,
                TotalGoalsAgaint = team.TotalGoalsAgaint,
                TotalGoalsFor = team.TotalGoalsFor,
                TotalPoints = team.TotalPoints
            };

            if (team.Players.Any())
            {
                teamViewModel.Players = new System.Collections.Generic.List<PlayerViewModel>();
                teamViewModel.Players = team.Players.Select(x =>_playerModelMapper.MapToPlayerViewModel(x)).ToList();
            }

            return teamViewModel;
        }
    }
}
