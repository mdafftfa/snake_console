using SadRogue.Primitives;

namespace snake_console.Objects.Snakes;

public class KingCobra : SnakeEntity, ISnake
{
    protected override string Name => "King Cobra";
    protected override Color BodyColor => Color.AliceBlue;
    protected override Color HeadColor => Color.BlueViolet;
    protected override int MaxHp => 100;
    protected override int Price => 8000;
}