using SadRogue.Primitives;
using snake_console.Objects.Snakes;

namespace snake_console.Objects;

public interface IObstacle
{
    string GetName();
    Color GetColor();
    char GetSymbol();
    int GetDamage();
    bool IsDestructible();
    void OnCollision(SnakeEntity snake);
}