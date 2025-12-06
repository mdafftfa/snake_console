using System.ComponentModel;

namespace snake_console.Objects.Snakes;

public static class EnumExtensions
{

    public static string GetDescription(this Enum value)
    {
        var type = value.GetType();
        var field = type.GetField(value.ToString());
        var attribute = field
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .FirstOrDefault() as DescriptionAttribute;
        return attribute?.Description ?? value.ToString();
    }

    public static T? FromDescription<T>(string description) where T : struct, Enum
    {
        var type = typeof(T);
        foreach (var field in type.GetFields())
        {
            if (!field.IsSpecialName)
            {
                var attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                var desc = attr != null ? attr.Description : field.Name;
                if (desc == description) return (T)field.GetValue(null);
            }
        }
        return null;
    }

}