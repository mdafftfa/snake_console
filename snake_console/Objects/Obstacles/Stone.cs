using SadRogue.Primitives;
using snake_console.Objects.Snakes;

namespace snake_console.Objects.Obstacles;

public class Stone : ObstacleEntity
{
    // Override properties untuk Stone
    protected override string Name => "Stone";
    protected override Color Color => Color.DarkGray;
    protected override char Symbol => 'O';
    protected override int Damage => 15;
    protected override bool Destructible => false;

    public override void OnCollision(SnakeEntity snake)
    {
        snake.attack(5);
    }
}