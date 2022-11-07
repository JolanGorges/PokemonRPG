namespace PokemonRPG.UI;

public class MenuItem
{
    public MenuItem(string name, Action? action = null)
    {
        Name = name;
        Action = action;
    }

    public string Name { get; }
    public Action? Action { get; }
}