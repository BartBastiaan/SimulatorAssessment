using Microsoft.AspNetCore.Mvc;
using Simulator.Domain;
using Simulator.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simulator.Web.Mapper
{
    public interface IPlayerModelMapper 
    {
        PlayerViewModel MapToPlayerViewModel(Player player);
    }
}
