using Simulator.Domain;
using Simulator.Web.ViewModels;

namespace Simulator.Web.Mapper
{
    public interface ITeamModelMapper
    {
        TeamViewModel MapToTeamViewModel(Team team);
    }
}
