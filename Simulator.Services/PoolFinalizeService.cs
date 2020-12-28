using Simulator.Domain;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace Simulator.Services
{
    public class PoolFinalizeService : IPoolFinalizeService
    {
        public List<Team> NormalizeTeamsFromMatches(List<Match> playedMatches)
        {
            var allTeamMatches = new List<Team>();

            foreach (var teams in playedMatches.Select(x => new List<Team>() {x.FirstTeam, x.SecondTeam }).ToList())
            {
                allTeamMatches.AddRange(teams);
            }

            var normalizedTeams = allTeamMatches.DistinctBy(x => x.Id).ToList(); ;

            return normalizedTeams;
        }

        public PoolResult DeterminePoolResults(List<Match> playedMatches)
        {
            var poolResults = new PoolResult();
            var normalizedTeamsFromMatches = NormalizeTeamsFromMatches(playedMatches);

            List<Team> orderedTeamsForPoolResult = normalizedTeamsFromMatches
              .OrderByDescending(x => x.TotalPoints)
              .ThenByDescending(x => x.GoalDifference)
              .ThenByDescending(x => x.TotalGoalsFor)
              .ThenBy(x => x.TotalGoalsAgaint)
              .ToList();

            var teamsWithSameResult = SearchTeamWihSameResults(orderedTeamsForPoolResult);
            // if teams have the same result order by mutual match results
            if (teamsWithSameResult != null && teamsWithSameResult.Count > 0)
            {
                var matchesForTeamsWithSameResult = SearchMatchesForMutualResult(teamsWithSameResult, playedMatches);
                var matchWithbiggestDifference = GetMutualMatchWithBiggestDifference(matchesForTeamsWithSameResult);
                var lossingTeamFromMutualResult = GetLosingTeam(matchWithbiggestDifference);
                var winningTeamFromMutualResult = GetWinnigTeam(matchWithbiggestDifference);

                var currentLosingIndex = orderedTeamsForPoolResult.FindIndex(x => x.Id == lossingTeamFromMutualResult.Id);
                var currentWinnigIndex = orderedTeamsForPoolResult.FindIndex(x => x.Id == winningTeamFromMutualResult.Id);

                // Only if lossingteamIndex is lower, bump the winning team up in position 
                if (currentLosingIndex < currentWinnigIndex)
                {
                    orderedTeamsForPoolResult = orderedTeamsForPoolResult
                        .Move(
                            orderedTeamsForPoolResult.FindIndex(x => x.Id == winningTeamFromMutualResult.Id),
                            orderedTeamsForPoolResult.Count, 
                            orderedTeamsForPoolResult.FindIndex(x => x.Id == winningTeamFromMutualResult.Id) - 1).ToList();
                }
                
            }
            poolResults.Teams = orderedTeamsForPoolResult;
            return poolResults;
        }

        public List<Team> SearchTeamWihSameResults(List<Team> teams)
        {
            var mutualResults = new List<Team>();
            foreach (var team in teams)
            {
                if (teams.Exists(x => team.TotalPoints == x.TotalPoints && team.GoalDifference == x.GoalDifference && team.TotalGoalsFor == x.TotalGoalsFor && x.Id != team.Id))
                {
                    mutualResults.Add(team);
                }
            }

            return mutualResults;
        }

        public List<Match> SearchMatchesForMutualResult(List<Team> mutalResults, List<Match> matches)
        {
            var mutalMatchResults = new List<Match>();
            foreach (var match in matches)
            {
                if (mutalResults.Exists(x => x.Id == match.FirstTeam.Id) && mutalResults.Exists(x => x.Id == match.SecondTeam.Id))
                {
                    mutalMatchResults.Add(match);
                }
            }

            return mutalMatchResults;
        }

        public Match GetMutualMatchWithBiggestDifference(List<Match> mutualMatchResults)
        {
            // Get the match with the biggest difference
            var match = mutualMatchResults.Aggregate((i1, i2)
                => GetScoreDifference(i1.ScoreFirstTeam, i1.ScoreSecondTeam) > GetScoreDifference(i2.ScoreFirstTeam, i2.ScoreSecondTeam) ? i1 : i2);

            return match;
        }

        public int GetScoreDifference(int score1, int score2)
        {
            var difference = score1 - score2;

            return difference > 0 ? difference : -difference;
        }

        public Team GetWinnigTeam(Match match)
        {
            // Get the winning team
            var winnigTeam = match.ScoreFirstTeam > match.ScoreSecondTeam ? match.FirstTeam : match.SecondTeam;
            return winnigTeam;
        }

        public Team GetLosingTeam(Match match)
        {
            // Get the winning team
            var losingTeam = match.ScoreFirstTeam < match.ScoreSecondTeam ? match.FirstTeam : match.SecondTeam;
            return losingTeam;
        }
    }
}
