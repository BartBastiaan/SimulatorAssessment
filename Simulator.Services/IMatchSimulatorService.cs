using Simulator.Domain;

namespace Simulator.Services
{
    public interface IMatchSimulatorService
    {
        void SwitchAttackingSide(Match match);

        Match RunSimulation(Match match, BallPositionEnum ballPosition = BallPositionEnum.Mid, int attempts = 12, bool shootAttempt = false);

        int CalculateAttackingPoinstAtBallPosition(Team team, BallPositionEnum ballPosition);
        int CalculateDefendingPoinstAtBallPosition(Team team, BallPositionEnum ballPosition, bool includeGoalKeeper);
    }
}