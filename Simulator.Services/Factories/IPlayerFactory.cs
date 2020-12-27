using Simulator.Domain;
using System;

namespace Simulator.Services.Factories
{
    public interface IPlayerFactory
    {
        Player CreatePlayer(Guid teamId, PlayerPosistionEnum position, int attack, int defense);
    }
}
