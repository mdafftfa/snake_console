using SadConsole.UI;
using SadRogue.Primitives;
using snake_console.Data;
using snake_console.Utils;

namespace snake_console.Game;

public class Settings : ControlsConsole
{

    private AudioSystem audio;
    private PlayerData playerData;

    public Settings(PlayerData data) : base(80, 25)
    {
        FontSize = new Point(10, 20);
        playerData = data;

        loadResources();
        init();
    }

    private void loadResources()
    {
        audio = new AudioSystem();
    }

    private void init()
    {

    }

}