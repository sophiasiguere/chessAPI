using chessAPI.models.game;

namespace chessAPI.business.interfaces;

public interface IGameBusiness<TI> 
    where TI : struct, IEquatable<TI>
{
    Task<clsGame<TI>> addGame(clsNewGame<TI> newGame);
    Task<clsGame<TI>> InitGame(TI TeamBlancas,TI Team);
    Task<clsGame<TI>> getGame(TI gameid);
    Task<clsGame<TI>> updateGame(clsGame<TI> updatedGame);
    Task<clsGame<TI>> secondTeamGame(TI gameId, TI playerId);

}