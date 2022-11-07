namespace PokemonRPG.UI;

public class Menu
{
    public Menu(string title, MenuItem[] menuItems, Action? action = null)
    {
        Title = title;
        MenuItems = menuItems;
        Action = action;
    }

    public string Title { get; }
    public MenuItem[] MenuItems { get; }
    public Action? Action { get; }
}