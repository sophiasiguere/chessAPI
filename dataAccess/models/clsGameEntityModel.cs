using chessAPI.dataAccess.common;
namespace chessAPI.dataAccess.models;

public sealed class clsGameEntityModel<TI, TC> : relationalEntity<TI, TC>
        where TC : struct
        where TI : struct, IEquatable<TI>
{
    public clsGameEntityModel()
    {
        started = "";
        turn = false;
    }

    public TI id { get; set; }
    public string started { get; set; }
    public bool turn { get; set; }
    public TI winner { get; set; }
    public TI whites { get; set; }
    public TI blacks { get; set; }
    public override TI key { get => id; set => id = value; }
}