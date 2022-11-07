using PokemonRPG.Data;
using PokemonRPG.Enums;
using PokemonRPG.UI;

namespace PokemonRPG;

public static class Game
{
    private static Starter _pokemon = null!;

    public static void New()
    {
        Ui.ShowMenu(new Menu("Choisis ton pokémon", new[]
        {
            new MenuItem("Bulbizarre", () => _pokemon = Starter.GetPokemon(StarterPokemon.Bulbasaur)),
            new MenuItem("Salamèche", () => _pokemon = Starter.GetPokemon(StarterPokemon.Charmander)),
            new MenuItem("Carapuce", () => _pokemon = Starter.GetPokemon(StarterPokemon.Squirtle))
        }));
        Ui.PrintNotification($"{_pokemon.Name} est maintenant à toi !");
        MainMenu();
    }

    private static void MainMenu()
    {
        while (true)
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(
                $"{_pokemon.Name} HP: {_pokemon.Hp.Value}/{_pokemon.MaxHp} LV: {_pokemon.Level} ATK: {_pokemon.Attack.Value} DEF: {_pokemon.Defense.Value} SPD: {_pokemon.Speed.Value} SPC: {_pokemon.Special.Value}          ");
            Console.WriteLine($"Encore {_pokemon.GetNextLevelExp()} EXP avant le prochain niveau");
            Ui.ShowMenu(new Menu("Menu Principal", new[]
            {
                new MenuItem("Affronter un pokémon", () =>
                {
                    if (_pokemon.Hp.Value == 0)
                        Ui.PrintNotification($"{_pokemon.Name} est K.O. et ne peut pas se battre.");
                    else
                        new Battle(_pokemon,
                                Wild.GetRandom(Random.Shared.Next(Math.Max(5, _pokemon.Level - 10),
                                    _pokemon.Level + 1)))
                            .MainMenu();
                }),
                // In order to respect the instructions of the exercise
                new MenuItem("Voir les stats de mon pokémon", () =>
                {
                    Ui.PrintNotification(
                        $"{_pokemon.Name} HP: {_pokemon.Hp.Value}/{_pokemon.MaxHp} LV: {_pokemon.Level} ATK: {_pokemon.Attack.Value} DEF: {_pokemon.Defense.Value} SPD: {_pokemon.Speed.Value} SPC: {_pokemon.Special.Value}");
                }),
                new MenuItem("Soigner mon pokémon", Heal)
            }));
            Console.Clear();
        }
    }

    private static void Heal()
    {
        _pokemon.Hp.Value = _pokemon.MaxHp;
        foreach (var move in _pokemon.Moves)
            move.PowerPoints = move.MaxPowerPoints;
        Ui.PrintNotification("Pokémon soigné !");
    }
}