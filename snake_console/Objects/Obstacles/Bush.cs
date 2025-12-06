using SadRogue.Primitives;
using snake_console.Objects.Snakes;

namespace snake_console.Objects.Obstacles;

public class Bush : ObstacleEntity
{

    protected override string Name => "Bush";
    protected override Color Color => Color.Green;
    protected override char Symbol => '♣';
    protected override int Damage => 5;
    protected override bool Destructible => true;

    public override void OnCollision(SnakeEntity snake)
    {
        snake.attack(8);
    }

}