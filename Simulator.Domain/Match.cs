using System;

namespace Simulator.Domain
{
    public class Match
    {
        private Guid _teamIdA;
        private Guid _teamIdB;

        private Team _teamA;
        private Team _teamB;

        public Match(Team firstTeam, Team secondTeam)
        {
            MatchId = Guid.NewGuid();
            firstTeam.IsAttacking = true;
            secondTeam.IsAttacking = false;

            FirstTeamId = firstTeam.Id;
            SecondTeamId = secondTeam.Id;
            FirstTeam = firstTeam;
            SecondTeam = secondTeam;
        }

        public Guid MatchId { get; set; }

        public Guid FirstTeamId
        {
            get
            {
                return _teamIdA;
            }
            private set
            {
                _teamIdA = value;
            }
        }

        public Team FirstTeam
        {
            get
            {
                return _teamA;
            }
            private set
            {
                _teamA = value;
            }
        }

        public Guid SecondTeamId
        {
            get
            {
                return _teamIdB;
            }
            private set
            {
                _teamIdB = value;
            }
        }

        public Team SecondTeam
        {
            get
            {
                return _teamB;
            }
            private set
            {
                _teamB = value;
            }
        }

        public int ScoreFirstTeam { get; set; }
        public int ScoreSecondTeam { get; set; }
    }
}