using SadRogue.Primitives;
using snake_console.Objects.Obstacles.Enums;
using snake_console.Objects.Snakes;

namespace snake_console.Objects.Obstacles;

public abstract class ObstacleEntity : IObstacle
{

    protected virtual string Name => "Obstacle";
    protected virtual Color Color => Color.Gray;
    protected virtual char Symbol => '#';
    protected virtual int Damage => 10;
    protected virtual bool Destructible => false;

    public string GetName() => Name;
    public Color GetColor() => Color;
    public char GetSymbol() => Symbol;
    public int GetDamage() => Damage;
    public bool IsDestructible() => Destructible;

    public static ObstacleEntity Create(ObstacleType type)
    {
        return type switch
        {
            ObstacleType.Stone => new Stone(),
            ObstacleType.Spike => new Spike(),
            ObstacleType.Bush => new Bush(),
            ObstacleType.Wall => new Wall(),
            _ => new Stone()
        };
    }

    public virtual void OnCollision(SnakeEntity snake)
    {
        // snake.attack(Damage);
    }
}