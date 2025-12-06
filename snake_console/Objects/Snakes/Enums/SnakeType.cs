using System.ComponentModel;

namespace snake_console.Objects.Snakes;

public enum SnakeType
{

    [Description("Garter Snake")] GarterSnake,
    [Description("Imperator")] Imperator,
    [Description("Viper")] Viper,
    [Description("Ratsnake")] Ratsnake,
    [Description("King Cobra")] KingCobra,
    [Description("Belcher")] Belcher,
    [Description("Rattlesnake")] Rattlesnake,
    [Description("Blue Krait")] BlueKrait,
    [Description("Python")] Python
}