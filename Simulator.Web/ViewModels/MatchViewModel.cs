using System;
using System.Collections.Generic;

namespace Simulator.Web.ViewModels
{
    public class MatchViewModel
    {
        public Guid MatchModelId { get; set; }   
        public TeamViewModel FirstTeam { get; set; }
        public TeamViewModel SecondTeam { get; set; }
        public int FirstTeamScore { get; set; }
        public int SecondTeamScore { get; set; }

        public List<TeamViewModel> TeamsForMatch { get; set; }
    }

}
