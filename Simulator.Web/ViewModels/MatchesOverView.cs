using Simulator.Domain;
using System.Collections.Generic;

namespace Simulator.Web.ViewModels
{
    public class MatchesOverView
    {
        public List<MatchViewModel> Matches { get; set; }

        public PoolOverView PoolOverView { get; set; }
}
}
