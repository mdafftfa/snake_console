using System.Runtime.InteropServices;
using SadConsole;
using SadConsole.Configuration;
using SadConsole.UI;
using snake_console.Data;
using snake_console.Game;
using snake_console.Utils;
using snake_console.Utils.Sounds;

namespace snake_console;

class Launcher
{


    [DllImport("kernel32.dll")]
    private static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    private const int SW_HIDE = 0;
    private const int SW_SHOW = 5;

    public static void Main(String[] args)
    {
        IntPtr consoleWindow = GetConsoleWindow();
        ShowWindow(consoleWindow, SW_HIDE);

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
        var splash = new SplashScreen(new ControlsConsole(80, 25));
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
                    SadConsole.Game.Instance.Screen = new MainMenu(new ControlsConsole(20, 10), playerData);
                    GlobalAudio.PlayBgm(Resources.getMusic(Resources.lobby_bgm), playerData.Settings.LobbyBgmVolume / 100f);
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