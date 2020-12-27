using System.Collections.Generic;

namespace Simulator.Web.ViewModels
{
    public class PoolOverView
    {
        public PoolOverView()
        {
            Teams = new List<TeamViewModel>();
        }
        public List<TeamViewModel> Teams { get; set; }
    }
}
