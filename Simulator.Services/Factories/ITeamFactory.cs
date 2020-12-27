using Simulator.Domain;

namespace Simulator.Services.Factories
{
    public interface ITeamFactory
    {
        Team CreateTeam(string name);
    }
}
