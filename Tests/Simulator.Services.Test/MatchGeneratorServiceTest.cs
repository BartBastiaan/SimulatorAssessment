using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simulator.Domain;
using System;
using System.Collections.Generic;

namespace Simulator.Services.Test
{
    [TestClass]
    public class MatchGeneratorServiceTest
    {
        public MatchGeneratorService CreateSut()
        {
            return new MatchGeneratorService();
        }

        [TestMethod]
        public void GenerateMatches_Should_Return_ListOfMatches_Based_onTeams()
        {
            var teamA = new Team(Guid.NewGuid(), "A");
            var teamB = new Team(Guid.NewGuid(), "B");
            var teamC = new Team(Guid.NewGuid(), "C");
            var teamD = new Team(Guid.NewGuid(), "D");

            var teams = new List<Team>() { teamA, teamB, teamC, teamD };

            var sut = CreateSut();

            var matches = sut.GenerateMatches(teams);

            var allTeamMatches = new List<Team>();
            foreach (var match in matches)
            {
                allTeamMatches.Add(match.FirstTeam);
                allTeamMatches.Add(match.SecondTeam);
            }

            Assert.AreEqual(6, matches.Count);
        }
    }
}
