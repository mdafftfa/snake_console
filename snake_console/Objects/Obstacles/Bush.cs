using SadRogue.Primitives;
using snake_console.Objects.Obstacles.Enums;

namespace snake_console.Objects.Obstacles;

public class Bush : ObstacleEntity
{

    protected override string Name => "Bush";
    protected override Color Color => Color.WhiteSmoke;
    protected override char Symbol => '>';
    protected override int Damage => 5;
    protected override bool Destructible => true;

    protected override bool CanMove => true;
    protected override int MoveInterval => 2000;
    protected override int MoveSpeed => 1;
    protected override MovementPattern MovementPattern => MovementPattern.Vertical;

}