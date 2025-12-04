using SadConsole;
using SadConsole.Configuration;
using SadRogue.Primitives;
using snake_console.Utils;

namespace snake_console.Game;

public class Game
{
    private int scores;
    private int seconds;

    public Game()
    {
        SadConsole.Settings.WindowTitle = "Snake Console | Scores: "+ scores +" | Game Ends In: "+ Time.Format(seconds);
        SadConsole.Settings.AllowWindowResize = false;

        Builder
            .GetBuilder()
            .SetScreenSize(80, 25)
            .ConfigureFonts()
            .UseDefaultConsole()
            .OnStart(OnStart)
            .OnEnd(OnEnd)
            .Run();
        SadConsole.Game.Instance.MonoGameInstance.Window.AllowAltF4 = true;
    }

    private void OnStart(object? sender, GameHost host)
    {

        var console = SadConsole.Game.Instance.StartingConsole;
        console.Print(2, 2, ColoredString.FromGradient(new Gradient(Color.Orange, Color.OrangeRed, Color.OrangeRed), "◼asdasd"));
    }

    private void OnEnd(object? sender, GameHost host)
    {
    }
}