using Simulator.Domain;
using Simulator.Web.ViewModels;
using System;

namespace Simulator.Web.Mapper
{
    public class PlayerModelMapper : IPlayerModelMapper
    {
        public PlayerViewModel MapToPlayerViewModel(Player player)
        {
            var playerViewModel = new PlayerViewModel();
            playerViewModel.Attack = player.Attack;
            playerViewModel.Defense = player.Defense;
            playerViewModel.Posistion = player.Posistion;

            return playerViewModel;
        }
    }
}
