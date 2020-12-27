using Simulator.Domain;
using System;
using System.Collections.Generic;

namespace Simulator.Services
{
    public class TeamGeneratorService : ITeamGeneratorService
    {
        public List<Team> GenerateFixedTeams()
        {
            var teams = new List<Team>();

            var teamA = CreateTeam("Team A");
            teamA.Players = CreatePlayers(teamA.Id, 6, 6, 5, 3, 2);
            teams.Add(teamA);

            var teamB = CreateTeam("Team B");
            teamB.Players = CreatePlayers(teamA.Id, 3, 3, 3, 4, 3);
            teams.Add(teamB);

            var teamC = CreateTeam("Team C");
            teamC.Players = CreatePlayers(teamA.Id, 2, 6, 2, 3, 5);
            teams.Add(teamC);

            var teamD = CreateTeam("Team D");
            teamD.Players = CreatePlayers(teamA.Id, 6, 2, 3, 5, 2);
            teams.Add(teamD);

            return teams;
        }

        private Team CreateTeam(string name)
        {
            var team = new Team(Guid.NewGuid(), name);
            
            return team;
        }


        private List<Player> CreatePlayers(Guid teamId, int attack, int defense, int amountBack, int amountMid, int amountFront)
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
    }
}
