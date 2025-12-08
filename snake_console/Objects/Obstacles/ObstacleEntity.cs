using SadRogue.Primitives;
using snake_console.Objects.Obstacles.Enums;
using snake_console.Objects.Snakes;

namespace snake_console.Objects.Obstacles;

public abstract class ObstacleEntity : IObstacle
{

    public Point Position { get; set; }
    public Point Direction { get; set; }

    protected virtual string Name => "Obstacle";
    protected virtual Color Color => Color.Gray;
    protected virtual char Symbol => '#';
    protected virtual int Damage => 10;
    protected virtual bool Destructible => false;

    protected virtual int MoveSpeed => 1;
    protected virtual int MoveInterval => 2000;
    protected virtual bool CanMove => true;
    protected virtual MovementPattern MovementPattern => MovementPattern.Random;

    private DateTime lastMoveTime = DateTime.Now;

    public string getName() => Name;
    public Color getColor() => Color;
    public char getSymbol() => Symbol;
    public int getDamage() => Damage;
    public bool isDestructible() => Destructible;
    public bool getCanMove() => CanMove;

    public MovementPattern getMovementPattern() => MovementPattern;

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

    public void Update(TimeSpan deltaTime, Point? targetPosition = null)
    {
        if (!CanMove) return;

        if ((DateTime.Now - lastMoveTime).TotalMilliseconds >= MoveInterval)
        {
            Move(targetPosition);
            lastMoveTime = DateTime.Now;
        }
    }

    protected virtual void Move(Point? targetPosition = null)
    {
        switch (MovementPattern)
        {
            case MovementPattern.Random:
                MoveRandom();
                break;
            case MovementPattern.Horizontal:
                if (Direction == Point.None)
                {
                    Direction = new Point(MoveSpeed, 0);
                }
                Position += Direction;
                break;
            case MovementPattern.Vertical:
                if (Direction == Point.None)
                {
                    Direction = new Point(0, MoveSpeed);
                }
                Position += Direction;
                break;
            case MovementPattern.TowardsPlayer:
                if (targetPosition.HasValue)
                {
                    Point snakeHead = targetPosition.Value;

                    int dx = snakeHead.X - Position.X;
                    int dy = snakeHead.Y - Position.Y;

                    // Normalize direction
                    if (Math.Abs(dx) > Math.Abs(dy))
                    {
                        Direction = new Point(Math.Sign(dx) * MoveSpeed, 0);
                    }
                    else
                    {
                        Direction = new Point(0, Math.Sign(dy) * MoveSpeed);
                    }

                    Position += Direction;
                }
                else
                {
                    // Fallback to random movement
                    MoveRandom();
                }
                break;
        }
    }

    public bool IsOutOfBounds(int minX, int maxX, int minY, int maxY)
    {
        return Position.X < minX || Position.X > maxX || Position.Y < minY || Position.Y > maxY;
    }

    public void BounceFromBounds(int minX, int maxX, int minY, int maxY)
    {
        if (Position.X <= minX || Position.X >= maxX)
        {
            Direction = new Point(-Direction.X, Direction.Y);
        }

        if (Position.Y <= minY || Position.Y >= maxY)
        {
            Direction = new Point(Direction.X, -Direction.Y);
        }

        Position = new Point(
            Math.Clamp(Position.X, minX + 1, maxX - 1),
            Math.Clamp(Position.Y, minY + 1, maxY - 1)
        );
    }

    public bool CollideWith(Point point)
    {
        return Position == point;
    }

    public bool CollidesWithArea(Point position, int radius = 0)
    {
        if (radius == 0)
        {
            return Position == position;
        }

        double distance = Math.Sqrt(
            Math.Pow(Position.X - position.X, 2) +
            Math.Pow(Position.Y - position.Y, 2)
        );

        return distance <= radius;
    }

    protected void MoveRandom()
    {
        Random random = new Random();
        int direction = random.Next(0, 4);

        switch (direction)
        {
            case 0: // Up
                Direction = new Point(0, -MoveSpeed);
                break;
            case 1: // Down
                Direction = new Point(0, MoveSpeed);
                break;
            case 2: // Left
                Direction = new Point(-MoveSpeed, 0);
                break;
            case 3: // Right
                Direction = new Point(MoveSpeed, 0);
                break;
        }

        Position += Direction;
    }



    public virtual void onCollision(SnakeEntity snake)
    {
        snake.attack(Damage);
    }
}