
using Simulator.Domain;
using System.Collections.Generic;

namespace Simulator.Services
{
    public interface IMatchGeneratorService
    {
        List<Match> GenerateMatches(List<Team> teams);
    }
}
