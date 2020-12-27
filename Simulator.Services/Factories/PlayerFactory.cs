using Simulator.Domain;
using System;

namespace Simulator.Services.Factories
{
    public class PlayerFactory : IPlayerFactory
    {
        public Player CreatePlayer(Guid teamId, PlayerPosistionEnum position, int attack, int defense)
        {
            return new Player(Guid.NewGuid(), teamId, position, attack, defense);
        }
    }
}
