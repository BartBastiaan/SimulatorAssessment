using System;
using System.Collections.Generic;

namespace Simulator.Domain
{
    public class Team
    {
        private Guid _id;
        private int _totalPoints;
        private string _name;

        public Team(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id
        {
            get
            {
                return _id;
            }
            private set
            {
                _id = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            private set
            {
                _name = value;
            }
        }

        public int TotalPoints
        {
            get
            {
                return _totalPoints;
            }
            private set
            {
                _totalPoints = value;
            }
        }

        public bool IsAttacking { get; set; }

        public List<Player> Players { get; set; }

        public int TotalGoalsFor { get; set; }
        public int TotalGoalsAgaint { get; set; }

        public int GoalDifference => TotalGoalsFor - TotalGoalsAgaint;

        public void AddToTotalPoints(int points)
        {
            TotalPoints += points;
        }

        public void SwitchAttackDefensePosition()
        {
            if (!IsAttacking)
            {
                IsAttacking = true;
            }
            else 
            {
                IsAttacking = false;
            }
        }
    }
}
