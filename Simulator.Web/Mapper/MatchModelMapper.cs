using MoreLinq;
using Simulator.Domain;
using Simulator.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Simulator.Web.Mapper
{
    public class MatchModelMapper : IMatchOverViewMapper
    {
        private ITeamModelMapper _teamOverViewMapper;

        public MatchModelMapper(ITeamModelMapper teamOverViewMapper)
        {
            _teamOverViewMapper = teamOverViewMapper;
        }

        public MatchesOverView MapToMatchesOverView(List<Match> matches)
        {
            var overView = new MatchesOverView();
            overView.Matches = MapToMatchViewModel(matches);
            overView.PoolOverView = new PoolOverView();
 
            return overView;
        }

        public List<MatchViewModel> MapToMatchViewModel(List<Match> matches)
        {
            var matchesViewModel = new List<MatchViewModel>();
            matchesViewModel.AddRange(matches.Select(x => new MatchViewModel{
                MatchModelId = x.MatchId,
                FirstTeam = _teamOverViewMapper.MapToTeamViewModel(x.FirstTeam),
                SecondTeam = _teamOverViewMapper.MapToTeamViewModel(x.SecondTeam),
                FirstTeamScore = x.ScoreFirstTeam,
                SecondTeamScore = x.ScoreSecondTeam
            }));
            return matchesViewModel;
        }


    }
}
