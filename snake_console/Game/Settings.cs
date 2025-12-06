using SadConsole;
using SadConsole.Input;
using SadConsole.UI;
using SadConsole.UI.Controls;
using SadRogue.Primitives;
using snake_console.Data;
using snake_console.Utils;
using snake_console.Utils.Sounds;

namespace snake_console.Game;

public class Settings : ControlsConsole
{

    private AudioSystem _audioSystem;
    private PlayerData playerData;

    private SelectionButton btnBack;
    private SelectionButton btnSaveAndExit;

    private TextBox textBoxLobbyBgmVolume;
    private TextBox textBoxGameBgmVolume;
    private TextBox textBoxLobbySoundEffectVolume;
    private TextBox textBoxGameSoundEffectVolume;

    public Settings(ControlsConsole prevConsole, PlayerData data) : base(80, 25)
    {
        FontSize = new Point(10, 20);
        playerData = data;

        loadResources();
        init();
    }

    private void loadResources()
    {
        _audioSystem = new AudioSystem();
    }

    private void init()
    {
        this.Print(2, 1, ColoredString.FromGradient(Color.Wheat, "> Settings <"));
        this.Print(2, 3, "Configure game preferences to match your style!");


        var textBoxColors = Colors.Default.Clone();
        textBoxColors.ControlBackgroundNormal.SetColor(Color.Black);
        textBoxColors.ControlForegroundNormal.SetColor(Color.Wheat);
        textBoxColors.ControlBackgroundFocused.SetColor(Color.Black);
        textBoxColors.ControlForegroundFocused.SetColor(Color.Wheat);
        textBoxColors.RebuildAppearances();

        this.Print(2, 6, ColoredString.FromGradient(Color.White, "Lobby Music Volume"));
        textBoxLobbyBgmVolume = new TextBox(10);
        textBoxLobbyBgmVolume.Text = playerData.Settings.LobbyBgmVolume.ToString();
        textBoxLobbyBgmVolume.Position = new Point(42, 6);
        textBoxLobbyBgmVolume.SetThemeColors(textBoxColors);

        this.Print(2, 8, ColoredString.FromGradient(Color.White, "Game Music Volume"));
        textBoxGameBgmVolume = new TextBox(10);
        textBoxGameBgmVolume.Text = playerData.Settings.GameBgmVolume.ToString();
        textBoxGameBgmVolume.Position = new Point(42, 8);
        textBoxGameBgmVolume.SetThemeColors(textBoxColors);

        this.Print(2, 10, ColoredString.FromGradient(Color.White, "Sound Effect Lobby Volume"));
        textBoxLobbySoundEffectVolume = new TextBox(10);
        textBoxLobbySoundEffectVolume.Text = playerData.Settings.LobbySoundEffectVolume.ToString();
        textBoxLobbySoundEffectVolume.Position = new Point(42, 10);
        textBoxLobbySoundEffectVolume.SetThemeColors(textBoxColors);

        this.Print(2, 12, ColoredString.FromGradient(Color.White, "Sound Effect Game Volume"));
        textBoxGameSoundEffectVolume = new TextBox(10);
        textBoxGameSoundEffectVolume.Text = playerData.Settings.GameSoundEffectVolume.ToString();
        textBoxGameSoundEffectVolume.Position = new Point(42, 12);
        textBoxGameSoundEffectVolume.SetThemeColors(textBoxColors);

        btnBack = new SelectionButton(18, 1);
        btnBack.Text = "Back";
        btnBack.Position = new Point(8, 18);

        btnBack.Click += (s, e) =>
        {
            SadConsole.Game.Instance.Screen = new MainMenu(this, playerData);
            LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.click_sound), playerData.Settings.LobbySoundEffectVolume / 100f);
        };
        btnBack.Unfocused += (s, e) =>
        {
            LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.switch_sound), playerData.Settings.LobbySoundEffectVolume / 100f);
        };
        
        btnSaveAndExit = new SelectionButton(18, 1);
        btnSaveAndExit.Text = "Save & Exit";
        btnSaveAndExit.Position = new Point(36, 18);

        btnSaveAndExit.Click += (s, e) =>
        {
            LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.click_sound), playerData.Settings.LobbySoundEffectVolume / 100f);

            if (int.TryParse(textBoxLobbyBgmVolume.Text, out int lobbyBgmVol) &&
                int.TryParse(textBoxGameBgmVolume.Text, out int gameBgmVol) &&
                int.TryParse(textBoxLobbySoundEffectVolume.Text, out int lobbySfxVol) &&
                int.TryParse(textBoxGameSoundEffectVolume.Text, out int gameSfxVol))
            {
                if (int.Parse(textBoxLobbyBgmVolume.Text) != playerData.Settings.LobbyBgmVolume)
                {
                    GlobalAudio.StopAll();
                    float normalizedBgmVolume = lobbyBgmVol / 100f;
                    GlobalAudio.PlayBgmIfNotPlaying(Resources.getMusic(Resources.lobby_bgm), normalizedBgmVolume);
                }

                lobbyBgmVol = Math.Max(0, Math.Min(lobbyBgmVol, 100));
                gameBgmVol = Math.Max(0, Math.Min(gameBgmVol, 100));
                lobbySfxVol = Math.Max(0, Math.Min(lobbySfxVol, 100));
                gameSfxVol = Math.Max(0, Math.Min(gameSfxVol, 100));

                playerData.Settings.LobbyBgmVolume = lobbyBgmVol;
                playerData.Settings.GameBgmVolume = gameBgmVol;
                playerData.Settings.LobbySoundEffectVolume = lobbySfxVol;
                playerData.Settings.GameSoundEffectVolume = gameSfxVol;

                PlayerDataManager playerDataManager = new PlayerDataManager();
                playerDataManager.Save(playerData);

                SadConsole.Game.Instance.Screen = new MainMenu(this, playerData);
            }
        };
        btnSaveAndExit.Unfocused += (s, e) =>
        {
            LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.switch_sound), playerData.Settings.LobbySoundEffectVolume / 100f);
        };
        
        var customColors = Colors.Default.Clone();
        customColors.ControlBackgroundNormal.SetColor(Color.Black);
        customColors.ControlForegroundNormal.SetColor(Color.DarkGray);
        customColors.ControlBackgroundFocused.SetColor(Color.Black);
        customColors.ControlForegroundFocused.SetColor(Color.Wheat);
        customColors.RebuildAppearances();

        btnBack.SetThemeColors(customColors);
        btnSaveAndExit.SetThemeColors(customColors);

        Controls.Add(textBoxLobbyBgmVolume);
        Controls.Add(textBoxGameBgmVolume);
        Controls.Add(textBoxLobbySoundEffectVolume);
        Controls.Add(textBoxGameSoundEffectVolume);

        Controls.Add(btnBack);
        Controls.Add(btnSaveAndExit);

        Controls.FocusedControl = btnSaveAndExit;
        IsFocused = true;

    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        if (keyboard.IsKeyPressed(Keys.Left))
        {
            Controls.FocusedControl = btnBack;
            return true;
        }
        else if (keyboard.IsKeyPressed(Keys.Right))
        {
            Controls.FocusedControl = btnSaveAndExit;
            return true;
        }

        return base.ProcessKeyboard(keyboard);
    }

}