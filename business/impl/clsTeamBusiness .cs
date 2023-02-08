using chessAPI.business.interfaces;
using chessAPI.dataAccess.repositores;
using chessAPI.models.team;

namespace chessAPI.business.impl;

public sealed class clsTeamBusiness<TI, TC> : ITeamBusiness<TI> 
    where TI : struct, IEquatable<TI>
    where TC : struct
{
    internal readonly ITeamRepository<TI, TC> TeamRepository;

    public clsTeamBusiness(ITeamRepository<TI, TC> TeamRepository)
    {
        this.TeamRepository = TeamRepository;
    }

   public async Task<clsTeam<TI>> addTeam(clsNewTeam newTeam)
    {
        var x = await TeamRepository.addTeam(newTeam).ConfigureAwait(false);
        return new clsTeam<TI>(x, newTeam.name);
    }
    public async Task<clsTeam<TI>> getTeam(TI TeamId)
    {
        var Team = await TeamRepository.getTeam(TeamId).ConfigureAwait(false);
        return new clsTeam<TI>(Team.id, Team.name);
    }

    public async Task<clsTeam<TI>> updateTeam(clsTeam<TI> updatedTeam)
    {
        await TeamRepository.updateTeam(updatedTeam).ConfigureAwait(false);
        return updatedTeam;
    }
}
