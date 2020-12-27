using Simulator.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simulator.Services
{
    public interface IPoolFinalizeService
    {
        PoolResult DeterminePoolResults(List<Match> playedMatches);
    }
}
