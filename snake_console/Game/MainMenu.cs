using SadConsole;
using SadConsole.UI;
using SadConsole.UI.Controls;
using SadRogue.Primitives;
using snake_console.Data;
using snake_console.Utils;
using snake_console.Utils.Sounds;

namespace snake_console.Game;

public class MainMenu : ControlsConsole
{

    private PlayerData playerData;

    public MainMenu(ControlsConsole prevConsole, PlayerData data) : base(80, 25)
    {
        prevConsole.Clear();
        prevConsole.Controls.Clear();
        prevConsole.IsFocused = false;
        prevConsole.ClearShiftValues();

        FontSize = new Point(10, 20);
        playerData = data;

        loadResources();
        init();
    }

    private void loadResources()
    {
    }

    private void init()
    {

        this.Print(2, 1, "=--------------- PROFILE ---------------=");
        this.Print(2, 3, "Player Name: ");
        this.Print(15, 3, ColoredString.FromGradient(Color.Wheat, playerData.Name));
        this.Print(2, 4, "Money:");
        this.Print(9, 4, ColoredString.FromGradient(Color.Wheat, playerData.Money.ToString("N0")));
        this.Print(2, 6, "=--------------- PROFILE ---------------=");
        this.Print(2, 8, ColoredString.FromGradient(Color.Wheat, "> Main Menu <"));

        var btnPlayGame = new SelectionButton(18, 1) {
            Position = new Point(2, 10),
            Text = "Play Game"
        };

        btnPlayGame.Click += (s,e) =>
        {
            SadConsole.Game.Instance.Screen = new SelectCharacter(this, playerData);
            LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.click_sound), 1f);
        };

        btnPlayGame.Unfocused += (s, e) =>
        {
            LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.switch_sound), 1f);
        };

        var btnMatchHistory = new SelectionButton(18, 1) {
            Position = new Point(2, 11),
            Text = "Match History"
        };
        btnMatchHistory.Click += (s,e) => {
            SadConsole.Game.Instance.Screen = new MatchHistory(this, playerData);
            LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.click_sound), 1f);
        };
        btnMatchHistory.Unfocused += (s, e) =>
        {
            LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.switch_sound), 1f);
        };

        var btnSnakes = new SelectionButton(18, 1) {
            Position = new Point(2, 12),
            Text = "Snakes"
        };
        btnSnakes.Click += (s,e) => {
            SadConsole.Game.Instance.Screen = new Snakes(this, playerData);
            LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.click_sound), 1f);
        };
        btnSnakes.Unfocused += (s, e) =>
        {
            LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.switch_sound), 1f);
        };

        var btnSettings = new SelectionButton(18, 1) {
            Position = new Point(2, 13),
            Text = "Settings"
        };
        btnSettings.Click += (s,e) => {
            SadConsole.Game.Instance.Screen = new Settings(this, playerData);
            LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.click_sound), 1f);
        };
        btnSettings.Unfocused += (s, e) =>
        {
            LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.switch_sound), 1f);
        };

        var btnExit = new SelectionButton(18, 1) {
            Position = new Point(2, 14),
            Text = "Exit"
        };
        btnExit.Click += (s, e) =>
        {
            SadConsole.Game.Instance.MonoGameInstance.Exit();

            LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.click_sound), 1f);
        };
        btnExit.Unfocused += (s, e) =>
        {
            LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.switch_sound), 1f);
        };

        var theme = Colors.Default.Clone();
        theme.ControlBackgroundNormal.SetColor(Color.Gray);
        theme.RebuildAppearances();

        var customColors = SadConsole.UI.Colors.Default.Clone();
        customColors.ControlBackgroundNormal.SetColor(Color.Black);
        customColors.ControlForegroundNormal.SetColor(Color.DarkGray);
        customColors.ControlBackgroundFocused.SetColor(Color.Black);
        customColors.ControlForegroundFocused.SetColor(Color.Wheat);
        customColors.RebuildAppearances();

        btnPlayGame.SetThemeColors(customColors);
        btnMatchHistory.SetThemeColors(customColors);
        btnSnakes.SetThemeColors(customColors);
        btnSettings.SetThemeColors(customColors);
        btnExit.SetThemeColors(customColors);

        btnPlayGame.NextSelection = btnMatchHistory;
        btnPlayGame.PreviousSelection = btnExit;

        btnMatchHistory.NextSelection = btnSnakes;
        btnMatchHistory.PreviousSelection = btnPlayGame;

        btnSnakes.NextSelection = btnSettings;
        btnSnakes.PreviousSelection = btnMatchHistory;

        btnSettings.NextSelection = btnExit;
        btnSettings.PreviousSelection = btnSnakes;

        btnExit.NextSelection = btnPlayGame;
        btnExit.PreviousSelection = btnSettings;

        btnPlayGame.Unfocused += (s, e) => {  };

        btnPlayGame.IsFocused = true;

        Controls.Add(btnPlayGame);
        Controls.Add(btnMatchHistory);
        Controls.Add(btnSnakes);
        Controls.Add(btnSettings);
        Controls.Add(btnExit);

        Controls.FocusedControl = btnPlayGame;
        IsFocused = true;

        SadConsole.Game.Instance.Screen = this;
    }

}