using System.Reflection;
using System.Text.RegularExpressions;

namespace NBS.Appointments.Service.Core.Helpers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DescriptorAttribute : Attribute
    {
        public DescriptorAttribute(string pattern)
        {
            Pattern = pattern;
        }

        public string Pattern { get; init; }
    }

    public static class DescriptorConverter
    {
        public static string ToString<TDescriptor>(TDescriptor descriptor)
        {
            var attribute = typeof(TDescriptor).GetCustomAttribute<DescriptorAttribute>();

            if (attribute == null)
                throw new InvalidOperationException("Type was not decorated with a descriptor attribute.");

            var result = new String(attribute.Pattern);
            var matches = Regex.Matches(attribute.Pattern, "{([A-Za-z0-9]+)}");
            var type = typeof(TDescriptor);

            foreach (Match match in matches)
            {
                var propertyName = match.Groups[1].Value;
                var value = type.GetProperty(propertyName).GetValue(descriptor);
                result = result.Replace(match.Value, value.ToString());
            }

            return result;
        }


        public static TDescriptor Parse<TDescriptor>(string rep) where TDescriptor : class, new()
        {
            if (String.IsNullOrWhiteSpace(rep))
                throw new ArgumentException("Descriptor cannot be null or empty.");

            var attribute = typeof(TDescriptor).GetCustomAttribute<DescriptorAttribute>();

            if (attribute == null)
                throw new InvalidOperationException("Type was not decorated with a descriptor attribute.");

            var parts = attribute.Pattern.Split(":");
            var repParts = rep.Split(":");

            if (parts.Length != repParts.Length)
                throw new FormatException($"Descriptor does not contain the right amount of elements. Expected {parts.Length} but found {repParts.Length}.");

            var dict = new Dictionary<string, string>();

            for (int i = 0; i < parts.Length; i++)
            {
                var part = parts[i];
                var repPart = repParts[i];
                var match = Regex.Match(part, "{([A-Za-z0-9]+)}");
                if (match.Success)
                {
                    var propertyName = match.Groups[1].Value;
                    dict[propertyName] = repPart;
                }
                else if (part != repPart)
                {
                    throw new FormatException($"Expected {part} but found {repPart}.");
                }
            }

            return DictionaryToObject<TDescriptor>(dict);
        }

        private static T DictionaryToObject<T>(IDictionary<string, string> dict) where T : new()
        {
            var t = new T();
            

            foreach (var kvp in dict)
            {
                var property = t.GetType().GetProperty(kvp.Key);
                if (property == null)
                    throw new InvalidOperationException($"Descriptor format expects property {kvp.Key} and it was not found.");                
                try
                {
                    object value = GetValue(property.PropertyType, kvp.Value);
                    property.SetValue(t, value, null);
                }
                catch (Exception ex)
                {
                    throw new FormatException($"The element {property.Name} was not in the correct format.", ex);
                }
            }
            return t;
        }

        private static object GetValue(Type propertyType, string value)
        {
            Type underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            switch (underlyingType.Name)
            {
                case nameof(DateOnly): // Need to add in other specific conversions as we encounter them
                    return DateOnly.Parse(value);
                case nameof(DateTime):
                    return DateTime.Parse(value);
                default:
                    return Convert.ChangeType(value, propertyType);
            }
        }

    }
}
