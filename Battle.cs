using System.Text;
using PokemonRPG.Data;
using PokemonRPG.Enums;
using PokemonRPG.UI;

namespace PokemonRPG;

public class Battle
{
    private static int _potions;
    private static int _pokeballs;
    private readonly List<Pokemon> _party;
    private int _pokemonIndex;
    private readonly Pokemon _wildPokemon;
    private int _attempts;
    private int _round = 1;

    public Battle(List<Pokemon> party, Pokemon wildPokemon)
    {
        _party = party;
        _wildPokemon = wildPokemon;
    }

    public void MainMenu()
    {
        ShowStatistics();
        Ui.ShowMenu(new Menu("Affronter un pokémon", new[]
        {
            new MenuItem("Attaquer", Fight),
            new MenuItem("Changer de pokemon", Switch),
            new MenuItem($"Utiliser une potion ({_potions})", Potion),
            new MenuItem($"Utiliser une pokeball ({_pokeballs})", Pokeball),
            new MenuItem("Fuir", Escape)
        }));
    }

    private void Switch()
    {
        var menuItems = new MenuItem[_party.Count];
        for (var i = 0; i < _party.Count; i++)
        {
            int index = i;
            menuItems[i] =
                new MenuItem(
                    $"{_party[i].Name} HP: {_party[i].Hp.Value}/{_party[i].MaxHp} LV: {_party[i].Level} ATK: {_party[i].Attack.Value} DEF: {_party[i].Defense.Value} SPD: {_party[i].Speed.Value}",
                    () => _pokemonIndex = index);
        }

        Ui.ShowMenu(new Menu("Changer de pokemon", menuItems, MainMenu));
        MainMenu();
    }

    private void ShowStatistics()
    {
        Pokemon[] pokemons = { _party[_pokemonIndex], _wildPokemon };
        StringBuilder sb = new();
        sb.AppendLine($"Manche : {_round}");
        foreach (var pokemon in pokemons)
        {
            sb.AppendLine(
                $"{pokemon.Name} HP: {pokemon.Hp.Value}/{pokemon.MaxHp} LV: {pokemon.Level} ATK: {pokemon.Attack.Value} DEF: {pokemon.Defense.Value} SPD: {pokemon.Speed.Value}            ");
#if DEBUG
            sb.AppendLine(
                $"BS: HP: {pokemon.Hp.Base} ATK: {pokemon.Attack.Base} DEF: {pokemon.Defense.Base} SPD: {pokemon.Speed.Base}");
            sb.AppendLine(
                $"IV: HP: {pokemon.Hp.Iv} ATK: {pokemon.Attack.Iv} DEF: {pokemon.Defense.Iv} SPD: {pokemon.Speed.Iv}");
#endif
        }

        Console.SetCursorPosition(0, 0);
        Console.Write("".PadLeft(Console.BufferWidth, ' '));
        Console.SetCursorPosition(0, 0);
        Console.Write(sb);
    }

    private void Fight()
    {
        ShowStatistics();

        // For now, only use attacks that do damage because the effects are not implemented
        var menuItems = _party[_pokemonIndex].Moves
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


        var wildMove = _wildPokemon.GetRandomMove();
        bool startsFirst;
        if (move.Move == Move.QuickAttack && wildMove.Move != Move.QuickAttack)
        {
            startsFirst = true;
        }
        else if (wildMove.Move == Move.QuickAttack && move.Move != Move.QuickAttack)
        {
            startsFirst = false;
        }
        else
        {
            if (_party[_pokemonIndex].Speed.Value == _wildPokemon.Speed.Value)
                startsFirst = Random.Shared.Next(2) == 0;
            else
                startsFirst = _party[_pokemonIndex].Speed.Value > _wildPokemon.Speed.Value;
        }

        if (startsFirst)
        {
            ExecuteMove(move, _party[_pokemonIndex], _wildPokemon);
            move.PowerPoints--;
            if (_wildPokemon.Hp.Value == 0)
            {
                WonBattle();
                return;
            }

            ExecuteMove(wildMove, _wildPokemon, _party[_pokemonIndex]);
            if (_party[_pokemonIndex].Hp.Value == 0)
            {
                Ui.PrintNotification($"{_party[_pokemonIndex].Name} est K.O. !");
                return;
            }
        }
        else
        {
            ExecuteMove(wildMove, _wildPokemon, _party[_pokemonIndex]);
            if (_party[_pokemonIndex].Hp.Value == 0)
            {
                Ui.PrintNotification($"{_party[_pokemonIndex].Name} est K.O. !");
                return;
            }

            ExecuteMove(move, _party[_pokemonIndex], _wildPokemon);
            move.PowerPoints--;
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
        _party[_pokemonIndex].CheckEvolutionsAndMoves();
        int level = _party[_pokemonIndex].Level;
        int gain = _party[_pokemonIndex].AddExpGain(_wildPokemon);
        Ui.PrintNotification($"{_party[_pokemonIndex].Name} gagne {gain} points d'EXP !");
        if (level != _party[_pokemonIndex].Level)
            Ui.PrintNotification($"{_party[_pokemonIndex].Name} monte au niveau {_party[_pokemonIndex].Level} !");
        AddPotionPokeball();
    }

    private static void AddPotionPokeball()
    {
        if (Random.Shared.NextDouble() > .75)
        {
            _potions++;
            Ui.PrintNotification("Vous avez obtenu une potion !");
        }

        if (Random.Shared.NextDouble() > .75)
        {
            _pokeballs++;
            Ui.PrintNotification("Vous avez obtenu une pokeball !");
        }
    }

    private static void ExecuteMove(MoveClass move, Pokemon attacker, Pokemon defender)
    {
        Ui.PrintNotification(
            $"{attacker.Name} {(attacker.IsWild ? "ennemi " : "")}lance {move.Name}");
        // https://bulbapedia.bulbagarden.net/wiki/Accuracy#Generation_I_and_II
        bool missed = Random.Shared.Next(100) < move.Accuracy;

        if (missed)
        {
            int damage = CalculateDamage(move, attacker, defender);
            defender.Hp.Value -= damage;
        }
        else
        {
            Ui.PrintNotification("Mais échoue !");
        }
    }


    private static int CalculateDamage(MoveClass move, Pokemon attacker, Pokemon target)
    {
        // https://bulbapedia.bulbagarden.net/wiki/Damage
        // STAB & TYPE MATCH UP
        double type1Effectiveness = move.GetEffectiveness(target.Type1);
        double type2Effectiveness = target.Type1 == target.Type2 ? 1 : move.GetEffectiveness(target.Type2);
        double typeEffectiveness = type1Effectiveness * type2Effectiveness;
        if (typeEffectiveness >= 2)
        {
            Ui.PrintNotification("C'est très efficace !");
        }
        else if (typeEffectiveness <= 0.5)
        {
            Ui.PrintNotification("Ce n'est pas très efficace...");
        }
        else if (type1Effectiveness == 0)
        {
            Ui.PrintNotification($"Pas d'effet sur {target.Name} !");
            return 0;
        }

        double stab = move.Type == attacker.Type1 || move.Type == attacker.Type2 ? 1.5 : 1;

        // https://bulbapedia.bulbagarden.net/wiki/Critical_hit#Probability
        // CRITICAL HIT TEST
        var threshold = (int)(attacker.Speed.Base / (double)2);

        if (move.HighCriticalHit)
            threshold = Math.Min(8 * threshold, 255);


        bool criticalHit = Random.Shared.Next(256) < threshold;

        // GET DAMAGE VARIABLES
        double level = criticalHit ? attacker.Level * 2 : attacker.Level;

        double a = attacker.Attack.Value;
        double d = attacker.Defense.Value;


        if (a > 255 || d > 255)
        {
            a = Math.Floor(a / 4) % 256;
            d = Math.Floor(d / 4) % 256;
        }

        // DAMAGE CALCULATION

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

    private void Pokeball()
    {
        if (_pokeballs > 0)
        {
            _pokeballs--;
            // https://bulbapedia.bulbagarden.net/wiki/Catch_rate#Capture_method_.28Generation_I.29
            int r1 = Random.Shared.Next(0, 256);
            int f = Math.Min(255, _wildPokemon.MaxHp * 255 * 4 / (_wildPokemon.Hp.Value * 12));
            int r2 = Random.Shared.Next(0, 256);
            if (_wildPokemon.CatchRate < r1 && r2 > f)
            {
                Ui.PrintNotification($"Vous n'avez pas réussi à attraper {_wildPokemon.Name} !");
                var wildMove = _wildPokemon.GetRandomMove();
                ExecuteMove(wildMove, _wildPokemon, _party[_pokemonIndex]);
                if (_party[_pokemonIndex].Hp.Value == 0)
                {
                    Ui.PrintNotification($"{_party[_pokemonIndex].Name} est K.O. !");
                    return;
                }
                _round++;
            }
            else
            {
                _wildPokemon.IsWild = false;
                _party.Add(_wildPokemon);
                Ui.PrintNotification($"Vous avez attrapé {_wildPokemon.Name} !");
            }
        }
        else
        {
            Ui.PrintNotification("Vous n'avez pas de pokeball !");
            MainMenu();
        }
    }

    private void Potion()
    {
        if (_potions > 0)
        {
            _party[_pokemonIndex].Hp.Value = Math.Min(_party[_pokemonIndex].MaxHp, _party[_pokemonIndex].Hp.Value + 20);
            _potions--;
            Ui.PrintNotification("Vous avez utilisé une potion.");
            var wildMove = _wildPokemon.GetRandomMove();
            ExecuteMove(wildMove, _wildPokemon, _party[_pokemonIndex]);
            if (_party[_pokemonIndex].Hp.Value == 0)
            {
                Ui.PrintNotification($"{_party[_pokemonIndex].Name} est K.O. !");
                return;
            }

            _round++;
        }
        else
        {
            Ui.PrintNotification("Vous n'avez pas de potion !");
        }

        MainMenu();
    }

    private void Escape()
    {
        // https://bulbapedia.bulbagarden.net/wiki/Escape#Generation_I_and_II
        int odds = _wildPokemon.Speed.Value / 4 % 256 == 0
            ? 256
            : _party[_pokemonIndex].Speed.Value * 32 / (_wildPokemon.Speed.Value / 4 % 256) + 30 + _attempts++;
        if (odds > 255 || Random.Shared.Next(256) < odds)
        {
            Ui.PrintNotification("Vous prenez la fuite !");
            return;
        }

        Ui.PrintNotification("Fuite impossible !");
        var wildMove = _wildPokemon.GetRandomMove();
        ExecuteMove(wildMove, _wildPokemon, _party[_pokemonIndex]);
        if (_party[_pokemonIndex].Hp.Value == 0)
        {
            Ui.PrintNotification($"{_party[_pokemonIndex].Name} est K.O. !");
            return;
        }

        _round++;
        MainMenu();
    }
}