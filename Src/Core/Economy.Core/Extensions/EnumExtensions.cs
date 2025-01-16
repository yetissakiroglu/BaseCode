using System.ComponentModel;

namespace Economy.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
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
