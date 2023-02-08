using chessAPI.business.interfaces;
using chessAPI.dataAccess.repositores;
using chessAPI.models.game;

namespace chessAPI.business.impl;

public sealed class clsGameBusiness<TI, TC> : IGameBusiness<TI> 
    where TI : struct, IEquatable<TI>
    where TC : struct
{
    internal readonly IGameRepository<TI, TC> GameRepository;

    public clsGameBusiness(IGameRepository<TI, TC> GameRepository)
    {
        this.GameRepository = GameRepository;
    }

    public async Task<clsGame<TI>> InitGame(TI TeamBlancas,TI Team)
    {
        clsNewGame<TI> newGame =  new clsNewGame<TI>(TeamBlancas, Team, Team);
         var x = await GameRepository.addGame(newGame).ConfigureAwait(false);
         string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        return new clsGame<TI>(x,fecha,true,TeamBlancas, Team, Team);
    }

    public async Task<clsGame<TI>> secondTeamGame(TI gameId, TI blackPlayer)
    {
        var x = await GameRepository.getGame(gameId).ConfigureAwait(false);
        var updatedGame = new clsGame<TI>(x.id, x.started,x.turn,x.whites, blackPlayer, x.winner);
        await GameRepository.updateGame(updatedGame).ConfigureAwait(false);
        return updatedGame;
    }

    public async Task<clsGame<TI>> addGame(clsNewGame<TI> newGame)
    {
        var x = await GameRepository.addGame(newGame).ConfigureAwait(false);
        return new clsGame<TI>(x,newGame.started, newGame.turn, newGame.winner ,newGame.whites, newGame.blacks);
    }
    public async Task<clsGame<TI>> getGame(TI Gameid)
    {
        var Game = await GameRepository.getGame(Gameid).ConfigureAwait(false);
        return new clsGame<TI>(Game.id, Game.started, Game.turn, Game.winner,Game.whites, Game.blacks);
    }

    public async Task<clsGame<TI>> updateGame(clsGame<TI> updatedGame)
    {
        await GameRepository.updateGame(updatedGame).ConfigureAwait(false);
        return updatedGame;
    }
}
