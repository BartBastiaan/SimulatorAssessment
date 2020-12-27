using Simulator.Domain;
using System.Collections.Generic;

namespace Simulator.Services
{
    public class MatchGeneratorService : IMatchGeneratorService
    {

        public List<Match> GenerateMatches(List<Team> teams)
        {
            var teamMatches = new List<Match>();
            for (int i = 0; i < teams.Count; i++)
            {
                for (int y = i ; y < teams.Count - 1; y++)
                {
                    var match = new Match(teams[i], teams[y+1]);
                    teamMatches.Add(match);
                }
            }
            return teamMatches;
        }
    }
}
