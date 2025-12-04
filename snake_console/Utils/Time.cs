namespace snake_console.Utils;

public class Time
{

    public static string Format(int seconds)
    {
        var ts = TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
    }

}