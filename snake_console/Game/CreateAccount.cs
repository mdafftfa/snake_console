using SadConsole;
using SadConsole.Input;
using SadConsole.UI;
using SadConsole.UI.Controls;
using SadRogue.Primitives;
using snake_console.Data;
using snake_console.Utils;
using snake_console.Utils.Sounds;

namespace snake_console.Game;

public class CreateAccount : ControlsConsole
{

    private TextBox boxName;
    private SelectionButton btnContinue;
    private SelectionButton btnExitGame;
    private AudioSystem _audioSystem;


    public CreateAccount() : base(80, 25)
    {
        FontSize = new Point(10, 20);

        init();
        loadResources();
    }

    private void loadResources()
    {
        _audioSystem = new AudioSystem();
    }

    private void init()
    {
        this.Print(2, 1, ColoredString.FromGradient(Color.Wheat, "Hello, this is your first time playing this game!"));
        this.Print(2, 3, "This game is created by using SadConsole and C# (.NET 8).");
        this.Print(2, 4, "SadConsole is a .NET library that emulates classic ASCII");
        this.Print(2, 5, "or console-style games, and also support multiple platforms");
        this.Print(2, 6, "like (Windows, Linux and MacOS).");
        this.Print(2, 8, "Because of SadConsole, this game looks old-school");
        this.Print(2, 9, "(ASCII / tile) but runs smoothly and also");
        this.Print(2, 10, "supports input, UI, sound and more.");

        this.Print(2, 12, "To continue your playing experience please complete");
        this.Print(2, 13, "your name in below!");
        this.Print(2, 15, ColoredString.FromGradient(Color.Wheat, "Enter your nickname: "));

        boxName = new TextBox(20);
        boxName.Position = new Point(3 + 20, 15);
        boxName.MaxLength = 20;
        boxName.IsFocused = true;

        btnExitGame = new SelectionButton(18, 1);
        btnExitGame.Text = "Exit Game";
        btnExitGame.Position = new Point(8, 18);

        btnExitGame.Click += (s, e) =>
        {
            SadConsole.Game.Instance.MonoGameInstance.Exit();
            _audioSystem.PlaySoundEffect(Resources.getSoundEffect(Resources.click_sound), 1f);
        };
        btnExitGame.Unfocused += (s, e) =>
        {
            _audioSystem.PlaySoundEffect(Resources.getSoundEffect(Resources.switch_sound), 1f);
        };

        btnContinue = new SelectionButton(18, 1);
        btnContinue.Text = "Continue";
        btnContinue.Position = new Point(36, 18);

        btnContinue.Click += (s, e) =>
        {
            TryCreateAccountAndProceed();
            _audioSystem.PlaySoundEffect(Resources.getSoundEffect(Resources.click_sound), 1f);
        };
        btnContinue.Unfocused += (s, e) =>
        {
            _audioSystem.PlaySoundEffect(Resources.getSoundEffect(Resources.switch_sound), 1f);
        };

        var customColors = SadConsole.UI.Colors.Default.Clone();
        customColors.ControlBackgroundNormal.SetColor(Color.Black);
        customColors.ControlForegroundNormal.SetColor(Color.DarkGray);
        customColors.ControlBackgroundFocused.SetColor(Color.Black);
        customColors.ControlForegroundFocused.SetColor(Color.Wheat);
        customColors.RebuildAppearances();

        btnExitGame.SetThemeColors(customColors);
        btnContinue.SetThemeColors(customColors);

        btnContinue.NextSelection = btnExitGame;
        btnContinue.PreviousSelection = btnExitGame;

        btnExitGame.NextSelection = btnContinue;
        btnExitGame.PreviousSelection = btnContinue;


        Controls.Add(boxName);
        Controls.Add(btnExitGame);
        Controls.Add(btnContinue);
        Controls.FocusedControl = boxName;
        SadConsole.Game.Instance.Screen = this;
    }

    private async void TryCreateAccountAndProceed()
    {
        string nickname = boxName.Text?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(nickname))
        {
            return;
        }

        var playerData = new PlayerData
        {
            Name = nickname,
            Money = 1000,
            History = new List<MatchRecord>(),
            Characters = new List<string>(),
            Settings = new SettingsData
            {
                LobbyBgmVolume = 25,
                GameBgmVolume = 25,
                LobbySoundEffectVolume = 100,
                GameSoundEffectVolume = 100,
            }
        };

        playerData.Characters.Add("Garter Snake");

        PlayerDataManager playerDataManager = new PlayerDataManager();
        playerDataManager.Save(playerData);

        var splash = new SplashScreen(new ControlsConsole(80, 25));
        SadConsole.Game.Instance.Screen = splash;
        await splash.RunAndWaitThen(() =>
        {
            SadConsole.Game.Instance.Screen = new MainMenu(this, playerData);
            GlobalAudio.PlayBgm(Resources.getMusic(Resources.lobby_bgm), 0.25f);
        });
    }

    public override bool ProcessKeyboard(SadConsole.Input.Keyboard keyboard)
    {
        if (keyboard.IsKeyPressed(Keys.Left))
        {
            Controls.FocusedControl = btnExitGame;
            return true;
        }
        else if (keyboard.IsKeyPressed(Keys.Right))
        {
            Controls.FocusedControl = btnContinue;
            return true;
        }

        return base.ProcessKeyboard(keyboard);
    }

}