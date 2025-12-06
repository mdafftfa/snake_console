using System.ComponentModel;

namespace snake_console.Objects.Obstacles.Enums;

public enum ObstacleType
{
    [Description("Stone")] Stone,
    [Description("Spike")] Spike,
    [Description("Bush")] Bush,
    [Description("Wall")] Wall,
}