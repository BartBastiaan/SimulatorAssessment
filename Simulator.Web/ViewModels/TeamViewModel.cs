using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simulator.Web.ViewModels
{
    public class TeamViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TotalPoints { get; set; }
        public int TotalGoalsFor { get; set; }
        public int TotalGoalsAgaint { get; set; }
        public int GoalDifference => TotalGoalsFor - TotalGoalsAgaint;
        public List<PlayerViewModel> Players { get; set; }
    }
}
