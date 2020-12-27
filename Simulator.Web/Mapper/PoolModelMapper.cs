using Simulator.Domain;
using Simulator.Web.ViewModels;
using System.Collections.Generic;

namespace Simulator.Web.Mapper
{
    public class PoolModelMapper : IPoolModelMapper
    {
        private ITeamModelMapper _teamOverViewMapper;

        public PoolModelMapper(ITeamModelMapper teamOverViewMapper)
        {
            _teamOverViewMapper = teamOverViewMapper;
        }

        public PoolOverView MapToPoolOverView(List<Team> teams)
        {
            var poolOverView = new PoolOverView();
            foreach (var team in teams)
            {
                poolOverView.Teams.Add(_teamOverViewMapper.MapToTeamViewModel(team));
            }
           
            return poolOverView;
        }


    }
}
