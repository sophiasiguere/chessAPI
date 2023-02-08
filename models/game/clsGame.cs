namespace chessAPI.models.game;

public sealed class clsGame<TI>
    where TI : struct, IEquatable<TI>
{
    public clsGame(TI id, string started, bool turn, TI winner,TI whites, TI blacks )
    {
        this.id = id;
        this.started = started;
        this.turn = turn;
        this.winner = winner;
        this.whites = whites;
        this.blacks = blacks;
    }

    public TI id { get; set; }
    public string started { get; set; }
    public bool turn { get; set; }
    public TI winner { get; set; }
    public TI whites { get; set; }
    public TI blacks { get; set; } 
}