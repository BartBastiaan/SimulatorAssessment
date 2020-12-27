using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simulator.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulator.Services.Test
{
    [TestClass]
    public class PoolFinalizeServiceTest
    {
        private Team _teamA = new Team( Guid.NewGuid(), "TeamA" );
        private Team _teamB = new Team( Guid.NewGuid(), "TeamB" );
        private Team _teamC = new Team( Guid.NewGuid(), "TeamC" );
        private Team _teamD = new Team( Guid.NewGuid(), "TeamD" );
        private IEnumerable<string> _expectedAtoD = new[]
            {
                "TeamA",
                "TeamB",
                "TeamC",
                "TeamD"
            };
        private IEnumerable<string> _expectedDtoA = new[]
        {
                 "TeamD",
                "TeamC",
                "TeamB",
                "TeamA"
        };

        private PoolFinalizeService CreateSut()
        {
            return new PoolFinalizeService();
        }

        private Match CreateMatch(Team team1, Team team2, int team1Score = 0, int team2Score = 0)
        {
            var match = new Match(team1, team2);
            match.ScoreFirstTeam = team1Score;
            match.ScoreSecondTeam = team2Score;
            return match;
        }

        private List<Match> CreateMatches()
        {
            var matches = new List<Match>();
            var match1 = CreateMatch(_teamA, _teamB);
            var match2 = CreateMatch(_teamA, _teamC);
            var match3 = CreateMatch(_teamA, _teamD);
            var match4 = CreateMatch(_teamB, _teamC);
            var match5 = CreateMatch(_teamB, _teamD);
            var match6 = CreateMatch(_teamC, _teamD, 2, 1);

            // add re-machtes for mutual result check
            var match7 = CreateMatch(_teamA, _teamB);
            var match8 = CreateMatch(_teamA, _teamC);
            var match9 = CreateMatch(_teamA, _teamD);
            var match10 = CreateMatch(_teamB, _teamC, 0, 3);
            var match11 = CreateMatch(_teamB, _teamD, 0, 2);
            var match12 = CreateMatch(_teamC, _teamD, 0, 2);

            matches.Add(match1);
            matches.Add(match2);
            matches.Add(match3);
            matches.Add(match4);
            matches.Add(match5);
            matches.Add(match6);

            matches.Add(match7);
            matches.Add(match8);
            matches.Add(match9);
            matches.Add(match10);
            matches.Add(match11);
            matches.Add(match12);
            return matches;
        }

        [TestMethod]
        public void DeterminePoolResults_Should_Return_OrderBy_Points_A_To_D()
        {
            var sut = CreateSut();
            List<Match> matches = null;
            _teamA.AddToTotalPoints(4);
            _teamB.AddToTotalPoints(3);
            _teamC.AddToTotalPoints(2);
            _teamD.AddToTotalPoints(1);

            matches = CreateMatches();

            var result = sut.DeterminePoolResults(matches);

            Assert.IsTrue(result.Teams.Select(x => x.Name).SequenceEqual(_expectedAtoD));
        }

        [TestMethod]
        public void DeterminePoolResults_Should_Return_OrderBy_Points_D_To_A()
        {
            var sut = CreateSut();
            List<Match> matches = null;
            _teamA.AddToTotalPoints(1);
            _teamB.AddToTotalPoints(2);
            _teamC.AddToTotalPoints(3);
            _teamD.AddToTotalPoints(4);

            matches = CreateMatches();

            var result = sut.DeterminePoolResults(matches);

            Assert.IsTrue(result.Teams.Select(x => x.Name).SequenceEqual(_expectedDtoA));
        }

        [TestMethod]
        public void DeterminePoolResults_Should_Return_OrderBy_GoalDiff_A_To_D()
        {
            var sut = CreateSut();
            List<Match> matches = null;
            _teamA.AddToTotalPoints(3);
            _teamA.TotalGoalsFor = 4;
            _teamA.TotalGoalsAgaint = 0;

            _teamB.AddToTotalPoints(3);
            _teamB.TotalGoalsFor = 3;
            _teamB.TotalGoalsAgaint = 0;

            _teamC.AddToTotalPoints(3);
            _teamC.TotalGoalsFor = 2;
            _teamC.TotalGoalsAgaint = 0;

            _teamD.AddToTotalPoints(3);
            _teamD.TotalGoalsFor = 1;
            _teamD.TotalGoalsAgaint = 0;

            matches = CreateMatches();

            var result = sut.DeterminePoolResults(matches);

            Assert.IsTrue(result.Teams.Select(x => x.Name).SequenceEqual(_expectedAtoD));
        }

        [TestMethod]
        public void DeterminePoolResults_Should_Return_OrderBy_GoalDiff_D_To_A()
        {
            var sut = CreateSut();
            List<Match> matches = null;
            _teamA.AddToTotalPoints(3);
            _teamA.TotalGoalsFor = 1;
            _teamA.TotalGoalsAgaint = 0;

            _teamB.AddToTotalPoints(3);
            _teamB.TotalGoalsFor = 2;
            _teamB.TotalGoalsAgaint = 0;

            _teamC.AddToTotalPoints(3);
            _teamC.TotalGoalsFor = 3;
            _teamC.TotalGoalsAgaint = 0;

            _teamD.AddToTotalPoints(3);
            _teamD.TotalGoalsFor = 4;
            _teamD.TotalGoalsAgaint = 0;

            matches = CreateMatches();

            var result = sut.DeterminePoolResults(matches);

            Assert.IsTrue(result.Teams.Select(x => x.Name).SequenceEqual(_expectedDtoA));
        }

        [TestMethod]
        public void DeterminePoolResults_Should_Return_OrderBy_GoalsFor_A_To_D()
        {
            var sut = CreateSut();
            List<Match> matches = null;
            _teamA.AddToTotalPoints(6);
            _teamA.TotalGoalsFor = 10;
            _teamA.TotalGoalsAgaint = 8;

            _teamB.AddToTotalPoints(6);
            _teamB.TotalGoalsFor = 8;
            _teamB.TotalGoalsAgaint = 6;

            _teamC.AddToTotalPoints(6);
            _teamC.TotalGoalsFor = 6;
            _teamC.TotalGoalsAgaint = 4;

            _teamD.AddToTotalPoints(6);
            _teamD.TotalGoalsFor = 4;
            _teamD.TotalGoalsAgaint = 2;

            matches = CreateMatches();

            var result = sut.DeterminePoolResults(matches);

            Assert.IsTrue(result.Teams.Select(x => x.Name).SequenceEqual(_expectedAtoD));
        }

        [TestMethod]
        public void DeterminePoolResults_Should_Return_OrderBy_GoalsFor_D_To_A()
        {
            var sut = CreateSut();
            _teamA.AddToTotalPoints(6);
            _teamA.TotalGoalsFor = 4;
            _teamA.TotalGoalsAgaint = 2;

            _teamB.AddToTotalPoints(6);
            _teamB.TotalGoalsFor = 6;
            _teamB.TotalGoalsAgaint = 4;

            _teamC.AddToTotalPoints(6);
            _teamC.TotalGoalsFor = 8;
            _teamC.TotalGoalsAgaint = 6;

            _teamD.AddToTotalPoints(6);
            _teamD.TotalGoalsFor = 10;
            _teamD.TotalGoalsAgaint = 8;

            var matches = CreateMatches();

            var result = sut.DeterminePoolResults(matches);

            Assert.IsTrue(result.Teams.Select(x => x.Name).SequenceEqual(_expectedDtoA));
        }

        
        [TestMethod]
        public void NormalizeTeamsFromMatches_Should_Bring_Total_TeamsFromMatches_Back_To_Four()
        {
            var sut = CreateSut();
            var teams = new List<Team>();
            _teamA.AddToTotalPoints(9);
            _teamA.TotalGoalsFor = 10;
            _teamA.TotalGoalsAgaint = 2;

            _teamB.AddToTotalPoints(9);
            _teamB.TotalGoalsFor = 6;
            _teamB.TotalGoalsAgaint = 2;

            _teamC.AddToTotalPoints(3);
            _teamC.TotalGoalsFor = 2;
            _teamC.TotalGoalsAgaint = 1;

            _teamD.AddToTotalPoints(3);
            _teamD.TotalGoalsFor = 2;
            _teamD.TotalGoalsAgaint = 1;

            teams.Add(_teamA);
            teams.Add(_teamB);
            teams.Add(_teamC);
            teams.Add(_teamD);
            var matches = CreateMatches();

            var result = sut.NormalizeTeamsFromMatches(matches);

            Assert.AreEqual(4, result.Count);
        }

        [TestMethod]
        public void SearchTeamWihSameResults_Should_Return_Teams_With_The_Same_Results()
        {
            var sut = CreateSut();
            var teams = new List<Team>();
            _teamA.AddToTotalPoints(9);
            _teamA.TotalGoalsFor = 10;
            _teamA.TotalGoalsAgaint = 2;

            _teamB.AddToTotalPoints(9);
            _teamB.TotalGoalsFor = 6;
            _teamB.TotalGoalsAgaint = 2;

            _teamC.AddToTotalPoints(3);
            _teamC.TotalGoalsFor = 2;
            _teamC.TotalGoalsAgaint = 1;

            _teamD.AddToTotalPoints(3);
            _teamD.TotalGoalsFor = 2;
            _teamD.TotalGoalsAgaint = 1;

            teams.Add(_teamA);
            teams.Add(_teamB);
            teams.Add(_teamC);
            teams.Add(_teamD);
            var matches = CreateMatches();

            var result = sut.SearchTeamWihSameResults(teams);
            IEnumerable<string> expected = new[]
            {
                "TeamC",
                "TeamD"
            };

            Assert.IsTrue(result.Select(x => x.Name).SequenceEqual(expected));
        }



        [TestMethod]
        public void GetMutualMatchWithBiggestDifference_Should_Return_The_Match_With_Biggest_Score_Difference()
        {
            var sut = CreateSut();
            var matches = new List<Match>();

            var match1 = CreateMatch(_teamA, _teamB, 2, 1);
            var match2 = CreateMatch(_teamA, _teamB, 0, 3);

            matches.Add(match1);
            matches.Add(match2);

            var result = sut.GetMutualMatchWithBiggestDifference(matches);

            Assert.AreEqual(match2.MatchId, result.MatchId);
            Assert.AreEqual(3, result.ScoreSecondTeam);
            Assert.AreEqual(0, result.ScoreFirstTeam);
        }
        

        [TestMethod]
        public void DeterminePoolResults_Should_Return_OrderBy_mutualResult_A_To_D()
        {
            var sut = CreateSut();
            var teams = new List<Team>();
            _teamA.AddToTotalPoints(9);
            _teamA.TotalGoalsFor = 10;
            _teamA.TotalGoalsAgaint = 2;

            _teamB.AddToTotalPoints(9);
            _teamB.TotalGoalsFor = 6;
            _teamB.TotalGoalsAgaint = 2;

            _teamC.AddToTotalPoints(3);
            _teamC.TotalGoalsFor = 2;
            _teamC.TotalGoalsAgaint = 1;

            _teamD.AddToTotalPoints(3);
            _teamD.TotalGoalsFor = 2;
            _teamD.TotalGoalsAgaint = 1;

            teams.Add(_teamA);
            teams.Add(_teamB);
            teams.Add(_teamC);
            teams.Add(_teamD);
            var matches = CreateMatches();

            var result = sut.DeterminePoolResults(matches);
            IEnumerable<string> expected = new[]
            {
                "TeamA",
                "TeamB",
                "TeamD",
                "TeamC"
            };

            Assert.IsTrue(result.Teams.Select(x => x.Name).SequenceEqual(expected));
        }
    }
}
