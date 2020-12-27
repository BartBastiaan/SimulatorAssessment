using Simulator.Domain;
using System;

namespace Simulator.Services.Factories
{
    public class TeamFactory : ITeamFactory
    {
        public Team CreateTeam(string name)
        {
            return new Team(Guid.NewGuid(), name);
        }
    }
}
