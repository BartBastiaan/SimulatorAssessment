using Simulator.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulator.Services
{
    public class MatchSimulatorService : IMatchSimulatorService
    {
        private Match _match { get; set; }
        private int _attempts { get; set; }
        private BallPositionEnum _ballPosition { get; set; }
        private bool _shootAttempt { get; set; }

        public MatchSimulatorService()
        {

        }
        private void SubtractAttempt()
        {
            _attempts -= 1;
        }

        private void PrepareMoveForwardAttempt()
        {
            _shootAttempt = false;
            _ballPosition += 1;
            SubtractAttempt();
        }

        private void PrepareShootAttempt()
        {
            _shootAttempt = true;
            SubtractAttempt();
        }

        // Adjust parameters After shoot attempt or if defending team wins at any other position
        private void PrepareAttackingAttempt()
        {
            _ballPosition = InvertBallPosition(_ballPosition);
            _shootAttempt = false;
            SubtractAttempt();
            SwitchAttackingSide(_match);
        }

        private void InitializeSimulationParamaters(Match match, BallPositionEnum ballPosition, int attempts, bool shootAttempt)
        {
            _match = match;
            _attempts = attempts;
            _ballPosition = ballPosition;
            _shootAttempt = shootAttempt;
        }

        public Match RunSimulation(Match match, BallPositionEnum ballPosition = BallPositionEnum.Mid, int attempts = 42, bool shootAttempt = false)
        {
            InitializeSimulationParamaters(match, ballPosition, attempts, shootAttempt);

            while (_attempts > 0)
            {
                var attackingTeam = _match.FirstTeam.IsAttacking ? _match.FirstTeam : _match.SecondTeam;
                var defendingTeam = !_match.FirstTeam.IsAttacking ? _match.FirstTeam : _match.SecondTeam;

                // Get playerScore at ballposition
                var attackingPlayerPoints = CalculateAttackingPoinstAtBallPosition(attackingTeam, _ballPosition);
                // When _shootAttempt is true defendingPlayerPoints = playersPoint + goalKeerpPoints
                var defendingPlayerPoints = CalculateDefendingPoinstAtBallPosition(defendingTeam, _ballPosition, _shootAttempt);

                // differenceBonusPlayerPoints is a banus for the players with higher playerPoint at the position
                var differenceBonusPlayerPoints = attackingPlayerPoints - defendingPlayerPoints;

                // attackingPlayerPoints and defendingPlayerPoints are used to generate a number
                var attackPoints = GenerateNumber(attackingPlayerPoints, differenceBonusPlayerPoints > 0 ? differenceBonusPlayerPoints : 0);
                var defensePoints = GenerateNumber(defendingPlayerPoints, differenceBonusPlayerPoints < 0 ? -differenceBonusPlayerPoints : 0);

                if (_shootAttempt)
                {
                    // if attack is higher then defense attakcing team scores
                    if (attackPoints > defensePoints)
                    {
                        if (_match.FirstTeam.IsAttacking)
                        {
                            _match.ScoreFirstTeam += 1;
                        }
                        else
                        {
                            _match.ScoreSecondTeam += 1;
                        }
                    }
                    // When attacking team does not score, switch defending team to attack and adjust parameters
                    PrepareAttackingAttempt();
                    continue;
                }

                if (attackPoints > defensePoints)
                {
                    // When already at the front the next Attempt will be a shooting attempt
                    if (_ballPosition == BallPositionEnum.Front)
                    {
                        PrepareShootAttempt();
                    }
                    else
                    {
                        PrepareMoveForwardAttempt();
                    }
                    continue;
                }

                // When defending team wins the attempt, switch defending team to attack and adjust parameters
                PrepareAttackingAttempt();
            }

            var processedMatchResults = ProcesMatchResult(_match);

            return processedMatchResults;
        }

        public Match ProcesMatchResult(Match match)
        {
            match.FirstTeam.TotalGoalsFor += match.ScoreFirstTeam;
            match.FirstTeam.TotalGoalsAgaint += match.ScoreSecondTeam;

            match.SecondTeam.TotalGoalsFor += match.ScoreSecondTeam;
            match.SecondTeam.TotalGoalsAgaint += match.ScoreFirstTeam;

            if (match.ScoreFirstTeam == match.ScoreSecondTeam)
            {
                match.FirstTeam.AddToTotalPoints(1);
                match.SecondTeam.AddToTotalPoints(1);
            }
            else 
            {
                if (match.ScoreFirstTeam > match.ScoreSecondTeam)
                {
                    match.FirstTeam.AddToTotalPoints(3);
                }
                else if (match.ScoreSecondTeam > match.ScoreFirstTeam)
                {
                    match.SecondTeam.AddToTotalPoints(3);
                }
            }

            return match;
        }

        public int GenerateNumber(int maxPlayerPoints, int differenceBonusPlayerPoints = 0)
        {
            var randominator = new Random();
            // differenceBonusPlayerPoints cannot exceed maxPlayerPoints
            if (differenceBonusPlayerPoints > maxPlayerPoints)
            {
                differenceBonusPlayerPoints = maxPlayerPoints;
            }

            return randominator.Next(differenceBonusPlayerPoints, maxPlayerPoints);
        }

        public void SwitchAttackingSide(Match match)
        {
            match.FirstTeam.SwitchAttackDefensePosition();
            match.SecondTeam.SwitchAttackDefensePosition();
        }

        public int CalculateAttackingPoinstAtBallPosition(Team team, BallPositionEnum ballPosition)
        {
            var playersAtBallPosition = GetPlayersAtPosition(team, ballPosition);
            return playersAtBallPosition.Select(x => x.Attack).ToList().Sum();
        }

        public int CalculateDefendingPoinstAtBallPosition(Team team, BallPositionEnum ballPosition,bool includeGoalKeeper)
        {
            BallPositionEnum defenseBallPosition = InvertBallPosition(ballPosition);

            var playersAtBallPosition = GetPlayersAtPosition(team, defenseBallPosition);
            var totalPlayerScore = playersAtBallPosition.Select(x => x.Defense).ToList().Sum();

            // Include goalkeeper score when attacking team tries to shoot
            if (includeGoalKeeper)
            {
                var goalKeeper = team.Players.Where(x => x.Posistion == PlayerPosistionEnum.GoalKeeper).FirstOrDefault();
                totalPlayerScore += goalKeeper == null ? 0 : goalKeeper.Defense;
            }

            return totalPlayerScore;
        }

        // should return the combined number of all players at the sameposition of the ball
        public List<Player> GetPlayersAtPosition(Team team, BallPositionEnum ballPosition)
        {
            return team.Players.Where(x => (int)x.Posistion == ((int)ballPosition)).ToList();
        }

        public BallPositionEnum InvertBallPosition(BallPositionEnum ballPosition)
        {
            BallPositionEnum defenseBallPosition = 0;
            // if ballPosition not mid use inverted position for defense
            if (ballPosition != BallPositionEnum.Mid)
            {
                defenseBallPosition = ballPosition == BallPositionEnum.Front ? BallPositionEnum.Back : BallPositionEnum.Front;
            }
            else
            {
                defenseBallPosition = ballPosition;
            }

            return defenseBallPosition;
        }
    }
}
