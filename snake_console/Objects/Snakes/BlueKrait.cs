using SadRogue.Primitives;

namespace snake_console.Objects.Snakes;

public class BlueKrait : SnakeEntity, ISnake
{
    protected override string Name => "Blue Krait";
    protected override Color BodyColor => Color.Gray;
    protected override Color HeadColor => Color.White;
    protected override int MaxHp => 100;
    protected override int Price => 8000;

}