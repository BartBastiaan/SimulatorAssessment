using Simulator.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simulator.Services
{
    public interface ITeamGeneratorService
    {
        List<Team> GenerateFixedTeams();
    }
}
