using SadConsole;
using SadConsole.Configuration;
using snake_console.Data;
using snake_console.Game;

namespace snake_console;

class Launcher
{

    public static void Main(String[] args)
    {
        SadConsole.Settings.WindowTitle = "Snake Console";
        SadConsole.Settings.AllowWindowResize = false;

        Builder
            .GetBuilder()
            .SetScreenSize(80, 25)
            .ConfigureFonts(true)
            .UseDefaultConsole()
            .OnStart(OnStart)
            .OnEnd(OnEnd)
            .Run();
        SadConsole.Game.Instance.MonoGameInstance.Window.AllowAltF4 = true;
    }

    private static async void OnStart(object? sender, GameHost host)
    {
        var splash = new SplashScreen();
        SadConsole.Game.Instance.Screen = splash;
        await splash.RunAndWaitThen(() =>
        {
            PlayerDataManager playerDataManager = new PlayerDataManager();
            var path = playerDataManager.GetSavePath();
            if (File.Exists(path))
            {
                var loaded = playerDataManager.Load();
                if (loaded is null)
                {
                    SadConsole.Game.Instance.Screen = new CreateAccount();
                }
                else
                {
                    var playerData = playerDataManager.Load();
                    SadConsole.Game.Instance.Screen = new MainMenu(playerData);
                }
            }
            else
            {
                SadConsole.Game.Instance.Screen = new CreateAccount();
            }
        });
    }

    private static void OnEnd(object? sender, GameHost host)
    {

    }

}