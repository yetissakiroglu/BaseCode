using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Economy.Core.Extensions
{
    public static class EnumExtensions
    {

        public static IEnumerable<(int Value, string Name)> GetDisplayValues<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                       .Cast<T>()
                       .Select(e => (
                           Convert.ToInt32(e),
                           e.GetType()
                            .GetMember(e.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()?.Name ?? e.ToString()
                       ));
        }


        public static string GetDescription(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
        public static string GetDisplayName(this Enum value)
        {
            if (value == null) return string.Empty;

            var fieldInfo = value.GetType().GetField(value.ToString());
            var displayAttr = fieldInfo?.GetCustomAttribute<DisplayAttribute>();
            return displayAttr?.Name ?? value.ToString();
        }
        public static string ValueToString(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var enumMember = Enum.GetName(enumType, enumValue);
            if (enumMember == null)
            {
                return string.Empty;
            }
            var fieldInfo = enumType.GetField(enumMember);
            if (fieldInfo == null)
            {
                return string.Empty;
            }
            var descriptionAttribute = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return descriptionAttribute?.Description ?? enumMember ?? string.Empty;
        }
    }
}
