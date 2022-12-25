using System.ComponentModel;
using System.Text;

namespace OrganizationNotificationService.Utils;

public static class EnumUtils
{
    public static bool BelongToType<T>(string text) where T : Enum
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            return false;

        var list = Enum.GetValues(typeof(T)) as T[];
        return list?.Any(x => x.ToString() == text) ?? false;
    }


    public static T ToEnum<T>(this string text) where T : Enum
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            throw new ArgumentNullException(nameof(T), "invalid value for enum");

        var list = Enum.GetValues(typeof(T)) as T[];

        return list!.FirstOrDefault(x => x.ToString() == text) ??
               throw new ArgumentOutOfRangeException(nameof(T), "invalid value for enum");
    }
}