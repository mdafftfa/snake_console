using SadRogue.Primitives;

namespace snake_console.Objects.Snakes;

public class Python : SnakeEntity, ISnake
{
    protected override string Name => "Python";
    protected override Color BodyColor => Color.AliceBlue;
    protected override Color HeadColor => Color.AnsiYellow;
    protected override int MaxHp => 100;
    protected override int Price => 8000;
}