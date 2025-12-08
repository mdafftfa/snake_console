using SadRogue.Primitives;
using snake_console.Objects.Snakes;

namespace snake_console.Objects.Obstacles;

public class Wall : ObstacleEntity
{

    protected override string Name => "Wall";
    protected override Color Color => Color.WhiteSmoke;
    protected override char Symbol => '0';
    protected override int Damage => 5;
    protected override bool Destructible => false;

    protected override bool CanMove => false;
    protected override int MoveInterval => 0;
    protected override int MoveSpeed => 0;

}