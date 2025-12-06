using SadRogue.Primitives;

namespace snake_console.Objects.Snakes;

public class GarterSnake : SnakeEntity, ISnake
{

    protected override string Name => "Garter Snake";
    protected override Color BodyColor => Color.Gray;
    protected override Color HeadColor => Color.LightGray;
    protected override int MaxHp => 100;
    protected override int Price => 8000;
}