using SadRogue.Primitives;

namespace snake_console.Objects.Snakes;

public class Rattlesnake : SnakeEntity, ISnake
{
    protected override string Name => "Rattlesnake";
    protected override Color BodyColor => Color.BlueViolet;
    protected override Color HeadColor => Color.DarkGray;
    protected override int MaxHp => 100;
    protected override int Price => 8000;
}