using SadConsole;
using SadConsole.Input;
using SadConsole.UI;
using SadConsole.UI.Controls;
using SadRogue.Primitives;
using snake_console.Data;
using snake_console.Objects.Snakes;
using snake_console.Utils;
using snake_console.Utils.Sounds;

namespace snake_console.Game;

public class SelectCharacter : ControlsConsole
{

    private PlayerData playerData;
    private SelectionButton btnConfirmAndPlay;
    private SelectionButton btnQuit;

    private RadioButton selectedCharacter;

    private List<RadioButton> characterButtons = new List<RadioButton>();
    private Dictionary<string, (int x, int y)> characters { get; set; }

    public SelectCharacter(ControlsConsole prevConsole, PlayerData data) : base(80, 25)
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
        characters = new Dictionary<string,(int x,int y)>() {
            { EnumExtensions.GetDescription(SnakeType.GarterSnake), (2, 6) },        // Garter Snake  :contentReference[oaicite:1]{index=1}
            { EnumExtensions.GetDescription(SnakeType.Imperator), (24, 6) },      // Boa Imperator  :contentReference[oaicite:3]{index=3}
            { EnumExtensions.GetDescription(SnakeType.Viper), (48, 6) },       // Gaboon Viper  :contentReference[oaicite:5]{index=5}
            { EnumExtensions.GetDescription(SnakeType.Ratsnake), (2, 10) },     // King Ratsnake  :contentReference[oaicite:7]{index=7}
            { EnumExtensions.GetDescription(SnakeType.KingCobra), (24, 10) },// King Cobra  :contentReference[oaicite:9]{index=9}
            { EnumExtensions.GetDescription(SnakeType.Belcher), (48, 10) },  // Belcher  :contentReference[oaicite:11]{index=11}
            { EnumExtensions.GetDescription(SnakeType.Rattlesnake), (2, 14) },        // Rattlesnake  :contentReference[oaicite:13]{index=13}
            { EnumExtensions.GetDescription(SnakeType.BlueKrait), (24, 14) },         // Blue Krait  :contentReference[oaicite:15]{index=15}
            { EnumExtensions.GetDescription(SnakeType.Python), (48, 14) }             // Python  :contentReference[oaicite:17]{index=17}
        };
    }

    private void init()
    {
        this.Print(2, 1, ColoredString.FromGradient(Color.Wheat, "> Select Character <"));
        this.Print(2, 3, "Please select a character in below!");

        btnQuit = new SelectionButton(18, 1);
        btnQuit.Text = "Quit";
        btnQuit.Position = new Point(8, 18);

        btnQuit.Click += (s, e) =>
        {
            Quit();
            LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.click_sound), playerData.Settings.LobbySoundEffectVolume / 100f);
        };
        btnQuit.Unfocused += (s, e) =>
        {
            LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.switch_sound), playerData.Settings.LobbySoundEffectVolume / 100f);
        };

        btnConfirmAndPlay = new SelectionButton(18, 1);
        btnConfirmAndPlay.Text = "Confirm & Play";
        btnConfirmAndPlay.Position = new Point(36, 18);
        btnConfirmAndPlay.IsEnabled = selectedCharacter != null;
        btnConfirmAndPlay.Click += (s, e) =>
        {
            LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.click_sound), playerData.Settings.LobbySoundEffectVolume / 100f);
            if (selectedCharacter != null)
            {
                var snake = EnumExtensions.FromDescription<SnakeType>(selectedCharacter.Text);
                if (snake.HasValue)
                {
                    EnterGame(snake.Value);
                }
            }
        };
        btnConfirmAndPlay.Unfocused += (s, e) =>
        {
            LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.switch_sound), playerData.Settings.LobbySoundEffectVolume / 100f);
        };

        foreach (var character in characters)
        {
            var characterButton = new RadioButton(character.Key);
            characterButton.Position = new Point(character.Value.x, character.Value.y);

            var ownedCharacterColors = Colors.Default.Clone();
            ownedCharacterColors.ControlBackgroundNormal.SetColor(Color.Black);
            ownedCharacterColors.ControlForegroundNormal.SetColor(Color.White);
            ownedCharacterColors.ControlBackgroundFocused.SetColor(Color.Black);
            ownedCharacterColors.ControlForegroundFocused.SetColor(Color.Wheat);
            ownedCharacterColors.RebuildAppearances();

            var notOwnedCharacterColors = Colors.Default.Clone();
            notOwnedCharacterColors.ControlBackgroundNormal.SetColor(Color.Black);
            notOwnedCharacterColors.ControlForegroundNormal.SetColor(Color.Red);
            notOwnedCharacterColors.ControlBackgroundFocused.SetColor(Color.Black);
            notOwnedCharacterColors.ControlForegroundFocused.SetColor(Color.Wheat);
            notOwnedCharacterColors.RebuildAppearances();

            bool isOwned = playerData.Characters != null && playerData.Characters.Contains(character.Key);
            if (isOwned)
            {
                characterButton.SetThemeColors(ownedCharacterColors);
            }
            else
            {
                characterButton.SetThemeColors(notOwnedCharacterColors);
                characterButton.IsEnabled = false;
            }

            characterButton.GroupName = "snakes";

            characterButton.Click += (s, e) =>
            {
                btnConfirmAndPlay.IsEnabled = true;
                selectedCharacter = characterButton;
                LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.switch_sound), playerData.Settings.LobbySoundEffectVolume / 100f);
            };
            characterButton.Unfocused += (s, e) =>
            {
                LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.switch_sound), 1f);
            };

            characterButtons.Add(characterButton);
            Controls.Add(characterButton);
        }

        var customColors = Colors.Default.Clone();
        customColors.ControlBackgroundNormal.SetColor(Color.Black);
        customColors.ControlForegroundNormal.SetColor(Color.DarkGray);
        customColors.ControlBackgroundFocused.SetColor(Color.Black);
        customColors.ControlForegroundFocused.SetColor(Color.Wheat);
        customColors.RebuildAppearances();

        btnQuit.SetThemeColors(customColors);
        btnConfirmAndPlay.SetThemeColors(customColors);

        Controls.Add(btnQuit);
        Controls.Add(btnConfirmAndPlay);

        btnConfirmAndPlay.NextSelection = btnQuit;
        btnConfirmAndPlay.PreviousSelection = btnQuit;

        btnQuit.NextSelection = btnConfirmAndPlay;
        btnQuit.PreviousSelection = btnConfirmAndPlay;

        Controls.Add(btnQuit);
        Controls.Add(btnConfirmAndPlay);

        Controls.FocusedControl = btnConfirmAndPlay;
        IsFocused = true;
    }

    private async void EnterGame(SnakeType snakeType)
    {
        var splash = new SplashScreen(this);
        SadConsole.Game.Instance.Screen = splash;
        await splash.RunAndWaitThen(() =>
        {
            SadConsole.Game.Instance.Screen = new Game(this, playerData, snakeType);
            GlobalAudio.StopAll();
            GlobalAudio.PlayBgm(Resources.getMusic(Resources.game_bgm), playerData.Settings.GameBgmVolume / 100f);
        });
    }

    private async void Quit()
    {
        var splash = new SplashScreen(this);
        SadConsole.Game.Instance.Screen = splash;
        await splash.RunAndWaitThen(() =>
        {
            SadConsole.Game.Instance.Screen = new MainMenu(this, playerData);
        });
    }

    public override bool ProcessKeyboard(SadConsole.Input.Keyboard keyboard)
    {
        if (keyboard.IsKeyPressed(Keys.Left))
        {
            Controls.FocusedControl = btnQuit;
            return true;
        }
        else if (keyboard.IsKeyPressed(Keys.Right))
        {
            Controls.FocusedControl = btnConfirmAndPlay;
            return true;
        }

        return base.ProcessKeyboard(keyboard);
    }


}