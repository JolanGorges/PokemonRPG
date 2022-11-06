using PokemonRPG.Enums;
using Type = PokemonRPG.Enums.Type;

namespace PokemonRPG.Data;

public abstract class Pokemon
{
    public string Name { get; protected set; }
    public Stat Hp { get; } = new();
    public int MaxHp { get; private set; }
    public Stat Attack { get; } = new();
    public Stat Defense { get; } = new();
    public Stat Speed { get; } = new();
    public Stat Special { get; } = new();
    private int ExpYield { get; }
    public Type Type1 { get; }
    public Type Type2 { get; }
    public List<MoveClass> Moves { get; }
    private GrowthRate GrowthRate { get; }
    public int Level { get; private set; }
    private int Exp { get; set; }
    public Stat Accuracy { get; } = new();
    public Stat Evasion { get; } = new();

    protected Pokemon(string name, int hp, int attack, int defense, int speed, int special, Type type1, Type type2, int expYield, IEnumerable<Move> moves, int growthRate, int level)
    {
        Name = name;
        Hp.Base = hp;
        Attack.Base = attack;
        Defense.Base = defense;
        Speed.Base = speed;
        Special.Base = special;
        Type1 = type1;
        Type2 = type2;
        ExpYield = expYield;
        Moves = moves.Select(MoveClass.GetMove).ToList();
        GrowthRate = (GrowthRate)growthRate;
        Level = level;
        Exp = CalculateExp(level);
        CalculateIVs();
        UpdateStats();
        Hp.Value = MaxHp;
    }

    public int GetNextLevelExp()
    {
        return CalculateExp(Level + 1);
    }

    private int CalculateExp(int level)
    {
        // https://bulbapedia.bulbagarden.net/wiki/Experience#Relation_to_level
        return GrowthRate switch
        {
            GrowthRate.MediumFast => (int)Math.Pow(level, 3),
            GrowthRate.MediumSlow => (int)(1.2 * Math.Pow(level, 3) - 15 * Math.Pow(level, 2) + 100 * level - 140),
            GrowthRate.Fast => (int)(4 * Math.Pow(level, 3) / 5),
            GrowthRate.Slow => (int)(5 * Math.Pow(level, 3) / 4),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private int CalculateLevel(int exp)
    {
        for (var i = 5; i < 255; i++)
        {
            if (CalculateExp(i) > exp)
            {
                return i - 1;
            }
        }
        return 100;
    }

    private void UpdateStats()
    {
        // https://bulbapedia.bulbagarden.net/wiki/Stat#Determination_of_stats
        MaxHp = (int)(((Hp.Base + Hp.Iv) * 2 + Math.Ceiling(Math.Sqrt(Hp.Exp)) / 4) * Level) / 100 + Level + 10;
        var otherStats = new[] { Attack, Defense, Speed, Special };
        foreach (var stat in otherStats)
        {
            stat.Value = (int)(((stat.Base + stat.Iv) * 2 + Math.Ceiling(Math.Sqrt(stat.Exp)) / 4) * Level) / 100 + 5;
        }
    }

    private void CalculateIVs(int encounterRate = 25)
    {
        // https://www.youtube.com/watch?v=BcIxMyf8yHY and http://wiki.pokemonspeedruns.com/index.php?title=Pok%C3%A9mon_Red%2FBlue_Wild_DVs
        int ra = Random.Shared.Next(0, encounterRate);
        int div = Random.Shared.Next(0, 256);
        int v1 = Random.Shared.Next(0, 2);
        int v2 = Random.Shared.Next(0, 2);
        int rand1 = ra + 2 * div + 0x2d + v1 + 1;
        int rand2 = rand1 + div + 0x2d + v1 + v2 + 2;

        // https://bulbapedia.bulbagarden.net/wiki/Individual_values#Generation_I_and_II
        Attack.Iv = (rand2 >> 4) % 16;
        Defense.Iv = rand2 % 16;
        Speed.Iv = (rand1 >> 4) % 16;
        Special.Iv = rand1 % 16;
        Hp.Iv = (Attack.Iv & 1) << 3 | (Defense.Iv & 1) << 2 | (Speed.Iv & 1) << 1 | (Special.Iv & 1);
    }

    public int AddExpGain(Pokemon defeatedPokemon)
    {
        // https://bulbapedia.bulbagarden.net/wiki/Effort_values#Stat_experience
        Attack.Exp += defeatedPokemon.Attack.Base;
        Defense.Exp += defeatedPokemon.Defense.Base;
        Speed.Exp += defeatedPokemon.Speed.Base;
        Special.Exp += defeatedPokemon.Special.Base;
        Hp.Exp += defeatedPokemon.Hp.Base;

        // https://bulbapedia.bulbagarden.net/wiki/Experience#Gain_formula
        int gain = defeatedPokemon.ExpYield * defeatedPokemon.Level / 7;
        Exp += gain;
        Level = CalculateLevel(Exp);
        UpdateStats();
        return gain;
    }
}