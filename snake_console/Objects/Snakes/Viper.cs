using SadRogue.Primitives;

namespace snake_console.Objects.Snakes;

public class Viper : SnakeEntity, ISnake
{

    protected override string Name => "Viper";
    protected override Color BodyColor => Color.Green;
    protected override Color HeadColor => Color.DarkGreen;
    protected override int MaxHp => 100;
    protected override int Price => 8000;

}