using SadRogue.Primitives;
using snake_console.Objects.Obstacles.Enums;
using snake_console.Objects.Snakes;

namespace snake_console.Objects.Obstacles;

public class Spike : ObstacleEntity
{

    protected override string Name => "Spike";
    protected override Color Color => Color.WhiteSmoke;
    protected override char Symbol => '<';
    protected override int Damage => 25;
    protected override bool Destructible => true;
    protected override bool CanMove => true;
    protected override int MoveInterval => 1000;
    protected override int MoveSpeed => 2;
    protected override MovementPattern MovementPattern => MovementPattern.Horizontal;

}