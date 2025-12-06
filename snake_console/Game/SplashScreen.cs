using SadConsole;
using SadConsole.UI;
using SadRogue.Primitives;

namespace snake_console.Game;

using SadConsole;
using SadConsole.UI.Controls;
using SadRogue.Primitives;
using System.Threading.Tasks;

public class SplashScreen : ControlsConsole
{
    private ProgressBar _bar;

    public SplashScreen(ControlsConsole prevConsole) : base(80, 25)
    {
        prevConsole.Clear();
        prevConsole.Controls.Clear();
        prevConsole.IsFocused = false;
        prevConsole.ClearShiftValues();

        this.Print(2, 1, "SNAKE CONSOLE v1.0.0");
        this.Print(2, 17, "Please wait!");
        this.Print(2, 18, "Loading Contents...");
        _bar = new ProgressBar(80 - 4, 1, HorizontalAlignment.Left)
        {
            Position = new Point(2, 20),
            BarGlyph = 1,
            BackgroundGlyph = 0,
            DisplayText = "Loading...",
            DisplayTextColor = Color.White
        };
        _bar.BarColor = Color.Wheat;
        Controls.Add(_bar);
    }

    public async Task RunAndWaitThen(Action onFinished)
    {
        for (int i = 0; i <= 100; i++)
        {
            _bar.Progress = i / 100f;
            await Task.Delay(20);
        }

        await Task.Delay(TimeSpan.FromSeconds(new Random().Next(3, 5)));
        onFinished?.Invoke();
    }

}
