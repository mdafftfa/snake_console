using snake_console.Data;
using snake_console.Objects.Snakes;

namespace snake_console.Objects;

public class Player
{

    private PlayerData playerData;
    private SnakeEntity snakeEntity;

    public Player(PlayerData data, SnakeEntity snake)
    {
        playerData = data;
        snakeEntity = snake;
    }

    public PlayerData getPlayerData()
    {
        return playerData;
    }

    public SnakeEntity getSnakeEntity()
    {
        return snakeEntity;
    }


}