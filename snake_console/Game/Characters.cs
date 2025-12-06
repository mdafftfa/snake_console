using SadConsole;
using SadConsole.Input;
using SadConsole.Readers;
using SadConsole.UI;
using SadConsole.UI.Controls;
using SadRogue.Primitives;
using snake_console.Data;
using snake_console.Objects.Snakes;
using snake_console.Utils;
using snake_console.Utils.Sounds;

namespace snake_console.Game;

public class Characters : ControlsConsole
{

    private PlayerData playerData;
    private List<RadioButton> characterButtons = new List<RadioButton>();
    private Dictionary<string, (int x, int y, int price)> characters { get; set; }
    private int price;

    private RadioButton selectedCharacter;

    private SelectionButton btnBack;
    private SelectionButton btnBuy;

    public Characters(ControlsConsole prevConsole, PlayerData data) : base(80, 25)
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
        characters = new Dictionary<string,(int x,int y, int price)>() {
            { EnumExtensions.GetDescription(SnakeType.GarterSnake), (2, 7, 8000) },        // Garter Snake  :contentReference[oaicite:1]{index=1}
            { EnumExtensions.GetDescription(SnakeType.Imperator), (24, 7, 8000) },      // Boa Imperator  :contentReference[oaicite:3]{index=3}
            { EnumExtensions.GetDescription(SnakeType.Viper), (48, 7, 8000) },       // Gaboon Viper  :contentReference[oaicite:5]{index=5}
            { EnumExtensions.GetDescription(SnakeType.Ratsnake), (2, 11, 8000) },     // King Ratsnake  :contentReference[oaicite:7]{index=7}
            { EnumExtensions.GetDescription(SnakeType.KingCobra), (24, 11, 8000) },// King Cobra  :contentReference[oaicite:9]{index=9}
            { EnumExtensions.GetDescription(SnakeType.Belcher), (48, 11, 8000) },  // Belcher  :contentReference[oaicite:11]{index=11}
            { EnumExtensions.GetDescription(SnakeType.Rattlesnake), (2, 15, 8000) },        // Rattlesnake  :contentReference[oaicite:13]{index=13}
            { EnumExtensions.GetDescription(SnakeType.BlueKrait), (24, 15, 8000) },         // Blue Krait  :contentReference[oaicite:15]{index=15}
            { EnumExtensions.GetDescription(SnakeType.Python), (48, 15, 8000) }             // Python  :contentReference[oaicite:17]{index=17}
        };

    }

    private void init()
    {
        this.Print(2, 1, ColoredString.FromGradient(Color.Wheat, "> Characters <"));
        this.Print(2, 3, "Select a character to buy!");
        this.Print(2, 4, "Your money: ");
        this.Print(14, 4, ColoredString.FromGradient(Color.Wheat, playerData.Money.ToString("N0")));
        this.Print(2, 5, "Price: ");
        this.Print(9, 5, ColoredString.FromGradient(Color.Wheat, price.ToString("N0")));

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

        btnBuy = new SelectionButton(18, 1);
        btnBuy.Text = "Buy";
        btnBuy.Position = new Point(36, 18);

        btnBuy.Click += (s, e) =>
        {
            LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.click_sound), playerData.Settings.LobbySoundEffectVolume / 100f);
            if (selectedCharacter != null)
            {
                var snake = EnumExtensions.FromDescription<SnakeType>(selectedCharacter.Text);
                if (snake.HasValue)
                {
                    BuyCharacter(snake.Value);
                }
            }
        };
        btnBuy.Unfocused += (s, e) =>
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
                characterButton.SetThemeColors(notOwnedCharacterColors);
                characterButton.IsEnabled = false;
            }
            else
            {
                characterButton.SetThemeColors(ownedCharacterColors);
            }

            characterButton.GroupName = "snakes";

            characterButton.Click += (s, e) =>
            {
                price = character.Value.price;
                selectedCharacter = characterButton;
                DisplayPriceInfo();
                LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.switch_sound), playerData.Settings.LobbySoundEffectVolume / 100f);
            };
            characterButton.Unfocused += (s, e) =>
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
            btnBuy.SetThemeColors(customColors);

            btnBuy.NextSelection = btnBack;
            btnBuy.PreviousSelection = btnBack;

            btnBack.NextSelection = btnBuy;
            btnBack.PreviousSelection = btnBuy;

            characterButtons.Add(characterButton);

            Controls.Add(characterButton);
        }

        Controls.Add(btnBack);
        Controls.Add(btnBuy);

        Controls.FocusedControl = btnBuy;
        IsFocused = true;

    }

    private void BuyCharacter(SnakeType snakeType)
    {
        SnakeEntity entity = SnakeEntity.Create(snakeType);
        if (playerData.Money >= entity.getPrice())
        {
            playerData.Money -= entity.getPrice();
            playerData.Characters.Add(EnumExtensions.GetDescription(snakeType));

            selectedCharacter.IsEnabled = false;
            price = 0;
            DisplayPriceInfo();
            DisplayBoughtInfo(snakeType);

            PlayerDataManager playerDataManager = new PlayerDataManager();
            playerDataManager.Save(playerData);
        }
    }

    private void DisplayBoughtInfo(SnakeType snakeType)
    {
        this.DrawLine(new Point(36, 1), new Point(79, 2), ' ');
        this.Print(40, 1, ColoredString.FromGradient(Color.Wheat, "Successfully bought"));
        this.Print(40, 2, ColoredString.FromGradient(Color.Wheat, EnumExtensions.GetDescription(snakeType) +"!"));
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
            Controls.FocusedControl = btnBuy;
            return true;
        }

        return base.ProcessKeyboard(keyboard);
    }

    private void DisplayPriceInfo()
    {
        this.DrawLine(new Point(2, 4), new Point(79, 4), ' ');
        this.DrawLine(new Point(2, 5), new Point(79, 5), ' ');

        this.Print(2, 4, "Your money: ");
        this.Print(14, 4, ColoredString.FromGradient(Color.Wheat, playerData.Money.ToString("N0")));
        this.Print(2, 5, "Price: ");
        this.Print(9, 5, ColoredString.FromGradient(Color.Wheat, price.ToString("N0")));

        if (btnBuy != null) btnBuy.IsEnabled = playerData.Money >= price;
    }

}