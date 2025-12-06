using SadRogue.Primitives;

namespace snake_console.Objects.Snakes;

public class Imperator : SnakeEntity, ISnake
{
    protected override string Name => "Imperator";
    protected override Color BodyColor => Color.Gray;
    protected override Color HeadColor => Color.Wheat;
    protected override int MaxHp => 100;
    protected override int Price => 8000;
}