using PokemonRPG.Enums;
using Type = PokemonRPG.Enums.Type;

namespace PokemonRPG.Data;

public class Pokemon
{
    private Pokemon(Dex dex, string name, int hp, int attack, int defense, int speed, Type type1, Type type2,
        int catchRate, int expYield, IEnumerable<Move> moves, GrowthRate growthRate, int level)
    {
        Name = name;
        Hp.Base = hp;
        Attack.Base = attack;
        Defense.Base = defense;
        Speed.Base = speed;
        Type1 = type1;
        Type2 = type2;
        CatchRate = catchRate;
        ExpYield = expYield;
        Moves = moves.Select(MoveClass.GetMove).ToList();
        GrowthRate = growthRate;
        Evolution = Evolution.GetEvolution(dex);
        MovesLearnset = Learnset.GetLearnset(dex);
        Level = level;
        Exp = CalculateExp(level);
        CalculateIVs();
        UpdateStats();
        Hp.Value = MaxHp;
    }

    public string Name { get; private set; }
    public Stat Hp { get; } = new();
    public int MaxHp { get; private set; }
    public Stat Attack { get; } = new();
    public Stat Defense { get; } = new();
    public Stat Speed { get; } = new();
    public int CatchRate { get; }
    private int ExpYield { get; }
    public Type Type1 { get; private set; }
    public Type Type2 { get; private set; }
    public List<MoveClass> Moves { get; }
    private GrowthRate GrowthRate { get; }
    public int Level { get; private set; }
    public int Exp { get; private set; }
    private Evolution? Evolution { get; }
    private Learnset[]? MovesLearnset { get; }
    public bool IsWild { get; set; }

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
            if (CalculateExp(i) > exp)
                return i - 1;
        return 100;
    }

    private void UpdateStats()
    {
        // https://bulbapedia.bulbagarden.net/wiki/Stat#Determination_of_stats
        MaxHp = (int)(((Hp.Base + Hp.Iv) * 2 + Math.Ceiling(Math.Sqrt(Hp.Exp)) / 4) * Level) / 100 + Level + 10;
        var otherStats = new[] { Attack, Defense, Speed };
        foreach (var stat in otherStats)
            stat.Value = (int)(((stat.Base + stat.Iv) * 2 + Math.Ceiling(Math.Sqrt(stat.Exp)) / 4) * Level) / 100 + 5;
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
        Hp.Iv = ((Attack.Iv & 1) << 3) | ((Defense.Iv & 1) << 2) | ((Speed.Iv & 1) << 1) | ((rand1 % 16) & 1);
    }

    public int AddExpGain(Pokemon defeatedPokemon)
    {
        // https://bulbapedia.bulbagarden.net/wiki/Effort_values#Stat_experience
        Hp.Exp += defeatedPokemon.Hp.Base;
        Attack.Exp += defeatedPokemon.Attack.Base;
        Defense.Exp += defeatedPokemon.Defense.Base;
        Speed.Exp += defeatedPokemon.Speed.Base;

        // https://bulbapedia.bulbagarden.net/wiki/Experience#Gain_formula
        int gain = defeatedPokemon.ExpYield * defeatedPokemon.Level / 7;
        Exp += gain;
        Level = CalculateLevel(Exp);
        UpdateStats();
        return gain;
    }

    public void CheckEvolutionsAndMoves()
    {
        if (Evolution != null && Evolution.Level <= Level)
        {
            var pokemon = GetPokemon(Evolution.Dex);
            Name = pokemon.Name;
            Hp.Base = pokemon.Hp.Base;
            Attack.Base = pokemon.Attack.Base;
            Defense.Base = pokemon.Defense.Base;
            Speed.Base = pokemon.Speed.Base;
            Type1 = pokemon.Type1;
            Type2 = pokemon.Type2;
            var moves = Moves.Select(x => x.Move).ToArray();
            foreach (var move in pokemon.Moves.Select(x => x.Move))
            {
                if (!moves.Contains(move))
                {
                    Moves.Add(MoveClass.GetMove(move));
                }
            }
        }

        if (MovesLearnset != null)
        {
            var moves = Moves.Select(x => x.Move).ToArray();
            foreach (var move in MovesLearnset.Where(x => x.Level <= Level).Select(x => x.Move))
            {
                if (!moves.Contains(move))
                {
                    Moves.Add(MoveClass.GetMove(move));
                }
            }
        }
    }

    public MoveClass GetRandomMove()
    {
        var damagingMoves = Moves.Where(move => move.Category == MoveCategory.Physical && move.Power > 0).ToArray();
        return damagingMoves.Length > 0
            ? damagingMoves[Random.Shared.Next(0, damagingMoves.Length)]
            : MoveClass.GetMove(Move.Struggle);
    }

    public static Pokemon GetRandomWildPokemon(int level)
    {
        Dex[] wildPokemons =
        {
            Dex.Abra, Dex.Arbok, Dex.Bellsprout, Dex.Chansey, Dex.Clefairy, Dex.Cubone, Dex.Dewgong, Dex.Diglett,
            Dex.Ditto, Dex.Dodrio, Dex.Doduo, Dex.Drowzee, Dex.Dugtrio, Dex.Ekans, Dex.Electabuzz, Dex.Electrode,
            Dex.Exeggcute, Dex.Fearow, Dex.Gastly, Dex.Geodude, Dex.Gloom, Dex.Golbat, Dex.Golduck, Dex.Graveler,
            Dex.Grimer, Dex.Growlithe, Dex.Haunter, Dex.Horsea, Dex.Hypno, Dex.Kadabra, Dex.Kangaskhan, Dex.Kingler,
            Dex.Koffing, Dex.Krabby, Dex.Machoke, Dex.Machop, Dex.Magmar, Dex.Magnemite, Dex.Magneton, Dex.Mankey,
            Dex.Marowak, Dex.Meowth, Dex.Muk, Dex.NidoranF, Dex.NidoranM, Dex.Nidorina, Dex.Nidorino, Dex.Oddish,
            Dex.Onix, Dex.Paras, Dex.Parasect, Dex.Pidgeotto, Dex.Pidgey, Dex.Pikachu, Dex.Pinsir, Dex.Ponyta,
            Dex.Psyduck, Dex.Raichu, Dex.Raticate, Dex.Rattata, Dex.Rhydon, Dex.Rhyhorn, Dex.Sandshrew, Dex.Sandslash,
            Dex.Scyther, Dex.Seadra, Dex.Seel, Dex.Shellder, Dex.Slowbro, Dex.Slowpoke, Dex.Spearow, Dex.Staryu,
            Dex.Tangela, Dex.Tauros, Dex.Tentacool, Dex.Venomoth, Dex.Venonat, Dex.Voltorb, Dex.Vulpix, Dex.Weepinbell,
            Dex.Weezing, Dex.Wigglytuff, Dex.Zubat
        };
        var pokemon = GetPokemon(wildPokemons[Random.Shared.Next(wildPokemons.Length)], level);
        pokemon.IsWild = true;
        return pokemon;
    }

    public static Pokemon GetPokemon(Dex dex, int level = 5)
    {
        return dex switch
        {
            Dex.Bulbasaur => new Pokemon(dex, "Bulbizarre", 45, 49, 49, 45, Type.Grass, Type.Poison, 45, 64,
                new[] { Move.Tackle, Move.Growl }, GrowthRate.MediumSlow, level),
            Dex.Ivysaur => new Pokemon(dex, "Herbizarre", 60, 62, 63, 60, Type.Grass, Type.Poison, 45, 141,
                new[] { Move.Tackle, Move.Growl, Move.LeechSeed }, GrowthRate.MediumSlow, level),
            Dex.Venusaur => new Pokemon(dex, "Florizarre", 80, 82, 83, 80, Type.Grass, Type.Poison, 45, 208,
                new[] { Move.Tackle, Move.Growl, Move.LeechSeed, Move.VineWhip }, GrowthRate.MediumSlow, level),
            Dex.Charmander => new Pokemon(dex, "Salamèche", 39, 52, 43, 65, Type.Fire, Type.Fire, 45, 65,
                new[] { Move.Scratch, Move.Growl }, GrowthRate.MediumSlow, level),
            Dex.Charmeleon => new Pokemon(dex, "Reptincel", 58, 64, 58, 80, Type.Fire, Type.Fire, 45, 142,
                new[] { Move.Scratch, Move.Growl, Move.Ember }, GrowthRate.MediumSlow, level),
            Dex.Charizard => new Pokemon(dex, "Dracaufeu", 78, 84, 78, 100, Type.Fire, Type.Flying, 45, 209,
                new[] { Move.Scratch, Move.Growl, Move.Ember, Move.Leer }, GrowthRate.MediumSlow, level),
            Dex.Squirtle => new Pokemon(dex, "Carapuce", 44, 48, 65, 43, Type.Water, Type.Water, 45, 66,
                new[] { Move.Tackle, Move.TailWhip }, GrowthRate.MediumSlow, level),
            Dex.Wartortle => new Pokemon(dex, "Carabaffe", 59, 63, 80, 58, Type.Water, Type.Water, 45, 143,
                new[] { Move.Tackle, Move.TailWhip, Move.Bubble }, GrowthRate.MediumSlow, level),
            Dex.Blastoise => new Pokemon(dex, "Tortank", 79, 83, 100, 78, Type.Water, Type.Water, 45, 210,
                new[] { Move.Tackle, Move.TailWhip, Move.Bubble, Move.WaterGun }, GrowthRate.MediumSlow, level),
            Dex.Caterpie => new Pokemon(dex, "Chenipan", 45, 30, 35, 45, Type.Bug, Type.Bug, 255, 53,
                new[] { Move.Tackle, Move.StringShot }, GrowthRate.MediumFast, level),
            Dex.Metapod => new Pokemon(dex, "Chrysacier", 50, 20, 55, 30, Type.Bug, Type.Bug, 120, 72,
                new[] { Move.Harden }, GrowthRate.MediumFast, level),
            Dex.Butterfree => new Pokemon(dex, "Papilusion", 60, 45, 50, 70, Type.Bug, Type.Flying, 45, 160,
                new[] { Move.Confusion }, GrowthRate.MediumFast, level),
            Dex.Weedle => new Pokemon(dex, "Aspicot", 40, 35, 30, 50, Type.Bug, Type.Poison, 255, 52,
                new[] { Move.PoisonSting, Move.StringShot }, GrowthRate.MediumFast, level),
            Dex.Kakuna => new Pokemon(dex, "Coconfort", 45, 25, 50, 35, Type.Bug, Type.Poison, 120, 71,
                new[] { Move.Harden }, GrowthRate.MediumFast, level),
            Dex.Beedrill => new Pokemon(dex, "Dardargnan", 65, 80, 40, 75, Type.Bug, Type.Poison, 45, 159,
                new[] { Move.FuryAttack }, GrowthRate.MediumFast, level),
            Dex.Pidgey => new Pokemon(dex, "Roucool", 40, 45, 40, 56, Type.Normal, Type.Flying, 255, 55,
                new[] { Move.Gust }, GrowthRate.MediumSlow, level),
            Dex.Pidgeotto => new Pokemon(dex, "Roucoups", 63, 60, 55, 71, Type.Normal, Type.Flying, 120, 113,
                new[] { Move.Gust, Move.SandAttack }, GrowthRate.MediumSlow, level),
            Dex.Pidgeot => new Pokemon(dex, "Roucarnage", 83, 80, 75, 91, Type.Normal, Type.Flying, 45, 172,
                new[] { Move.Gust, Move.SandAttack, Move.QuickAttack }, GrowthRate.MediumSlow, level),
            Dex.Rattata => new Pokemon(dex, "Rattata", 30, 56, 35, 72, Type.Normal, Type.Normal, 255, 57,
                new[] { Move.Tackle, Move.TailWhip }, GrowthRate.MediumFast, level),
            Dex.Raticate => new Pokemon(dex, "Rattatac", 55, 81, 60, 97, Type.Normal, Type.Normal, 90, 116,
                new[] { Move.Tackle, Move.TailWhip, Move.QuickAttack }, GrowthRate.MediumFast, level),
            Dex.Spearow => new Pokemon(dex, "Piafabec", 40, 60, 30, 70, Type.Normal, Type.Flying, 255, 58,
                new[] { Move.Peck, Move.Growl }, GrowthRate.MediumFast, level),
            Dex.Fearow => new Pokemon(dex, "Rapasdepic", 65, 90, 65, 100, Type.Normal, Type.Flying, 90, 162,
                new[] { Move.Peck, Move.Growl, Move.Leer }, GrowthRate.MediumFast, level),
            Dex.Ekans => new Pokemon(dex, "Abo", 35, 60, 44, 55, Type.Poison, Type.Poison, 255, 62,
                new[] { Move.Wrap, Move.Leer }, GrowthRate.MediumFast, level),
            Dex.Arbok => new Pokemon(dex, "Arbok", 60, 85, 69, 80, Type.Poison, Type.Poison, 90, 147,
                new[] { Move.Wrap, Move.Leer, Move.PoisonSting }, GrowthRate.MediumFast, level),
            Dex.Pikachu => new Pokemon(dex, "Pikachu", 35, 55, 30, 90, Type.Electric, Type.Electric, 190, 82,
                new[] { Move.Thundershock, Move.Growl }, GrowthRate.MediumFast, level),
            Dex.Raichu => new Pokemon(dex, "Raichu", 60, 90, 55, 100, Type.Electric, Type.Electric, 75, 122,
                new[] { Move.Thundershock, Move.Growl, Move.ThunderWave }, GrowthRate.MediumFast, level),
            Dex.Sandshrew => new Pokemon(dex, "Sabelette", 50, 75, 85, 40, Type.Ground, Type.Ground, 255, 93,
                new[] { Move.Scratch }, GrowthRate.MediumFast, level),
            Dex.Sandslash => new Pokemon(dex, "Sablaireau", 75, 100, 110, 65, Type.Ground, Type.Ground, 90, 163,
                new[] { Move.Scratch, Move.SandAttack }, GrowthRate.MediumFast, level),
            Dex.NidoranF => new Pokemon(dex, "Nidoran♀", 55, 47, 52, 41, Type.Poison, Type.Poison, 235, 59,
                new[] { Move.Growl, Move.Tackle }, GrowthRate.MediumSlow, level),
            Dex.Nidorina => new Pokemon(dex, "Nidorina", 70, 62, 67, 56, Type.Poison, Type.Poison, 120, 117,
                new[] { Move.Growl, Move.Tackle, Move.Scratch }, GrowthRate.MediumSlow, level),
            Dex.Nidoqueen => new Pokemon(dex, "Nidoqueen", 90, 82, 87, 76, Type.Poison, Type.Ground, 45, 194,
                new[] { Move.Tackle, Move.Scratch, Move.TailWhip, Move.BodySlam }, GrowthRate.MediumSlow, level),
            Dex.NidoranM => new Pokemon(dex, "Nidoran♂", 46, 57, 40, 50, Type.Poison, Type.Poison, 235, 60,
                new[] { Move.Leer, Move.Tackle }, GrowthRate.MediumSlow, level),
            Dex.Nidorino => new Pokemon(dex, "Nidorino", 61, 72, 57, 65, Type.Poison, Type.Poison, 120, 118,
                new[] { Move.Leer, Move.Tackle, Move.HornAttack }, GrowthRate.MediumSlow, level),
            Dex.Nidoking => new Pokemon(dex, "Nidoking", 81, 92, 77, 85, Type.Poison, Type.Ground, 45, 195,
                new[] { Move.Tackle, Move.HornAttack, Move.PoisonSting, Move.Thrash }, GrowthRate.MediumSlow, level),
            Dex.Clefairy => new Pokemon(dex, "Mélofée", 70, 45, 48, 35, Type.Normal, Type.Normal, 150, 68,
                new[] { Move.Pound, Move.Growl }, GrowthRate.Fast, level),
            Dex.Clefable => new Pokemon(dex, "Mélodelfe", 95, 70, 73, 60, Type.Normal, Type.Normal, 25, 129,
                new[] { Move.Sing, Move.Doubleslap, Move.Minimize, Move.Metronome }, GrowthRate.Fast, level),
            Dex.Vulpix => new Pokemon(dex, "Goupix", 38, 41, 40, 65, Type.Fire, Type.Fire, 190, 63,
                new[] { Move.Ember, Move.TailWhip }, GrowthRate.MediumFast, level),
            Dex.Ninetales => new Pokemon(dex, "Feunard", 73, 76, 75, 100, Type.Fire, Type.Fire, 75, 178,
                new[] { Move.Ember, Move.TailWhip, Move.QuickAttack, Move.Roar }, GrowthRate.MediumFast, level),
            Dex.Jigglypuff => new Pokemon(dex, "Rondoudou", 115, 45, 20, 20, Type.Normal, Type.Normal, 170, 76,
                new[] { Move.Sing }, GrowthRate.Fast, level),
            Dex.Wigglytuff => new Pokemon(dex, "Grodoudou", 140, 70, 45, 45, Type.Normal, Type.Normal, 50, 109,
                new[] { Move.Sing, Move.Disable, Move.DefenseCurl, Move.Doubleslap }, GrowthRate.Fast, level),
            Dex.Zubat => new Pokemon(dex, "Nosferapti", 40, 45, 35, 55, Type.Poison, Type.Flying, 255, 54,
                new[] { Move.LeechLife }, GrowthRate.MediumFast, level),
            Dex.Golbat => new Pokemon(dex, "Nosferalto", 75, 80, 70, 90, Type.Poison, Type.Flying, 90, 171,
                new[] { Move.LeechLife, Move.Screech, Move.Bite }, GrowthRate.MediumFast, level),
            Dex.Oddish => new Pokemon(dex, "Mystherbe", 45, 50, 55, 30, Type.Grass, Type.Poison, 255, 78,
                new[] { Move.Absorb }, GrowthRate.MediumSlow, level),
            Dex.Gloom => new Pokemon(dex, "Ortide", 60, 65, 70, 40, Type.Grass, Type.Poison, 120, 132,
                new[] { Move.Absorb, Move.Poisonpowder, Move.StunSpore }, GrowthRate.MediumSlow, level),
            Dex.Vileplume => new Pokemon(dex, "Rafflesia", 75, 80, 85, 50, Type.Grass, Type.Poison, 45, 184,
                new[] { Move.StunSpore, Move.SleepPowder, Move.Acid, Move.PetalDance }, GrowthRate.MediumSlow, level),
            Dex.Paras => new Pokemon(dex, "Paras", 35, 70, 55, 25, Type.Bug, Type.Grass, 190, 70,
                new[] { Move.Scratch }, GrowthRate.MediumFast, level),
            Dex.Parasect => new Pokemon(dex, "Parasect", 60, 95, 80, 30, Type.Bug, Type.Grass, 75, 128,
                new[] { Move.Scratch, Move.StunSpore, Move.LeechLife }, GrowthRate.MediumFast, level),
            Dex.Venonat => new Pokemon(dex, "Mimitoss", 60, 55, 50, 45, Type.Bug, Type.Poison, 190, 75,
                new[] { Move.Tackle, Move.Disable }, GrowthRate.MediumFast, level),
            Dex.Venomoth => new Pokemon(dex, "Aéromite", 70, 65, 60, 90, Type.Bug, Type.Poison, 75, 138,
                new[] { Move.Tackle, Move.Disable, Move.Poisonpowder, Move.LeechLife }, GrowthRate.MediumFast, level),
            Dex.Diglett => new Pokemon(dex, "Taupiqueur", 10, 55, 25, 95, Type.Ground, Type.Ground, 255, 81,
                new[] { Move.Scratch }, GrowthRate.MediumFast, level),
            Dex.Dugtrio => new Pokemon(dex, "Triopikeur", 35, 80, 50, 120, Type.Ground, Type.Ground, 50, 153,
                new[] { Move.Scratch, Move.Growl, Move.Dig }, GrowthRate.MediumFast, level),
            Dex.Meowth => new Pokemon(dex, "Miaouss", 40, 45, 35, 90, Type.Normal, Type.Normal, 255, 69,
                new[] { Move.Scratch, Move.Growl }, GrowthRate.MediumFast, level),
            Dex.Persian => new Pokemon(dex, "Persian", 65, 70, 60, 115, Type.Normal, Type.Normal, 90, 148,
                new[] { Move.Scratch, Move.Growl, Move.Bite, Move.Screech }, GrowthRate.MediumFast, level),
            Dex.Psyduck => new Pokemon(dex, "Psykokwak", 50, 52, 48, 55, Type.Water, Type.Water, 190, 80,
                new[] { Move.Scratch }, GrowthRate.MediumFast, level),
            Dex.Golduck => new Pokemon(dex, "Akwakwak", 80, 82, 78, 85, Type.Water, Type.Water, 75, 174,
                new[] { Move.Scratch, Move.TailWhip, Move.Disable }, GrowthRate.MediumFast, level),
            Dex.Mankey => new Pokemon(dex, "Férosinge", 40, 80, 35, 70, Type.Fighting, Type.Fighting, 190, 74,
                new[] { Move.Scratch, Move.Leer }, GrowthRate.MediumFast, level),
            Dex.Primeape => new Pokemon(dex, "Colossinge", 65, 105, 60, 95, Type.Fighting, Type.Fighting, 75, 149,
                new[] { Move.Scratch, Move.Leer, Move.KarateChop, Move.FurySwipes }, GrowthRate.MediumFast, level),
            Dex.Growlithe => new Pokemon(dex, "Caninos", 55, 70, 45, 60, Type.Fire, Type.Fire, 190, 91,
                new[] { Move.Bite, Move.Roar }, GrowthRate.Slow, level),
            Dex.Arcanine => new Pokemon(dex, "Arcanin", 90, 110, 80, 95, Type.Fire, Type.Fire, 75, 213,
                new[] { Move.Roar, Move.Ember, Move.Leer, Move.TakeDown }, GrowthRate.Slow, level),
            Dex.Poliwag => new Pokemon(dex, "Ptitard", 40, 50, 40, 90, Type.Water, Type.Water, 255, 77,
                new[] { Move.Bubble }, GrowthRate.MediumSlow, level),
            Dex.Poliwhirl => new Pokemon(dex, "Têtarte", 65, 65, 65, 90, Type.Water, Type.Water, 120, 131,
                new[] { Move.Bubble, Move.Hypnosis, Move.WaterGun }, GrowthRate.MediumSlow, level),
            Dex.Poliwrath => new Pokemon(dex, "Tartard", 90, 85, 95, 70, Type.Water, Type.Fighting, 45, 185,
                new[] { Move.Hypnosis, Move.WaterGun, Move.Doubleslap, Move.BodySlam }, GrowthRate.MediumSlow, level),
            Dex.Abra => new Pokemon(dex, "Abra", 25, 20, 15, 90, Type.Psychic, Type.Psychic, 200, 73,
                new[] { Move.Teleport }, GrowthRate.MediumSlow, level),
            Dex.Kadabra => new Pokemon(dex, "Kadabra", 40, 35, 30, 105, Type.Psychic, Type.Psychic, 100, 145,
                new[] { Move.Teleport, Move.Confusion, Move.Disable }, GrowthRate.MediumSlow, level),
            Dex.Alakazam => new Pokemon(dex, "Alakazam", 55, 50, 45, 120, Type.Psychic, Type.Psychic, 50, 186,
                new[] { Move.Teleport, Move.Confusion, Move.Disable }, GrowthRate.MediumSlow, level),
            Dex.Machop => new Pokemon(dex, "Machoc", 70, 80, 50, 35, Type.Fighting, Type.Fighting, 180, 88,
                new[] { Move.KarateChop }, GrowthRate.MediumSlow, level),
            Dex.Machoke => new Pokemon(dex, "Machopeur", 80, 100, 70, 45, Type.Fighting, Type.Fighting, 90, 146,
                new[] { Move.KarateChop, Move.LowKick, Move.Leer }, GrowthRate.MediumSlow, level),
            Dex.Machamp => new Pokemon(dex, "Mackogneur", 90, 130, 80, 55, Type.Fighting, Type.Fighting, 45, 193,
                new[] { Move.KarateChop, Move.LowKick, Move.Leer }, GrowthRate.MediumSlow, level),
            Dex.Bellsprout => new Pokemon(dex, "Chétiflor", 50, 75, 35, 40, Type.Grass, Type.Poison, 255, 84,
                new[] { Move.VineWhip, Move.Growth }, GrowthRate.MediumSlow, level),
            Dex.Weepinbell => new Pokemon(dex, "Boustiflor", 65, 90, 50, 55, Type.Grass, Type.Poison, 120, 151,
                new[] { Move.VineWhip, Move.Growth, Move.Wrap }, GrowthRate.MediumSlow, level),
            Dex.Victreebel => new Pokemon(dex, "Empiflor", 80, 105, 65, 70, Type.Grass, Type.Poison, 45, 191,
                new[] { Move.SleepPowder, Move.StunSpore, Move.Acid, Move.RazorLeaf }, GrowthRate.MediumSlow, level),
            Dex.Tentacool => new Pokemon(dex, "Tentacool", 40, 40, 35, 70, Type.Water, Type.Poison, 190, 105,
                new[] { Move.Acid }, GrowthRate.Slow, level),
            Dex.Tentacruel => new Pokemon(dex, "Tentacruel", 80, 70, 65, 100, Type.Water, Type.Poison, 60, 205,
                new[] { Move.Acid, Move.Supersonic, Move.Wrap }, GrowthRate.Slow, level),
            Dex.Geodude => new Pokemon(dex, "Racaillou", 40, 80, 100, 20, Type.Rock, Type.Ground, 255, 86,
                new[] { Move.Tackle }, GrowthRate.MediumSlow, level),
            Dex.Graveler => new Pokemon(dex, "Gravalanch", 55, 95, 115, 35, Type.Rock, Type.Ground, 120, 134,
                new[] { Move.Tackle, Move.DefenseCurl }, GrowthRate.MediumSlow, level),
            Dex.Golem => new Pokemon(dex, "Grolem", 80, 110, 130, 45, Type.Rock, Type.Ground, 45, 177,
                new[] { Move.Tackle, Move.DefenseCurl }, GrowthRate.MediumSlow, level),
            Dex.Ponyta => new Pokemon(dex, "Ponyta", 50, 85, 55, 90, Type.Fire, Type.Fire, 190, 152,
                new[] { Move.Ember }, GrowthRate.MediumFast, level),
            Dex.Rapidash => new Pokemon(dex, "Galopa", 65, 100, 70, 105, Type.Fire, Type.Fire, 60, 192,
                new[] { Move.Ember, Move.TailWhip, Move.Stomp, Move.Growl }, GrowthRate.MediumFast, level),
            Dex.Slowpoke => new Pokemon(dex, "Ramoloss", 90, 65, 65, 15, Type.Water, Type.Psychic, 190, 99,
                new[] { Move.Confusion }, GrowthRate.MediumFast, level),
            Dex.Slowbro => new Pokemon(dex, "Flagadoss", 95, 75, 110, 30, Type.Water, Type.Psychic, 75, 164,
                new[] { Move.Confusion, Move.Disable, Move.Headbutt }, GrowthRate.MediumFast, level),
            Dex.Magnemite => new Pokemon(dex, "Magnéti", 25, 35, 70, 45, Type.Electric, Type.Electric, 190, 89,
                new[] { Move.Tackle }, GrowthRate.MediumFast, level),
            Dex.Magneton => new Pokemon(dex, "Magnéton", 50, 60, 95, 70, Type.Electric, Type.Electric, 60, 161,
                new[] { Move.Tackle, Move.Sonicboom, Move.Thundershock }, GrowthRate.MediumFast, level),
            Dex.Farfetchd => new Pokemon(dex, "Canarticho", 52, 65, 55, 60, Type.Normal, Type.Flying, 45, 94,
                new[] { Move.Peck, Move.SandAttack }, GrowthRate.MediumFast, level),
            Dex.Doduo => new Pokemon(dex, "Doduo", 35, 85, 45, 75, Type.Normal, Type.Flying, 190, 96,
                new[] { Move.Peck }, GrowthRate.MediumFast, level),
            Dex.Dodrio => new Pokemon(dex, "Dodrio", 60, 110, 70, 100, Type.Normal, Type.Flying, 45, 158,
                new[] { Move.Peck, Move.Growl, Move.FuryAttack }, GrowthRate.MediumFast, level),
            Dex.Seel => new Pokemon(dex, "Otaria", 65, 45, 55, 45, Type.Water, Type.Water, 190, 100,
                new[] { Move.Headbutt }, GrowthRate.MediumFast, level),
            Dex.Dewgong => new Pokemon(dex, "Lamantine", 90, 70, 80, 70, Type.Water, Type.Ice, 75, 176,
                new[] { Move.Headbutt, Move.Growl, Move.AuroraBeam }, GrowthRate.MediumFast, level),
            Dex.Grimer => new Pokemon(dex, "Tadmorv", 80, 80, 50, 25, Type.Poison, Type.Poison, 190, 90,
                new[] { Move.Pound, Move.Disable }, GrowthRate.MediumFast, level),
            Dex.Muk => new Pokemon(dex, "Grotadmorv", 105, 105, 75, 50, Type.Poison, Type.Poison, 75, 157,
                new[] { Move.Pound, Move.Disable, Move.PoisonGas }, GrowthRate.MediumFast, level),
            Dex.Shellder => new Pokemon(dex, "Kokiyas", 30, 65, 100, 40, Type.Water, Type.Water, 190, 97,
                new[] { Move.Tackle, Move.Withdraw }, GrowthRate.Slow, level),
            Dex.Cloyster => new Pokemon(dex, "Crustabri", 50, 95, 180, 70, Type.Water, Type.Ice, 60, 203,
                new[] { Move.Withdraw, Move.Supersonic, Move.Clamp, Move.AuroraBeam }, GrowthRate.Slow, level),
            Dex.Gastly => new Pokemon(dex, "Fantominus", 30, 35, 30, 80, Type.Ghost, Type.Poison, 190, 95,
                new[] { Move.Lick, Move.ConfuseRay, Move.NightShade }, GrowthRate.MediumSlow, level),
            Dex.Haunter => new Pokemon(dex, "Spectrum", 45, 50, 45, 95, Type.Ghost, Type.Poison, 90, 126,
                new[] { Move.Lick, Move.ConfuseRay, Move.NightShade }, GrowthRate.MediumSlow, level),
            Dex.Gengar => new Pokemon(dex, "Ectoplasma", 60, 65, 60, 110, Type.Ghost, Type.Poison, 45, 190,
                new[] { Move.Lick, Move.ConfuseRay, Move.NightShade }, GrowthRate.MediumSlow, level),
            Dex.Onix => new Pokemon(dex, "Onix", 35, 45, 160, 70, Type.Rock, Type.Ground, 45, 108,
                new[] { Move.Tackle, Move.Screech }, GrowthRate.MediumFast, level),
            Dex.Drowzee => new Pokemon(dex, "Soporifik", 60, 48, 45, 42, Type.Psychic, Type.Psychic, 190, 102,
                new[] { Move.Pound, Move.Hypnosis }, GrowthRate.MediumFast, level),
            Dex.Hypno => new Pokemon(dex, "Hypnomade", 85, 73, 70, 67, Type.Psychic, Type.Psychic, 75, 165,
                new[] { Move.Pound, Move.Hypnosis, Move.Disable, Move.Confusion }, GrowthRate.MediumFast, level),
            Dex.Krabby => new Pokemon(dex, "Krabby", 30, 105, 90, 50, Type.Water, Type.Water, 225, 115,
                new[] { Move.Bubble, Move.Leer }, GrowthRate.MediumFast, level),
            Dex.Kingler => new Pokemon(dex, "Krabboss", 55, 130, 115, 75, Type.Water, Type.Water, 60, 206,
                new[] { Move.Bubble, Move.Leer, Move.Vicegrip }, GrowthRate.MediumFast, level),
            Dex.Voltorb => new Pokemon(dex, "Voltorbe", 40, 30, 50, 100, Type.Electric, Type.Electric, 190, 103,
                new[] { Move.Tackle, Move.Screech }, GrowthRate.MediumFast, level),
            Dex.Electrode => new Pokemon(dex, "Électrode", 60, 50, 70, 140, Type.Electric, Type.Electric, 60, 150,
                new[] { Move.Tackle, Move.Screech, Move.Sonicboom }, GrowthRate.MediumFast, level),
            Dex.Exeggcute => new Pokemon(dex, "Noeunoeuf", 60, 40, 80, 40, Type.Grass, Type.Psychic, 90, 98,
                new[] { Move.Barrage, Move.Hypnosis }, GrowthRate.Slow, level),
            Dex.Exeggutor => new Pokemon(dex, "Noadkoko", 95, 95, 85, 55, Type.Grass, Type.Psychic, 45, 212,
                new[] { Move.Barrage, Move.Hypnosis }, GrowthRate.Slow, level),
            Dex.Cubone => new Pokemon(dex, "Osselait", 50, 50, 95, 35, Type.Ground, Type.Ground, 190, 87,
                new[] { Move.BoneClub, Move.Growl }, GrowthRate.MediumFast, level),
            Dex.Marowak => new Pokemon(dex, "Ossatueur", 60, 80, 110, 45, Type.Ground, Type.Ground, 75, 124,
                new[] { Move.BoneClub, Move.Growl, Move.Leer, Move.FocusEnergy }, GrowthRate.MediumFast, level),
            Dex.Hitmonlee => new Pokemon(dex, "Kicklee", 50, 120, 53, 87, Type.Fighting, Type.Fighting, 45, 139,
                new[] { Move.DoubleKick, Move.Meditate }, GrowthRate.MediumFast, level),
            Dex.Hitmonchan => new Pokemon(dex, "Tygnon", 50, 105, 79, 76, Type.Fighting, Type.Fighting, 45, 140,
                new[] { Move.CometPunch, Move.Agility }, GrowthRate.MediumFast, level),
            Dex.Lickitung => new Pokemon(dex, "Excelangue", 90, 55, 75, 30, Type.Normal, Type.Normal, 45, 127,
                new[] { Move.Wrap, Move.Supersonic }, GrowthRate.MediumFast, level),
            Dex.Koffing => new Pokemon(dex, "Smogo", 40, 65, 95, 35, Type.Poison, Type.Poison, 190, 114,
                new[] { Move.Tackle, Move.Smog }, GrowthRate.MediumFast, level),
            Dex.Weezing => new Pokemon(dex, "Smogogo", 65, 90, 120, 60, Type.Poison, Type.Poison, 60, 173,
                new[] { Move.Tackle, Move.Smog, Move.Sludge }, GrowthRate.MediumFast, level),
            Dex.Rhyhorn => new Pokemon(dex, "Rhinocorne", 80, 85, 95, 25, Type.Ground, Type.Rock, 120, 135,
                new[] { Move.HornAttack }, GrowthRate.Slow, level),
            Dex.Rhydon => new Pokemon(dex, "Rhinoféros", 105, 130, 120, 40, Type.Ground, Type.Rock, 60, 204,
                new[] { Move.HornAttack, Move.Stomp, Move.TailWhip, Move.FuryAttack }, GrowthRate.Slow, level),
            Dex.Chansey => new Pokemon(dex, "Leveinard", 250, 5, 5, 50, Type.Normal, Type.Normal, 30, 255,
                new[] { Move.Pound, Move.Doubleslap }, GrowthRate.Fast, level),
            Dex.Tangela => new Pokemon(dex, "Saquedeneu", 65, 55, 115, 60, Type.Grass, Type.Grass, 45, 166,
                new[] { Move.Constrict, Move.Bind }, GrowthRate.MediumFast, level),
            Dex.Kangaskhan => new Pokemon(dex, "Kangourex", 105, 95, 80, 90, Type.Normal, Type.Normal, 45, 175,
                new[] { Move.CometPunch, Move.Rage }, GrowthRate.MediumFast, level),
            Dex.Horsea => new Pokemon(dex, "Hypotrempe", 30, 40, 70, 60, Type.Water, Type.Water, 225, 83,
                new[] { Move.Bubble }, GrowthRate.MediumFast, level),
            Dex.Seadra => new Pokemon(dex, "Hypocéan", 55, 65, 95, 85, Type.Water, Type.Water, 75, 155,
                new[] { Move.Bubble, Move.Smokescreen }, GrowthRate.MediumFast, level),
            Dex.Goldeen => new Pokemon(dex, "Poissirène", 45, 67, 60, 63, Type.Water, Type.Water, 225, 111,
                new[] { Move.Peck, Move.TailWhip }, GrowthRate.MediumFast, level),
            Dex.Seaking => new Pokemon(dex, "Poissoroy", 80, 92, 65, 68, Type.Water, Type.Water, 60, 170,
                new[] { Move.Peck, Move.TailWhip, Move.Supersonic }, GrowthRate.MediumFast, level),
            Dex.Staryu => new Pokemon(dex, "Stari", 30, 45, 55, 85, Type.Water, Type.Water, 225, 106,
                new[] { Move.Tackle }, GrowthRate.Slow, level),
            Dex.Starmie => new Pokemon(dex, "Staross", 60, 75, 85, 115, Type.Water, Type.Psychic, 60, 207,
                new[] { Move.Tackle, Move.WaterGun, Move.Harden }, GrowthRate.Slow, level),
            Dex.MrMime => new Pokemon(dex, "M. Mime", 40, 45, 65, 90, Type.Psychic, Type.Psychic, 45, 136,
                new[] { Move.Confusion, Move.Barrier }, GrowthRate.MediumFast, level),
            Dex.Scyther => new Pokemon(dex, "Insécateur", 70, 110, 80, 105, Type.Bug, Type.Flying, 45, 187,
                new[] { Move.QuickAttack }, GrowthRate.MediumFast, level),
            Dex.Jynx => new Pokemon(dex, "Lippoutou", 65, 50, 35, 95, Type.Ice, Type.Psychic, 45, 137,
                new[] { Move.Pound, Move.LovelyKiss }, GrowthRate.MediumFast, level),
            Dex.Electabuzz => new Pokemon(dex, "Élektek", 65, 83, 57, 105, Type.Electric, Type.Electric, 45, 156,
                new[] { Move.QuickAttack, Move.Leer }, GrowthRate.MediumFast, level),
            Dex.Magmar => new Pokemon(dex, "Magmar", 65, 95, 57, 93, Type.Fire, Type.Fire, 45, 167,
                new[] { Move.Ember }, GrowthRate.MediumFast, level),
            Dex.Pinsir => new Pokemon(dex, "Scarabrute", 65, 125, 100, 85, Type.Bug, Type.Bug, 45, 200,
                new[] { Move.Vicegrip }, GrowthRate.Slow, level),
            Dex.Tauros => new Pokemon(dex, "Tauros", 75, 100, 95, 110, Type.Normal, Type.Normal, 45, 211,
                new[] { Move.Tackle }, GrowthRate.Slow, level),
            Dex.Magikarp => new Pokemon(dex, "Magicarpe", 20, 10, 55, 80, Type.Water, Type.Water, 255, 20,
                new[] { Move.Splash }, GrowthRate.Slow, level),
            Dex.Gyarados => new Pokemon(dex, "Léviator", 95, 125, 79, 81, Type.Water, Type.Flying, 45, 214,
                new[] { Move.Bite, Move.DragonRage, Move.Leer, Move.HydroPump }, GrowthRate.Slow, level),
            Dex.Lapras => new Pokemon(dex, "Lokhlass", 130, 85, 80, 60, Type.Water, Type.Ice, 45, 219,
                new[] { Move.WaterGun, Move.Growl }, GrowthRate.Slow, level),
            Dex.Ditto => new Pokemon(dex, "Métamorph", 48, 48, 48, 48, Type.Normal, Type.Normal, 35, 61,
                new[] { Move.Transform }, GrowthRate.MediumFast, level),
            Dex.Eevee => new Pokemon(dex, "Évoli", 55, 55, 50, 55, Type.Normal, Type.Normal, 45, 92,
                new[] { Move.Tackle, Move.SandAttack }, GrowthRate.MediumFast, level),
            Dex.Vaporeon => new Pokemon(dex, "Aquali", 130, 65, 60, 65, Type.Water, Type.Water, 45, 196,
                new[] { Move.Tackle, Move.SandAttack, Move.QuickAttack, Move.WaterGun }, GrowthRate.MediumFast, level),
            Dex.Jolteon => new Pokemon(dex, "Voltali", 65, 65, 60, 130, Type.Electric, Type.Electric, 45, 197,
                new[] { Move.Tackle, Move.SandAttack, Move.QuickAttack, Move.Thundershock }, GrowthRate.MediumFast,
                level),
            Dex.Flareon => new Pokemon(dex, "Pyroli", 65, 130, 60, 65, Type.Fire, Type.Fire, 45, 198,
                new[] { Move.Tackle, Move.SandAttack, Move.QuickAttack, Move.Ember }, GrowthRate.MediumFast, level),
            Dex.Porygon => new Pokemon(dex, "Porygon", 65, 60, 70, 40, Type.Normal, Type.Normal, 45, 130,
                new[] { Move.Tackle, Move.Sharpen, Move.Conversion }, GrowthRate.MediumFast, level),
            Dex.Omanyte => new Pokemon(dex, "Amonita", 35, 40, 100, 35, Type.Rock, Type.Water, 45, 120,
                new[] { Move.WaterGun, Move.Withdraw }, GrowthRate.MediumFast, level),
            Dex.Omastar => new Pokemon(dex, "Amonistar", 70, 60, 125, 55, Type.Rock, Type.Water, 45, 199,
                new[] { Move.WaterGun, Move.Withdraw, Move.HornAttack }, GrowthRate.MediumFast, level),
            Dex.Kabuto => new Pokemon(dex, "Kabuto", 30, 80, 90, 55, Type.Rock, Type.Water, 45, 119,
                new[] { Move.Scratch, Move.Harden }, GrowthRate.MediumFast, level),
            Dex.Kabutops => new Pokemon(dex, "Kabutops", 60, 115, 105, 80, Type.Rock, Type.Water, 45, 201,
                new[] { Move.Scratch, Move.Harden, Move.Absorb }, GrowthRate.MediumFast, level),
            Dex.Aerodactyl => new Pokemon(dex, "Ptéra", 80, 105, 65, 130, Type.Rock, Type.Flying, 45, 202,
                new[] { Move.WingAttack, Move.Agility }, GrowthRate.Slow, level),
            Dex.Snorlax => new Pokemon(dex, "Ronflex", 160, 110, 65, 30, Type.Normal, Type.Normal, 25, 154,
                new[] { Move.Headbutt, Move.Amnesia, Move.Rest }, GrowthRate.Slow, level),
            Dex.Articuno => new Pokemon(dex, "Artikodin", 90, 85, 100, 85, Type.Ice, Type.Flying, 3, 215,
                new[] { Move.Peck, Move.IceBeam }, GrowthRate.Slow, level),
            Dex.Zapdos => new Pokemon(dex, "Électhor", 90, 90, 85, 100, Type.Electric, Type.Flying, 3, 216,
                new[] { Move.Thundershock, Move.DrillPeck }, GrowthRate.Slow, level),
            Dex.Moltres => new Pokemon(dex, "Sulfura", 90, 100, 90, 90, Type.Fire, Type.Flying, 3, 217,
                new[] { Move.Peck, Move.FireSpin }, GrowthRate.Slow, level),
            Dex.Dratini => new Pokemon(dex, "Minidraco", 41, 64, 45, 50, Type.Dragon, Type.Dragon, 45, 67,
                new[] { Move.Wrap, Move.Leer }, GrowthRate.Slow, level),
            Dex.Dragonair => new Pokemon(dex, "Draco", 61, 84, 65, 70, Type.Dragon, Type.Dragon, 45, 144,
                new[] { Move.Wrap, Move.Leer, Move.ThunderWave }, GrowthRate.Slow, level),
            Dex.Dragonite => new Pokemon(dex, "Dracolosse", 91, 134, 95, 80, Type.Dragon, Type.Flying, 45, 218,
                new[] { Move.Wrap, Move.Leer, Move.ThunderWave, Move.Agility }, GrowthRate.Slow, level),
            Dex.Mewtwo => new Pokemon(dex, "Mewtwo", 106, 110, 90, 130, Type.Psychic, Type.Psychic, 3, 220,
                new[] { Move.Confusion, Move.Disable, Move.Swift, Move.PsychicM }, GrowthRate.Slow, level),
            Dex.Mew => new Pokemon(dex, "Mew", 100, 100, 100, 100, Type.Psychic, Type.Psychic, 45, 64,
                new[] { Move.Pound }, GrowthRate.MediumSlow, level),
            _ => throw new ArgumentOutOfRangeException(nameof(dex), dex, null)
        };
    }
}