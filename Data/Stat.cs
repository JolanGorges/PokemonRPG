namespace PokemonRPG.Data;

public class Stat
{
    public int Base { get; set; }
    public int Iv { get; set; }
    private int _exp;

    public int Exp
    {
        get => _exp;
        set => _exp = value > ushort.MaxValue ? ushort.MaxValue : value;
    }

    private int _value;

    public int Value
    {
        get => _value;
        set => _value = value < 0 ? 0 : value;
    }

    private int _current;

    public int Current
    {
        get => _current;
        set
        {
            if (value > 999)
                _current = 999;
            else if (value < 1)
                _current = 1;
            else
                _current = value;
        }
    }

    private int _stage;

    public int Stage
    {
        get => _stage;
        set
        {
            if (value > 6)
                _stage = 6;
            else if (value < -6)
                _stage = -6;
            else
                _stage = value;
        }
    }
}