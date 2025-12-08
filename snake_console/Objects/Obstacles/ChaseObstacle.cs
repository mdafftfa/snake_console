using SadRogue.Primitives;
using snake_console.Objects.Obstacles.Enums;
using snake_console.Objects.Snakes;

namespace snake_console.Objects.Obstacles;

public class ChaseObstacle : ObstacleEntity
{
    protected override string Name => "Chaser";
    protected override Color Color => Color.WhiteSmoke;
    protected override char Symbol => 'C';
    protected override int Damage => 15;
    protected override bool Destructible => true;
    protected override int MoveSpeed => 2;
    protected override int MoveInterval => 1000;
    protected override bool CanMove => true;
    protected override MovementPattern MovementPattern => MovementPattern.TowardsPlayer;

    private SnakeEntity targetSnake;

}