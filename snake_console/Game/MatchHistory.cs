using SadConsole;
using SadConsole.Configuration;
using SadConsole.UI;
using SadConsole.UI.Controls;
using SadRogue.Primitives;
using snake_console.Data;
using snake_console.Utils;
using snake_console.Utils.Sounds;

namespace snake_console.Game;

public class MatchHistory : ControlsConsole
{

    private PlayerData playerData;

    private SelectionButton btnBack;


    private List<MatchRecord> records;

    public MatchHistory(ControlsConsole prevConsole, PlayerData data) : base(80, 25)
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
        records = playerData.History;
    }

    private void init()
    {
        this.Print(2, 1, ColoredString.FromGradient(Color.Wheat, "> Match History <"));
        this.Print(2, 3, "View detailed records of your recent games!");

        this.Print(2, 5, ColoredString.FromGradient(Color.White, "Character"));
        this.Print(16, 5, ColoredString.FromGradient(Color.White, "Kills"));
        this.Print(24, 5, ColoredString.FromGradient(Color.White, "Time Ends"));
        this.Print(36, 5, ColoredString.FromGradient(Color.White, "Score"));
        this.Print(44, 5, ColoredString.FromGradient(Color.White, "Date & Time"));

        // this.Print(2, 7, ColoredString.FromGradient(Color.Wheat, "Rattlesnake"));
        // this.Print(16, 7, ColoredString.FromGradient(Color.Wheat, "13"));
        // this.Print(24, 7, ColoredString.FromGradient(Color.Wheat, "05:00"));
        // this.Print(36, 7, ColoredString.FromGradient(Color.Wheat, "312"));
        // this.Print(44, 7, ColoredString.FromGradient(Color.Wheat, "02/02/2005 (08:00)"));

        DisplayRecords();
        if (records.Count == 0)
        {
            this.Print(22, 11, "No records found");
        }

        btnBack = new SelectionButton(18, 1);
        btnBack.Text = "Back";
        btnBack.Position = new Point(22, 18);

        btnBack.Click += (s, e) =>
        {
            SadConsole.Game.Instance.Screen = new MainMenu(this, playerData);
            LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.click_sound), playerData.Settings.LobbySoundEffectVolume / 100f);
        };
        btnBack.Unfocused += (s, e) =>
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

        Controls.Add(btnBack);
        Controls.FocusedControl = btnBack;
        IsFocused = true;

    }

    private void DisplayRecords()
    {
        var sortedRecords = records
            .OrderByDescending(r => r.DateTime)
            .Take(10)
            .ToList();

        int startY = 7;

        foreach (var record in sortedRecords)
        {
            this.Print(2, startY, ColoredString.FromGradient(Color.Wheat, record.Character));
            this.Print(16, startY, ColoredString.FromGradient(Color.Wheat, record.Kills.ToString()));
            this.Print(24, startY, ColoredString.FromGradient(Color.Wheat, record.TimeEnds));
            this.Print(36, startY, ColoredString.FromGradient(Color.Wheat, record.Score.ToString("N0")));

            this.Print(44, startY, ColoredString.FromGradient(Color.Wheat,
                string.Format("{0:dd/MM/yyyy (HH:mm)}", record.DateTime)));

            if (startY < 16)
            {
                startY++;
            }
            else
            {
                break;
            }
        }
    }

}