using SadRogue.Primitives;

namespace snake_console.Objects.Snakes;

public class SnakeEntity
{
    public enum Direction { Left, Right, Up, Down }

    private Direction currentDirection = Direction.Right;
    protected SnakeType snakeType;

    protected virtual string Name => "SnakeEntity";
    protected virtual Color BodyColor => Color.Wheat;
    protected virtual Color HeadColor => Color.White;

    private int hp;
    private bool isAlive;

    protected virtual int MaxHp => 100;
    protected virtual int Price => 8000;

    public SnakeEntity()
    {
        hp = getMaxHp();
        isAlive = true;
    }

    public static SnakeEntity Create(SnakeType snakeType)
    {
        return snakeType switch
        {
            SnakeType.GarterSnake => new GarterSnake(),
            SnakeType.Imperator => new Imperator(),
            SnakeType.Viper => new Viper(),
            SnakeType.Ratsnake => new Ratsnake(),
            SnakeType.KingCobra => new KingCobra(),
            SnakeType.Belcher => new Belcher(),
            SnakeType.Rattlesnake => new Rattlesnake(),
            SnakeType.BlueKrait => new BlueKrait(),
            SnakeType.Python => new Python(),
            _ => new GarterSnake()
        };
    }

    public string getName() => Name;
    public Color getBodyColor() => BodyColor;
    public Color getHeadColor() => HeadColor;
    public int getHp() => hp;
    public int getMaxHp() => MaxHp;
    public int getPrice() => Price;

    public void attack(int baseDamage)
    {
        if (hp - baseDamage <= 0)
        {
            isAlive = false;
        }

        hp = Math.Max(0, hp - baseDamage);
    }

    public void heal(int healAmount)
    {
        hp = Math.Max(0, hp + healAmount);
    }

    public bool IsAlive() => isAlive;

    public Direction getCurrentDirection() => currentDirection;
    public void setCurrentDirection(Direction newDirection) => currentDirection = newDirection;
}