using SadRogue.Primitives;
using snake_console.Objects.Snakes;

namespace snake_console.Objects.Obstacles;

public class Wall : ObstacleEntity
{

    protected override string Name => "Wall";
    protected override Color Color => Color.Brown;
    protected override char Symbol => '▓';
    protected override int Damage => 0; // Tidak damage, hanya block
    protected override bool Destructible => false;

    public override void OnCollision(SnakeEntity snake)
    {
        snake.attack(12);
    }

}