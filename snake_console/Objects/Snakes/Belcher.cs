using SadRogue.Primitives;

namespace snake_console.Objects.Snakes;

public class Belcher : SnakeEntity, ISnake
{
    protected override string Name => "Belcher";
    protected override Color BodyColor => Color.Cyan;
    protected override Color HeadColor => Color.DarkCyan;
    protected override int MaxHp => 100;
    protected override int Price => 8000;

}