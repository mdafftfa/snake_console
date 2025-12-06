using SadConsole;
using SadConsole.Input;
using SadConsole.UI;
using SadRogue.Primitives;
using System.Collections.Generic;
using snake_console.Data;
using snake_console.Objects.Snakes; // Tambahkan ini untuk SnakeType
using snake_console.Utils; // Untuk Time utility jika ada

namespace snake_console.Game;

public class Gameas : ControlsConsole
{
    private PlayerData playerData;
    private SnakeType snakeType;

    private int score;
    private int seconds;
    private bool isGameOver;

    // Snake components
    private List<Point> snakeBody;
    private Point snakeDirection;
    private Point foodPosition;
    private const int InitialSnakeLength = 3;

    // Game settings
    private const int GameSpeed = 150; // ms per move
    private DateTime lastUpdateTime;

    public Gameas(ControlsConsole prevConsole, PlayerData playerData, SnakeType snakeType) : base(80, 25)
    {
        prevConsole.Clear();
        prevConsole.Controls.Clear();
        prevConsole.IsFocused = false;
        prevConsole.ClearShiftValues();

        this.playerData = playerData;
        this.snakeType = snakeType;
        this.seconds = 600; // 10 minutes in seconds

        SadConsole.Game.Instance.MonoGameInstance.Window.Title = "Snake Console";
        SadConsole.Settings.AllowWindowResize = false;

        InitializeGame();
    }

    private void InitializeGame()
    {
        // Initialize snake
        snakeBody = new List<Point>();
        snakeDirection = new Point(1, 0); // Start moving right

        // Create initial snake in the middle of the screen
        int startX = 40;
        int startY = 12;

        for (int i = 0; i < InitialSnakeLength; i++)
        {
            snakeBody.Add(new Point(startX - i, startY));
        }

        // Generate first food
        GenerateFood();

        lastUpdateTime = DateTime.Now;
        isGameOver = false;
        score = 0;

        UpdateWindowTitle();
    }

    private void GenerateFood()
    {
        Random random = new Random();

        while (true)
        {
            int x = random.Next(1, 78);  // Leave border
            int y = random.Next(1, 23);  // Leave border

            foodPosition = new Point(x, y);

            // Make sure food doesn't spawn on snake
            if (!snakeBody.Contains(foodPosition))
                break;
        }
    }

    public override void Update(TimeSpan delta)
    {
        base.Update(delta);

        if (isGameOver)
            return;

        // Update game timer
        if ((DateTime.Now - lastUpdateTime).TotalMilliseconds >= GameSpeed)
        {
            MoveSnake();
            CheckCollisions();
            lastUpdateTime = DateTime.Now;

            // Update game timer (countdown)
            if (seconds > 0)
                seconds--;

            UpdateWindowTitle();
        }

        // Render game
        Render();
    }

    private void MoveSnake()
    {
        // Create new head position
        Point newHead = snakeBody[0] + snakeDirection;

        // Add new head
        snakeBody.Insert(0, newHead);

        // If snake didn't eat food, remove tail
        if (newHead != foodPosition)
        {
            snakeBody.RemoveAt(snakeBody.Count - 1);
        }
        else
        {
            // Snake ate food
            score += 10;
            GenerateFood();
        }
    }

    private void CheckCollisions()
    {
        Point head = snakeBody[0];

        // Check wall collision
        if (head.X <= 0 || head.X >= 79 || head.Y <= 0 || head.Y >= 24)
        {
            GameOver("Hit the wall!");
            return;
        }

        // Check self collision
        for (int i = 1; i < snakeBody.Count; i++)
        {
            if (head == snakeBody[i])
            {
                GameOver("Ate yourself!");
                return;
            }
        }

        // Check timer
        if (seconds <= 0)
        {
            GameOver("Time's up!");
        }
    }

    private void GameOver(string reason)
    {
        isGameOver = true;

        // Add to player history - PERBAIKAN: DateTime harus string atau ubah tipe MatchRecord
        playerData.History.Add(new MatchRecord
        {
            Character = EnumExtensions.GetDescription(snakeType), // Menggunakan Description bukan ToString
            Kills = score / 10, // Assuming 10 points per kill
            TimeEnds = TimeSpan.FromSeconds(600 - seconds).ToString(@"mm\:ss"),
            Score = score,
            DateTime = DateTime.Now.ToString("dd/MM/yyyy (HH:mm)") // Convert ke string
        });

        // Save player data
        PlayerDataManager playerDataManager = new PlayerDataManager();
        playerDataManager.Save(playerData);

        // Show game over screen
        this.Clear();
        this.Print(30, 10, "GAME OVER");
        this.Print(25, 12, $"Reason: {reason}");
        this.Print(25, 14, $"Final Score: {score}");
        this.Print(25, 16, "Press SPACE to return to menu");
    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        if (isGameOver)
        {
            if (keyboard.IsKeyPressed(Keys.Space))
            {
                SadConsole.Game.Instance.Screen = new MainMenu(this, playerData);
            }
            return true;
        }

        // Handle snake direction changes
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

    private void Render()
    {
        if (isGameOver)
            return;

        // Clear screen
        this.Clear();

        // Draw border - PERBAIKAN: Menggunakan DrawLine atau Rectangle
        // Cara 1: Menggunakan DrawLine untuk membuat border
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
                new ColoredGlyph(GetSnakeColor(), Color.Black, 'O'));
        }

        // Draw snake head
        if (snakeBody.Count > 0)
        {
            this.SetCellAppearance(snakeBody[0].X, snakeBody[0].Y,
                new ColoredGlyph(GetSnakeHeadColor(), Color.Black, '@'));
        }

        // Draw food
        this.SetCellAppearance(foodPosition.X, foodPosition.Y,
            new ColoredGlyph(Color.Red, Color.Black, '*'));

        // Draw UI
        this.Print(2, 0, $"Score: {score}");
        this.Print(60, 0, $"Time: {TimeSpan.FromSeconds(seconds):mm\\:ss}");
        this.Print(2, 24, "Use arrow keys to move");
    }

    private Color GetSnakeColor()
    {
        // Return different colors based on snake type
        return snakeType switch
        {
            SnakeType.GarterSnake => Color.Green,
            SnakeType.Imperator => Color.Brown,
            SnakeType.Viper => Color.Yellow,
            SnakeType.Ratsnake => Color.Orange,
            SnakeType.KingCobra => Color.DarkGreen,
            SnakeType.Belcher => Color.Cyan,
            SnakeType.Rattlesnake => Color.Gray,
            SnakeType.BlueKrait => Color.Blue,
            SnakeType.Python => Color.DarkRed,
            _ => Color.White
        };
    }

    private Color GetSnakeHeadColor()
    {
        Color baseColor = GetSnakeColor();
        return new Color(
            Math.Min(255, baseColor.R + 50),
            Math.Min(255, baseColor.G + 50),
            Math.Min(255, baseColor.B + 50),
            baseColor.A
        );
    }

    private void UpdateWindowTitle()
    {
        SadConsole.Game.Instance.MonoGameInstance.Window.Title =
            $"Snake Console | Score: {score} | Time: {TimeSpan.FromSeconds(seconds):mm\\:ss}";
    }
}