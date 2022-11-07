using PokemonRPG.Data;
using PokemonRPG.Enums;
using PokemonRPG.UI;

namespace PokemonRPG;

public static class Game
{
    private static readonly List<Pokemon> Party = new();

    public static void New()
    {
        Ui.ShowMenu(new Menu("Choisis ton pokémon", new[]
        {
            new MenuItem("Bulbizarre", () => Party.Add(Pokemon.GetPokemon(Dex.Bulbasaur))),
            new MenuItem("Salamèche", () => Party.Add(Pokemon.GetPokemon(Dex.Charmander))),
            new MenuItem("Carapuce", () => Party.Add(Pokemon.GetPokemon(Dex.Squirtle)))
        }));
        Ui.PrintNotification($"{Party[0].Name} est maintenant à toi !");
        MainMenu();
    }

    private static void MainMenu()
    {
        while (true)
        {
            Console.SetCursorPosition(0, 0);
            Ui.ShowMenu(new Menu("Menu Principal", new[]
            {
                new MenuItem("Affronter un pokémon", () =>
                {
                    if (Party.Count == 1 && Party[0].Hp.Value == 0)
                        Ui.PrintNotification($"{Party[0].Name} est K.O. et ne peut plus se battre.");
                    else if (!Party.Select(x => x.Hp.Value > 0).Any())
                        Ui.PrintNotification("Tous tes pokemon sont K.O. et ne peuvent plus se battre.");
                    else
                    {
                        int max = Party.Max(x => x.Level);
                        new Battle(Party,
                                Pokemon.GetRandomWildPokemon(Random.Shared.Next(Math.Max(5, max - 10),
                                    max + 1)))
                            .MainMenu();
                    }
                }),
                new MenuItem("Statistiques de mes pokémon", ShowStatistics),
                new MenuItem("Soigner mes pokémon", Heal)
            }));
            Console.Clear();
        }
    }

    private static void ShowStatistics()
    {
        var menuItems = new MenuItem[Party.Count];
        for (var i = 0; i < Party.Count; i++)
        {
            menuItems[i] = new MenuItem(
                    $"{Party[i].Name} " +
                    $"HP: {Party[i].Hp.Value}/{Party[i].MaxHp} LV: {Party[i].Level} " +
                    $"EXP: {Party[i].Exp} Prochain niveau dans {Party[i].GetNextLevelExp()} EXP " +
                    $"ATK: {Party[i].Attack.Value} DEF: {Party[i].Defense.Value} SPD: {Party[i].Speed.Value}");
        }

        Ui.ShowMenu(new Menu("Statistiques de mes pokémon", menuItems, MainMenu));
    }

    private static void Heal()
    {
        foreach (var pokemon in Party)
        {
            pokemon.Hp.Value = pokemon.MaxHp;
            foreach (var move in pokemon.Moves)
                move.PowerPoints = move.MaxPowerPoints;
        }

        Ui.PrintNotification("Tous les pokémon ont été soignés !");
    }
}