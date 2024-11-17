using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace JO.Tools.Helpers
{
    public static class EnumHelper<T>
        where T : struct, Enum
    {
        public static IList<T> GetValues(Enum value)
        {
            var enumValues = new List<T>();

            foreach (FieldInfo fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumValues.Add((T)Enum.Parse(value.GetType(), fi.Name, false));
            }
            return enumValues;
        }

        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static IList<string> GetNames(Enum value)
        {
            return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        public static IList<string> GetDisplayValues(Enum value)
        {
            return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
        }

        private static string? lookupResource(Type resourceManagerProvider, string resourceKey)
        {
            var resourceKeyProperty = resourceManagerProvider.GetProperty(resourceKey,
                BindingFlags.Static | BindingFlags.Public, null, typeof(string),
                new Type[0], null);
            if (resourceKeyProperty != null)
            {
                return resourceKeyProperty.GetMethod.Invoke(null, null) as string;
            }

            return resourceKey;
        }

        public static string GetDisplayValue(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes == null || !descriptionAttributes.Any())
                return string.Empty;

            if (descriptionAttributes[0].ResourceType != null)
                return lookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);

            return (descriptionAttributes.Any()) ? descriptionAttributes[0].Name : value.ToString();
        }
    }
}
