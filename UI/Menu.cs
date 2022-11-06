namespace PokemonRPG.UI;

public class Menu
{
    public string Title { get; }
    public MenuItem[] MenuItems { get; }
    public Action? Action { get; }

    public Menu(string title, MenuItem[] menuItems, Action? action = null)
    {
        Title = title;
        MenuItems = menuItems;
        Action = action;
    }
}

public class MenuItem
{
    public string Name { get; }
    public Action? Action { get; }

    public MenuItem(string name, Action? action = null)
    {
        Name = name;
        Action = action;
    }
}