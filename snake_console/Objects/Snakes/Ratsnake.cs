using SadRogue.Primitives;

namespace snake_console.Objects.Snakes;

public class Ratsnake : SnakeEntity, ISnake
{
    protected override string Name => "Ratsnake";
    protected override Color BodyColor => Color.Gray;
    protected override Color HeadColor => Color.WhiteSmoke;
    protected override int MaxHp => 100;
    protected override int Price => 8000;
}