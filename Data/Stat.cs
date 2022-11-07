namespace PokemonRPG.Data;

public class Stat
{
    private int _exp;
    
    private int _value;
    public int Base { get; set; }
    public int Iv { get; set; }

    public int Exp
    {
        get => _exp;
        set => _exp = value > ushort.MaxValue ? ushort.MaxValue : value;
    }

    public int Value
    {
        get => _value;
        set => _value = value < 0 ? 0 : value;
    }
}