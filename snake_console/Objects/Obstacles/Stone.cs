using SadRogue.Primitives;
using snake_console.Objects.Obstacles.Enums;
using snake_console.Objects.Snakes;

namespace snake_console.Objects.Obstacles;

public class Stone : ObstacleEntity
{

    protected override string Name => "Stone";
    protected override Color Color => Color.WhiteSmoke;
    protected override char Symbol => '?';
    protected override int Damage => 15;
    protected override bool Destructible => false;

    protected override bool CanMove => true;
    protected override int MoveInterval => 3000;
    protected override int MoveSpeed => 1;
    protected override MovementPattern MovementPattern => MovementPattern.Random;

}