namespace PokemonRPG.UI;

public static class Ui
{
    private static int _titlePosition;
    private static int _menuPosition;
    private static int _selectedIndex;
    private static Menu _menu = null!;
    public static void ShowMenu(Menu menu, bool clearConsole = false)
    {
        if (clearConsole)
            Console.Clear();
        Console.CursorVisible = false;
        _menu = menu;
        _titlePosition = Console.CursorTop;
        Console.WriteLine(_menu.Title);
        _menuPosition = Console.CursorTop;
        _selectedIndex = 0;
        UpdateMenu();
        ClearMenu();
    }

    private static void UpdateMenu()
    {
        do
        {
            Console.SetCursorPosition(0, _menuPosition);
            for (var i = 0; i < _menu.MenuItems.Length; i++)
            {
                Console.Write("".PadRight(_menu.MenuItems[i].Name.Length + 2, ' '));
                Console.CursorLeft = 0;
                if (_selectedIndex == i)
                    Console.Write("> ");
                Console.WriteLine(_menu.MenuItems[i].Name);
            }
        } while (HandleKey());
    }

    private static void ClearMenu()
    {
        Console.SetCursorPosition(0, _titlePosition);
        for(int i = _titlePosition; i <= _menuPosition; i++)
            Console.WriteLine("".PadRight(Console.BufferWidth, ' '));
        Console.SetCursorPosition(0, _menuPosition);
        foreach (var menuItem in _menu.MenuItems)
            Console.WriteLine("".PadRight(menuItem.Name.Length + 2, ' '));
        Console.SetCursorPosition(0, _titlePosition);
    }

    public static void PrintNotification(string message)
    {
        Console.CursorLeft = 0;
        foreach (char c in message)
        {
            Console.Write(c);
            Thread.Sleep(10);
        }

        Thread.Sleep(1000);
        Console.CursorLeft = 0;
        Console.Write("".PadRight(message.Length, ' '));
    }
    private static bool HandleKey()
    {
        var key = Console.ReadKey(false).Key;
        switch (key)
        {
            case ConsoleKey.Enter:
                if (_menu.MenuItems[_selectedIndex].Action == null)
                    return true;
                ClearMenu();
                _menu.MenuItems[_selectedIndex].Action!.Invoke();
                return false;
            case ConsoleKey.Backspace:
                if (_menu.Action == null)
                    return true;
                ClearMenu();
                _menu.Action.Invoke();
                return false;
            case ConsoleKey.UpArrow:
                if (_selectedIndex > 0)
                    _selectedIndex--;
                else
                    _selectedIndex = _menu.MenuItems.Length - 1;
                return true;
            case ConsoleKey.DownArrow:
                if (_selectedIndex < _menu.MenuItems.Length - 1)
                    _selectedIndex++;
                else
                    _selectedIndex = 0;
                return true;
            default:
                return true;
        }
    }
}