using SadRogue.Primitives;
using snake_console.Objects.Snakes;

namespace snake_console.Objects.Obstacles;

public class Spike : ObstacleEntity
{

    protected override string Name => "Spike";
    protected override Color Color => Color.Red;
    protected override char Symbol => '▲';
    protected override int Damage => 25;
    protected override bool Destructible => true;

    public override void OnCollision(SnakeEntity snake)
    {
        snake.attack(10);
    }

}