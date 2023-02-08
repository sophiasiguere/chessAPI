namespace chessAPI.models.game;

public sealed class clsNewGame<TI>
where TI : struct, IEquatable<TI>
{
    //SIEMPRE INICIAN BLANCAS
    public clsNewGame(TI whites, TI blacks)
    {
        this.whites = whites;
        this.blacks = blacks;
        this.turn = true;
        this.winner = whites;
    }

    //CONTINUACIÓN DE CÓDIGO
    public clsNewGame(TI whites, TI blacks, TI defaultValue)
    {
        this.whites = whites;
        this.blacks = blacks;
        this.turn = true;
        this.winner = defaultValue;
    }

    public string started { get; set; }
    public bool turn { get; set; }
    public TI winner { get; set; }
    public TI whites { get; set; }
    public TI blacks { get; set; }
        
    
}