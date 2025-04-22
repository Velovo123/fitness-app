namespace fitness_app.Extensions;

public static class EnumExtensions
{
    public static int ToEnumIndex<TEnum>(this string? value)
        where TEnum : struct, Enum
    {
        return Enum.TryParse<TEnum>(value, out var e) ? Convert.ToInt32(e) : 0;
    }
}