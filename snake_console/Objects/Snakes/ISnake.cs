using SadRogue.Primitives;

namespace snake_console.Objects;

public interface ISnake
{

    public int getPrice();
    public int getMaxHp();
    public Color getHeadColor();
    public Color getBodyColor();
    public string getName();

}