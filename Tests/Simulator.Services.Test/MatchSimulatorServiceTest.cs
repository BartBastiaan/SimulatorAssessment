using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simulator.Domain;
using System;
using System.Collections.Generic;

namespace Simulator.Services.Test
{
    [TestClass]
    public class MatchSimulatorServiceTest
    {
    
        public MatchSimulatorService CreateSut()
        {

            return new MatchSimulatorService();
        }

        private List<Player> CreatePlayer(Guid teamId, int attack, int defense, int amountBack, int amountMid, int amountFront)
        {
            var players = new List<Player>();

            // Add goalKeeper
            players.Add(new Player(Guid.NewGuid(), teamId, PlayerPosistionEnum.GoalKeeper, attack, defense));

            // Add three back players
            for (int i = 0; i < amountBack; i++)
            {
                var backPlayer = new Player(Guid.NewGuid(), teamId, PlayerPosistionEnum.Back, attack, defense);
            players.Add(backPlayer);
            }

            // Add three mid players
            for (int i = 0; i < amountMid; i++)
            {
                var midPlayer = new Player(Guid.NewGuid(), teamId, PlayerPosistionEnum.Mid, attack, defense);
            players.Add(midPlayer);
            }

            // Add four front players
            for (int i = 0; i < amountFront; i++)
            {
                var frontPlayer = new Player(Guid.NewGuid(), teamId, PlayerPosistionEnum.Front, attack, defense);
                players.Add(frontPlayer);
            }

            return players;
        }

        [TestMethod]
        public void RunSimulation_TeamA_Should_Win_With_Four_Goals_IfDefence_Other_Team_Is_0()
        {
            var sut = CreateSut();
            var winningTeam = new Team(Guid.NewGuid(), "A");
            winningTeam.Players = CreatePlayer(winningTeam.Id, 6, 6, 5, 3 , 2);
            
            
            var loosingTeam = new Team(Guid.NewGuid(), "B");
            loosingTeam.Players = CreatePlayer(loosingTeam.Id, 0, 0, 5, 3, 2);

            var match1 = new Domain.Match(winningTeam, loosingTeam);

            var matchResults = sut.RunSimulation(match1);

            Assert.AreEqual(14, matchResults.ScoreFirstTeam);
            Assert.AreEqual(0, matchResults.ScoreSecondTeam);
        }

        [TestMethod]
        public void CalculateAttackingPoinstAtBallPosition_Should_Return_AttackPoints_BackPosition()
        {
            var sut = CreateSut();
            var team = new Team(Guid.NewGuid(), "A");
            team.Players = CreatePlayer(team.Id, 6, 6, 5, 3, 2);
            
            var totalPlayerScore = sut.CalculateAttackingPoinstAtBallPosition(team, BallPositionEnum.Back);

            // team has 5 back players * 6 attack total should be 30
            Assert.AreEqual(30, totalPlayerScore);
        }

        [TestMethod]
        public void CalculateAttackingPoinstAtBallPosition_Should_Return_AttackPoints_MidPosition()
        {
            var sut = CreateSut();
            var team = new Team(Guid.NewGuid(), "A");
            team.Players = CreatePlayer(team.Id, 6, 6, 5, 3, 2);

            var totalPlayerScore = sut.CalculateAttackingPoinstAtBallPosition(team, BallPositionEnum.Mid);

            // team has 3 mid players * 6 attack total should be 18
            Assert.AreEqual(18, totalPlayerScore);
        }

        [TestMethod]
        public void CalculateAttackingPoinstAtBallPosition_Should_Return_AttackPoints_FrontPosition()
        {
            var sut = CreateSut();
            var team = new Team(Guid.NewGuid(), "A");
            team.Players = CreatePlayer(team.Id, 6, 6, 5, 3, 2);

            var totalPlayerScore = sut.CalculateAttackingPoinstAtBallPosition(team, BallPositionEnum.Front);

            // team has 2 front players * 6 attack total should be 12
            Assert.AreEqual(12, totalPlayerScore);
        }


        [TestMethod]
        public void CalculateDefendingPoinstAtBallPosition_Should_Return_DefensePoints_FrontPosition()
        {
            var sut = CreateSut();
            var team = new Team(Guid.NewGuid(), "A");
            team.Players = CreatePlayer(team.Id, 6, 1, 5, 3, 2);

            var totalPlayerScore = sut.CalculateDefendingPoinstAtBallPosition(team, BallPositionEnum.Back,false);

            // team has 2 back players * 1 attack total should be 2
            Assert.AreEqual(2, totalPlayerScore);
        }

        [TestMethod]
        public void CalculateDefendingPoinstAtBallPosition_Should_Return_DefensePoints_MidPosition()
        {
            var sut = CreateSut();
            var team = new Team(Guid.NewGuid(), "A");
            team.Players = CreatePlayer(team.Id, 6, 1, 5, 3, 2);

            var totalPlayerScore = sut.CalculateDefendingPoinstAtBallPosition(team, BallPositionEnum.Mid, false);

            // team has 3 mid players * 1 defense total should be 3
            Assert.AreEqual(3, totalPlayerScore);
        }

        [TestMethod]
        public void CalculateDefendingPoinstAtBallPosition_Should_Return_DefensePoints_BackPosition()
        {
            var sut = CreateSut();
            var team = new Team(Guid.NewGuid(), "A");
            team.Players = CreatePlayer(team.Id, 6, 1, 5, 3, 2);

            var totalPlayerScore = sut.CalculateDefendingPoinstAtBallPosition(team, BallPositionEnum.Front, false);

            // team has 5 front players * 1 defense total should be 5
            Assert.AreEqual(5, totalPlayerScore);
        }

        [TestMethod]
        public void CalculateDefendingPoinstAtBallPosition_Should_Return_DefensePoints_BackPosition_Include_KeeperPoints()
        {
            var sut = CreateSut();
            var team = new Team(Guid.NewGuid(), "A");
            team.Players = CreatePlayer(team.Id, 6, 1, 5, 3, 2);

            var totalPlayerScore = sut.CalculateDefendingPoinstAtBallPosition(team, BallPositionEnum.Front, true);

            // team has 5 front players * 1 defense + Keeper total should be 6
            Assert.AreEqual(6, totalPlayerScore);
        }

        [TestMethod]
        public void SetBallPositionForDefense_Should_Invert_BallPositionForDefence_When_Front()
        {
            var sut = CreateSut();

            var ballposition = sut.InvertBallPosition(BallPositionEnum.Front);

            Assert.AreEqual(BallPositionEnum.Back, ballposition);
        }

        [TestMethod]
        public void SetBallPositionForDefense_Should_Invert_BallPositionForDefence_When_Back()
        {
            var sut = CreateSut();

            var ballposition = sut.InvertBallPosition(BallPositionEnum.Back);

            Assert.AreEqual(BallPositionEnum.Front, ballposition);
        }

        [TestMethod]
        public void SetBallPositionForDefense_Should_Not_Invert_When_Mid()
        {
            var sut = CreateSut();

            var ballposition = sut.InvertBallPosition(BallPositionEnum.Mid);

            Assert.AreEqual(BallPositionEnum.Mid, ballposition);
        }

        [TestMethod]
        public void GetPlayersAtPosition_Should_Return_Amopunt_Of_Player_At_BallPosition_Back()
        {
            var sut = CreateSut();
            var team = new Team(Guid.NewGuid(), "A");
            team.Players = CreatePlayer(team.Id, 6, 1, 5, 3, 2);

            var players = sut.GetPlayersAtPosition(team, BallPositionEnum.Back);

            Assert.AreEqual(5, players.Count);
        }

        [TestMethod]
        public void GetPlayersAtPosition_Should_Return_Amopunt_Of_Player_At_BallPosition_Mid()
        {
            var sut = CreateSut();
            var team = new Team(Guid.NewGuid(), "A");
            team.Players = CreatePlayer(team.Id, 6, 1, 5, 3, 2);

            var players = sut.GetPlayersAtPosition(team, BallPositionEnum.Mid);

            Assert.AreEqual(3, players.Count);
        }

        [TestMethod]
        public void GetPlayersAtPosition_Should_Return_Amount_Of_Player_At_BallPosition_Front()
        {
            var sut = CreateSut();
            var team = new Team(Guid.NewGuid(), "A");
            team.Players = CreatePlayer(team.Id, 6, 1, 5, 3, 2);

            var players = sut.GetPlayersAtPosition(team, BallPositionEnum.Front);

            Assert.AreEqual(2, players.Count);
        }

        [TestMethod]
        public void GenerateNumber_Should_Always_Return_Number_Between_MinAndsMax()
        {
            var sut = CreateSut();

            for (int i =0; i<10; i++)
            {
                var number = sut.GenerateNumber(10, 0);

                Assert.IsTrue(number >= 0);
                Assert.IsTrue(number <= 10);
            }       
        }

        [TestMethod]
        public void GenerateNumber_Should_Always_Return_Same_Number_When_Min_And_Max_Are_The_Same()
        {
            var sut = CreateSut();

            for (int i = 0; i < 10; i++)
            {
                var number = sut.GenerateNumber(10, 10);

                Assert.AreEqual(10, number);
            }
        }

        [TestMethod]
        public void GenerateNumber_Should()
        {
            var sut = CreateSut();
            var negativeScore = -10;


            var number = sut.GenerateNumber(2, -negativeScore);

        }
    }
}
