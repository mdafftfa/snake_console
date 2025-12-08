using SadRogue.Primitives;
using snake_console.Objects.Snakes;

namespace snake_console.Objects;

public interface IObstacle
{
    string getName();
    Color getColor();
    char getSymbol();
    int getDamage();
    bool isDestructible();
    void onCollision(SnakeEntity snake);
}