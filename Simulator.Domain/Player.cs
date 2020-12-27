using System;

namespace Simulator.Domain
{
    public class Player
    {
        public Player(Guid id, Guid teamId, PlayerPosistionEnum position, int attack, int defense)
        {
            Id = id;
            TeamId = teamId;
            Posistion = position;
            Attack = attack;
            Defense = defense;
        }

        public Guid Id { get; private set; }
        public Guid TeamId { get; private set; }
        public PlayerPosistionEnum Posistion { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }    
    }
}
