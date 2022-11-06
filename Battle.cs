using System.Text;
using PokemonRPG.Data;
using PokemonRPG.Enums;
using PokemonRPG.UI;

namespace PokemonRPG;

public class Battle
{
    private static int _potions;
    private readonly Starter _pokemon;
    private readonly Wild _wildPokemon;
    private int _attempts;
    private int _round = 1;
    public Battle(Starter pokemon, Wild wildPokemon)
    {
        _pokemon = pokemon;
        _wildPokemon = wildPokemon;
        _pokemon.Hp.Stage = 0;
        _pokemon.Attack.Stage = 0;
        _pokemon.Defense.Stage = 0;
        _pokemon.Speed.Stage = 0;
        _pokemon.Special.Stage = 0;
        _pokemon.Accuracy.Stage = 0;
        _pokemon.Evasion.Stage = 0;
    }

    public void MainMenu()
    {
        ShowStats();
        Ui.ShowMenu(new Menu("Affronter un pokémon", new[]
        {
            new MenuItem("Attaquer", Fight),
            new MenuItem("Changer de pokémon", Switch),
            new MenuItem($"Utiliser une potion ({_potions})", Potion),
            new MenuItem("Fuir", Escape)
        }));
    }

    private void ShowStats()
    {
        Pokemon[] pokemons = { _pokemon, _wildPokemon };
        StringBuilder sb = new();
        sb.AppendLine($"Manche : {_round}");
        foreach (var pokemon in pokemons)
        {
#if DEBUG
            sb.AppendLine(
                $"{pokemon.Name} HP: {pokemon.Hp.Value}/{pokemon.MaxHp} LV: {pokemon.Level} ATK: {pokemon.Attack.Value} DEF: {pokemon.Defense.Value} SPD: {pokemon.Speed.Value} SPC: {pokemon.Special.Value}          ");
            sb.AppendLine(
                $"BS: HP: {pokemon.Hp.Base} ATK: {pokemon.Attack.Base} DEF: {pokemon.Defense.Base} SPD: {pokemon.Speed.Base} SPC: {pokemon.Special.Base}");
            sb.AppendLine(
                $"IV: HP: {pokemon.Hp.Iv} ATK: {pokemon.Attack.Iv} DEF: {pokemon.Defense.Iv} SPD: {pokemon.Speed.Iv} SPC: {pokemon.Special.Iv}");
#else
            sb.AppendLine($"{pokemon.Name} HP: {pokemon.Hp.Value}/{pokemon.MaxHp} LV: {pokemon.Level}          ");
#endif
        }

        Console.SetCursorPosition(0, 0);
        Console.Write("".PadLeft(Console.BufferWidth, ' '));
        Console.SetCursorPosition(0, 0);
        Console.Write(sb);
    }

    private void Fight()
    {
        ShowStats();

        // For now, only use attacks that do damage because the effects are not implemented
        var menuItems = _pokemon.Moves
            .Where(move => move.Category == MoveCategory.Physical && move.Power > 0)
            .Select(move => new MenuItem($"{move.Name} ({move.PowerPoints}/{move.MaxPowerPoints})", () => Round(move)))
            .ToList();
        Ui.ShowMenu(new Menu("Attaquer", menuItems.ToArray(), MainMenu));
    }


    private void Round(MoveClass move)
    {
        if (move.PowerPoints <= 0)
        {
            Ui.PrintNotification("Plus de points de pouvoir !");
            Fight();
            return;
        }

        _pokemon.Hp.Current = (int)(_pokemon.Hp.Value * GetStageMultiplier(_pokemon.Hp.Stage));
        _pokemon.Attack.Current = (int)(_pokemon.Attack.Value * GetStageMultiplier(_pokemon.Attack.Stage));
        _pokemon.Defense.Current = (int)(_pokemon.Defense.Value * GetStageMultiplier(_pokemon.Defense.Stage));
        _pokemon.Speed.Current = (int)(_pokemon.Speed.Value * GetStageMultiplier(_pokemon.Speed.Stage));
        _pokemon.Special.Current = (int)(_pokemon.Special.Value * GetStageMultiplier(_pokemon.Special.Stage));
        
        var wildMove = _wildPokemon.GetRandomMove();
        bool startsFirst;
        if (move.Move == Move.QuickAttack && wildMove.Move != Move.QuickAttack)
            startsFirst = true;
        else if (wildMove.Move == Move.QuickAttack && move.Move != Move.QuickAttack)
            startsFirst = false;
        else
        {
            if (_pokemon.Speed.Current == _wildPokemon.Speed.Current)
                startsFirst = Random.Shared.Next(2) == 0;
            else
                startsFirst = _pokemon.Speed.Current > _wildPokemon.Speed.Current;
        }

        if (startsFirst)
        {
            ExecuteMove(move, _pokemon, _wildPokemon);
            if (_wildPokemon.Hp.Value == 0)
            {
                WonBattle();
                return;
            }

            ExecuteMove(wildMove, _wildPokemon, _pokemon);
            if (_pokemon.Hp.Value == 0)
            {
                Ui.PrintNotification($"{_pokemon} est K.O. !");
                return;
            }
        }
        else
        {
            ExecuteMove(wildMove, _wildPokemon, _pokemon);
            if (_pokemon.Hp.Value == 0)
            {
                Ui.PrintNotification($"{_pokemon} est K.O. !");
                return;
            }

            ExecuteMove(move, _pokemon, _wildPokemon);
            if (_wildPokemon.Hp.Value == 0)
            {
                WonBattle();
                return;
            }
        }

        _round++;
        MainMenu();
    }

    private void WonBattle()
    {
        Ui.PrintNotification($"Le {_wildPokemon.Name} ennemi est K.O. !");
        _pokemon.CheckEvolutionsAndMoves();
        int level = _pokemon.Level;
        int gain = _pokemon.AddExpGain(_wildPokemon);
        Ui.PrintNotification($"{_pokemon.Name} gagne {gain} points d'EXP !");
        if(level != _pokemon.Level)
            Ui.PrintNotification($"{_pokemon.Name} monte au niveau {_pokemon.Level} !");
        AddPotion();
    }
    private void AddPotion()
    {
        if (Random.Shared.NextDouble() > .75)
        {
            _potions++;
            Ui.PrintNotification("Vous avez obtenu une potion !");
        }
    }
    private static void ExecuteMove(MoveClass move, Pokemon attacker, Pokemon defender)
    {
        Ui.PrintNotification(
            $"{attacker.Name} {(attacker is Wild ? "ennemi " : "")}lance {move.Name}");

        // https://bulbapedia.bulbagarden.net/wiki/Accuracy#Generation_I_and_II
        var accuracy = (int)(move.Accuracy * GetStageMultiplier(attacker.Accuracy.Stage - defender.Accuracy.Stage));
        accuracy = Math.Min(100, accuracy);
        bool missed = Random.Shared.Next(100) < accuracy;

        if (missed)
        {
            int damage = CalculateDamage(move, attacker, defender);
            defender.Hp.Value -= damage;
        }
        else
            Ui.PrintNotification("Mais échoue !");
    }

    private static double GetStageMultiplier(int stage)
    {
        return stage switch
        {
            -6 => 25 / 100,
            -5 => 28 / 100,
            -4 => 33 / 100,
            -3 => 40 / 100,
            -2 => 50 / 100,
            -1 => 66 / 100,
            0 => 1 / 1,
            1 => 15 / 10,
            2 => 2 / 1,
            3 => 25 / 10,
            4 => 3 / 1,
            5 => 35 / 10,
            6 => 4 / 1,
            _ => throw new ArgumentOutOfRangeException(nameof(stage), stage, null)
        };
    }

    private static int CalculateDamage(MoveClass move, Pokemon attacker, Pokemon target)
    {
        // https://bulbapedia.bulbagarden.net/wiki/Damage

        // https://bulbapedia.bulbagarden.net/wiki/Critical_hit#Probability


        // STAB & TYPE MATCH UP
        double type1Effectiveness = move.GetEffectiveness(target.Type1);
        double type2Effectiveness = target.Type1 == target.Type2 ? 1 : move.GetEffectiveness(target.Type2);
        double typeEffectiveness = type1Effectiveness * type2Effectiveness;
        if (typeEffectiveness >= 2)
            Ui.PrintNotification("C'est très efficace !");
        else if (typeEffectiveness <= 0.5)
            Ui.PrintNotification("Ce n'est pas très efficace...");
        else if (type1Effectiveness == 0)
        {
            Ui.PrintNotification($"Pas d'effet sur {target.Name} !");
            return 0;
        }

        double stab = move.Type == attacker.Type1 || move.Type == attacker.Type2 ? 1.5 : 1;

        // CRITICAL HIT TEST
        var threshold = (int)(attacker.Speed.Base / (double)2);
        
        if (move.HighCriticalHit)
            threshold = Math.Min(8 * threshold, 255);


        bool criticalHit = Random.Shared.Next(256) < threshold;

        // GET DAMAGE VARIABLES

        double a;
        double d;
        double level = criticalHit ? attacker.Level * 2 : attacker.Level;
        if (move.Category == MoveCategory.Physical)
        {
            a = criticalHit ? attacker.Attack.Current : attacker.Attack.Value;
            d = criticalHit ? attacker.Defense.Current : attacker.Defense.Value;
        }
        else
        {
            a = criticalHit ? attacker.Special.Current : attacker.Special.Value;
            d = a;
        }

        if (a > 255 || d > 255)
        {
            a = Math.Floor(a / 4) % 256;
            d = Math.Floor(d / 4) % 256;
        }

        // 3. DAMAGE CALCULATION

        double damage = Math.Floor(Math.Floor(2 * level / 5 + 2) * Math.Max(1, a) * move.Power / Math.Max(1, d)) / 50;
        damage = Math.Min(997, damage) + 2;

        damage = (int)(damage * stab * typeEffectiveness);

        if ((int)damage == 1)
            return 1;

        damage = Math.Floor(damage * Random.Shared.Next(217, 256) / 255);

        if (criticalHit)
            Ui.PrintNotification("Coup critique !");

        return (int)damage;
    }

    private static void Switch()
    {
        throw new NotImplementedException();
    }

    private void Potion()
    {
        if (_potions > 0)
        {
            _pokemon.Hp.Value = Math.Min(_pokemon.MaxHp, _pokemon.Hp.Value + 20);
            Ui.PrintNotification("Vous avez utilisé une potion.");
        }
        else
            Ui.PrintNotification("Vous n'avez pas de potion !");
        MainMenu();
    }

    private void Escape()
    {
        // https://bulbapedia.bulbagarden.net/wiki/Escape#Generation_I_and_II
        int odds = _wildPokemon.Speed.Value / 4 % 256 == 0
            ? 256
            : _pokemon.Speed.Value * 32 / (_wildPokemon.Speed.Value / 4 % 256) + 30 + _attempts++;
        if (odds > 255 || Random.Shared.Next(256) < odds)
        {
            Ui.PrintNotification("Vous prenez la fuite !");
            return;
        }

        Ui.PrintNotification("Fuite impossible !");
        MainMenu();
    }
}