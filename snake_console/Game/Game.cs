using SadConsole;
using SadConsole.Input;
using SadConsole.UI;
using SadConsole.UI.Controls;
using SadRogue.Primitives;
using snake_console.Data;
using snake_console.Objects;
using snake_console.Objects.Obstacles;
using snake_console.Objects.Obstacles.Enums;
using snake_console.Objects.Snakes;
using snake_console.Utils;
using snake_console.Utils.Sounds;

namespace snake_console.Game;

public class Game : ControlsConsole
{

    private Player player;

    private int kills;
    private int seconds = 600;

    private List<Point> snakeBody;
    private Point snakeDirection;

    private Point foodPosition;

    private Dictionary<ObstacleEntity, Point> obstacles;

    private const int InitialSnakeLength = 5;

    private int gameSpeed = 100;
    private DateTime lastSecondsUpdate = DateTime.Now;
    private DateTime lastMoveTime = DateTime.Now;
    private bool isGameOver;

    private const int REASON_TIMES_UP = 0;
    private const int REASON_DEATH = 1;

    public Game(ControlsConsole prevConsole, PlayerData data, SnakeType snake) : base (80, 25)
    {
        prevConsole.Clear();
        prevConsole.Controls.Clear();
        prevConsole.IsFocused = false;
        prevConsole.ClearShiftValues();

        SnakeEntity entity = SnakeEntity.Create(snake);

        player = new Player(data, entity);
        obstacles = new Dictionary<ObstacleEntity, Point>();

        SadConsole.Game.Instance.MonoGameInstance.Window.Title = "Snake Console | Kills: "+ kills +" | Game Ends In: "+ Time.Format(seconds);
        SadConsole.Settings.AllowWindowResize = false;

        loadResources();
        init();
    }

    private void loadResources()
    {
    }

    private void init()
    {
        snakeBody = new List<Point>();
        snakeDirection = new Point(1, 0);

        int startX = 40;
        int startY = 12;

        IsFocused = true;

        for (int i = 0; i < InitialSnakeLength; i++)
        {
            snakeBody.Add(new Point(startX - i, startY));
        }

        GenerateFood();
        GenerateObstacles();

        isGameOver = false;
        kills = 0;
        seconds = 600;

        UpdateWindowTitle();
    }

    private int GetAdjustedGameSpeed()
    {
        if (snakeDirection.Y != 0)
            return (int)(gameSpeed * 1.5f);
        else
            return gameSpeed;
    }

    public override void Update(TimeSpan delta)
    {
        base.Update(delta);

        if (isGameOver)
        {
            return;
        }

        if ((DateTime.Now - lastSecondsUpdate).TotalMilliseconds >= 1000)
        {
            seconds--;

            if (seconds <= 0)
            {
                GameOver("Time's up!", REASON_TIMES_UP);
                return;
            }
            lastSecondsUpdate = DateTime.Now;
            UpdateWindowTitle();
        }

        int currentSpeed = GetAdjustedGameSpeed();

        if ((DateTime.Now - lastMoveTime).TotalMilliseconds >= currentSpeed)
        {
            MoveSnake();
            UpdateObstacles();
            CheckCollisions();
            lastMoveTime = DateTime.Now;
        }

        Render();
    }

    private void UpdateObstacles()
    {
        List<ObstacleEntity> obstaclesToRemove = new List<ObstacleEntity>();
        foreach (var obstacleEntry in obstacles)
        {
            ObstacleEntity obstacle = obstacleEntry.Key;
            if (obstacle.getCanMove())
            {
                Point oldPosition = obstacleEntry.Value;

                Point snakeHead = snakeBody.Count > 0 ? snakeBody[0] : Point.None;

                if (obstacle.getMovementPattern() == MovementPattern.TowardsPlayer && snakeBody.Count > 0)
                {
                    obstacle.Update(TimeSpan.FromMilliseconds(gameSpeed), snakeHead);
                }
                else
                {
                    obstacle.Update(TimeSpan.FromMilliseconds(gameSpeed));
                }

                if (obstacle.IsOutOfBounds(1, 78, 1, 23))
                {
                    obstacle.BounceFromBounds(1, 78, 1, 23);
                }

                obstacles[obstacle] = obstacle.Position;
            }
        }

        foreach (var obstacle in obstaclesToRemove)
        {
            obstacles.Remove(obstacle);
        }
    }

    private void CheckCollisions()
    {
        if (snakeBody.Count == 0) return;

        Point head = snakeBody[0];

        if (head.X <= 0 || head.X >= 79 || head.Y <= 0 || head.Y >= 24)
        {
            GameOver("Hit the wall!", REASON_DEATH);
            return;
        }

        for (int i = 1; i < snakeBody.Count; i++)
        {
            if (head == snakeBody[i])
            {
                GameOver("Ate yourself!", REASON_DEATH);
                return;
            }
        }

        List<ObstacleEntity> obstacleToRemove = new List<ObstacleEntity>();
        bool hasCollided = false;

        for (int segmentIndex = 0; segmentIndex < snakeBody.Count; segmentIndex++)
        {
            if (hasCollided) break;

            Point segment = snakeBody[segmentIndex];

            foreach (var obstacleEntry in obstacles)
            {
                ObstacleEntity obstacle = obstacleEntry.Key;
                Point obstaclePos = obstacleEntry.Value;

                if (obstacle.CollideWith(segment))
                {
                    hasCollided = true;
                    obstacle.onCollision(player.getSnakeEntity());
                    LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.snake_eating_crash_sound), player.getPlayerData().Settings.GameSoundEffectVolume / 100f);
                    if (!player.getSnakeEntity().IsAlive())
                    {
                        GameOver($"Hit by {obstacle.getName()}!", REASON_DEATH);
                        return;
                    }

                    if (obstacle.isDestructible())
                    {
                        obstacleToRemove.Add(obstacle);
                    }

                    break;
                }
            }
        }

        foreach (var obstacle in obstacleToRemove)
        {
            obstacles.Remove(obstacle);
        }

        if (seconds <= 0)
        {
            GameOver("Time's up!", REASON_TIMES_UP);
        }
    }

    private void GenerateObstacles()
    {
        Random random = new Random();
        const int obstacleCount = 10;

        for (int i = 0; i < obstacleCount; i++)
        {
            Point? validPos = FindValidObstaclePosition();
            if (validPos.HasValue)
            {
                ObstacleEntity obstacle;

                int obstacleType = random.Next(0, 100);
                if (obstacleType < 20)
                {
                    obstacle = new ChaseObstacle();
                }
                else
                {
                    obstacle = random.Next(0, 4) switch
                    {
                        0 => new Stone(),
                        1 => new Spike(),
                        2 => new Bush(),
                        3 => new Wall(),
                        _ => new Stone()
                    };
                }

                obstacle.Position = validPos.Value;

                obstacles.Add(obstacle, validPos.Value);
            }
        }
    }

    private Point? FindValidObstaclePosition()
    {
        Random random = new Random();

        for (int attempt = 0; attempt < 50; attempt++)
        {
            int x = random.Next(2, 78);
            int y = random.Next(2, 23);
            Point pos = new Point(x, y);

            if (!snakeBody.Contains(pos) && pos != foodPosition)
            {
                bool positionFree = true;
                foreach (var obstaclePos in obstacles.Values)
                {
                    if (obstaclePos == pos)
                    {
                        positionFree = false;
                        break;
                    }
                }

                if (positionFree)
                    return pos;
            }
        }

        return null;
    }

    private void MoveSnake()
    {
        Point newHead = snakeBody[0] + snakeDirection;

        snakeBody.Insert(0, newHead);

        snakeBody.RemoveAt(snakeBody.Count - 1);

        if (newHead == foodPosition)
        {
            kills += 1;
            LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.snake_eating_sound),
                player.getPlayerData().Settings.GameSoundEffectVolume / 100f);
            GenerateFood();
        }
    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        if (keyboard.IsKeyPressed(Keys.Up) && snakeDirection != new Point(0, 1))
            snakeDirection = new Point(0, -1);
        else if (keyboard.IsKeyPressed(Keys.Down) && snakeDirection != new Point(0, -1))
            snakeDirection = new Point(0, 1);
        else if (keyboard.IsKeyPressed(Keys.Left) && snakeDirection != new Point(1, 0))
            snakeDirection = new Point(-1, 0);
        else if (keyboard.IsKeyPressed(Keys.Right) && snakeDirection != new Point(-1, 0))
            snakeDirection = new Point(1, 0);

        return base.ProcessKeyboard(keyboard);
    }

    private void GenerateFood()
    {
        Random random = new Random();

        while (true)
        {
            int x = random.Next(1, 78);
            int y = random.Next(1, 23);

            foodPosition = new Point(x, y);

            if (!snakeBody.Contains(foodPosition))
                break;
        }
    }

    private void GameOver(string reason, int reasonId)
    {
        GlobalAudio.StopAll();
        switch (reasonId)
        {
            case REASON_TIMES_UP:
                LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.gameover_time_up_sound), player.getPlayerData().Settings.GameSoundEffectVolume / 100f);
                break;
            case REASON_DEATH:
                LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.gameover_sound), player.getPlayerData().Settings.GameSoundEffectVolume / 100f);
                break;
        }

        isGameOver = true;

        int finalScore = CalculateFinalScore();

        Random random = new Random();
        int moneyReward = finalScore * random.Next(1, 25);
        int playerMoney = player.getPlayerData().Money;

        if (playerMoney + moneyReward >= 10000)
        {
            player.getPlayerData().Money += Math.Max(0, 10000);
        }
        else
        {
            player.getPlayerData().Money += Math.Max(0, moneyReward);
        }

        player.getPlayerData().History.Add(new MatchRecord
        {
            Character = player.getSnakeEntity().getName(),
            Kills = kills,
            TimeEnds = Time.Format(seconds),
            Score = CalculateFinalScore(),
            DateTime = DateTime.Now.ToString("dd/MM/yyyy (HH:mm)")
        });

        PlayerDataManager playerDataManager = new PlayerDataManager();
        playerDataManager.Save(player.getPlayerData());

        this.Clear();

        FontSize = new Point(10, 20);

        this.Print(25, 6, ColoredString.FromGradient(Color.Wheat, "GAME OVER"));
        this.Print(19, 8, $"Reason: {reason}");
        this.Print(19, 9, $"Final Score: {CalculateFinalScore()}");
        this.Print(19, 10, $"Money: +{moneyReward.ToString("N0")}");

        this.Print(19, 12, ColoredString.FromGradient(Color.White, "Press ENTER to return to"));
        this.Print(19, 13, ColoredString.FromGradient(Color.White, "Main Menu"));

        SelectionButton btnOk = new SelectionButton(18, 1);
        btnOk.Text = "OK";
        btnOk.Position = new Point(22, 1);
        btnOk.IsVisible = false;

        btnOk.Click += (s, e) =>
        {
            ResetWindowTitle();
            SadConsole.Game.Instance.Screen = new MainMenu(this, player.getPlayerData());
            GlobalAudio.StopAll();
            LocalAudio.StopAll();
            float normalizedBgmVolume = player.getPlayerData().Settings.LobbyBgmVolume / 100f;
            GlobalAudio.PlayBgmIfNotPlaying(Resources.getMusic(Resources.lobby_bgm), normalizedBgmVolume);
            LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.click_sound), player.getPlayerData().Settings.LobbySoundEffectVolume / 100f);
        };
        btnOk.Unfocused += (s, e) =>
        {
            LocalAudio.PlaySoundEffect(Resources.getSoundEffect(Resources.switch_sound), player.getPlayerData().Settings.LobbySoundEffectVolume / 100f);
        };

        var customColors = Colors.Default.Clone();
        customColors.ControlBackgroundNormal.SetColor(Color.Black);
        customColors.ControlForegroundNormal.SetColor(Color.DarkGray);
        customColors.ControlBackgroundFocused.SetColor(Color.Black);
        customColors.ControlForegroundFocused.SetColor(Color.Wheat);
        customColors.RebuildAppearances();

        btnOk.SetThemeColors(customColors);

        Controls.Add(btnOk);

        Controls.FocusedControl = btnOk;
        IsFocused = true;

    }

    private void Render()
    {
        if (isGameOver) return;

        this.Clear();

        foreach (var obstacleEntry in obstacles)
        {
            ObstacleEntity obstacle = obstacleEntry.Key;
            Point position = obstacleEntry.Value;
            this.SetCellAppearance(position.X, position.Y, new ColoredGlyph(obstacle.getColor(), Color.Black, obstacle.getSymbol()));
        }

        for (int x = 0; x < 80; x++)
        {
            this.SetCellAppearance(x, 0, new ColoredGlyph(Color.Gray, Color.Black, 205));
            this.SetCellAppearance(x, 24, new ColoredGlyph(Color.Gray, Color.Black, 205));
        }

        for (int y = 0; y < 25; y++)
        {
            this.SetCellAppearance(0, y, new ColoredGlyph(Color.Gray, Color.Black, 186));
            this.SetCellAppearance(79, y, new ColoredGlyph(Color.Gray, Color.Black, 186));
        }

        // Corner characters
        this.SetCellAppearance(0, 0, new ColoredGlyph(Color.Gray, Color.Black, 201));
        this.SetCellAppearance(79, 0, new ColoredGlyph(Color.Gray, Color.Black, 187));
        this.SetCellAppearance(0, 24, new ColoredGlyph(Color.Gray, Color.Black, 200));
        this.SetCellAppearance(79, 24, new ColoredGlyph(Color.Gray, Color.Black, 188));

        // Draw snake body
        foreach (Point segment in snakeBody)
        {
            this.SetCellAppearance(segment.X, segment.Y,
                new ColoredGlyph(player.getSnakeEntity().getBodyColor(), Color.Black, 'O'));
        }

        // Draw snake head
        if (snakeBody.Count > 0)
        {
            this.SetCellAppearance(snakeBody[0].X, snakeBody[0].Y,
                new ColoredGlyph(player.getSnakeEntity().getHeadColor(), Color.Black, '@'));
        }

        // Draw food
        this.SetCellAppearance(foodPosition.X, foodPosition.Y,
            new ColoredGlyph(Color.Red, Color.Black, '*'));

        // Draw UI
        this.Print(2, 0, ColoredString.FromGradient(Color.White, "Kills: "+ kills));
        this.Print(16, 0, ColoredString.FromGradient(Color.White, "HP: ("+ player.getSnakeEntity().getHp() +"/"+player.getSnakeEntity().getMaxHp()+")"));
        this.Print(56, 0, ColoredString.FromGradient(Color.White, "Game Ends In: "+ Time.Format(seconds)));
        this.Print(2, 24, ColoredString.FromGradient(Color.White, "Use arrow keys to move"));
        this.Print(2, 24, ColoredString.FromGradient(Color.White, "Use arrow keys to move"));
    }

    private int CalculateFinalScore()
    {
        // Formula: finalScore = (foodScore * timeMultiplier) + snakeLengthBonus

        int foodScore = kills;
        int timeRemaining = seconds;

        float timeMultiplier = 1.0f + (timeRemaining / 600f);
        int finalScore = (int)(foodScore * timeMultiplier);

        return finalScore;
    }

    private void UpdateWindowTitle()
    {
        SadConsole.Game.Instance.MonoGameInstance.Window.Title =
            "Snake Console | HP: ("+ player.getSnakeEntity().getHp() +"/"+player.getSnakeEntity().getMaxHp()+") | Kills: "+kills+" | Game Ends In: "+Time.Format(seconds);
    }

    private void ResetWindowTitle()
    {
        SadConsole.Game.Instance.MonoGameInstance.Window.Title = "Snake Console";
    }

}