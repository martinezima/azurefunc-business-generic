using System.Text;
namespace AppBusinessGeneric.RestClient.Helpers;

public class QueryStringHelper
{
    public static string BuildQueryStrings(Dictionary<string, object> parameters)
    {
        var sb = new StringBuilder();
        foreach (var p in parameters)
        {
            var fragment = BuildQueryStringFragment(p.Key, p.Value);
            if (!string.IsNullOrEmpty(fragment))
            {
                sb.Append(sb.Length == 0 ? "?" : "&");
                sb.Append(fragment);
            }
        }
        return sb.ToString();
    }
    private static string? BuildQueryStringFragment(string name, object value)
    {
        if (value == null)
        {
            return null;
        }
        if (value is DateTime dateTime)
        {
            return $"{name}={dateTime.ToString("o")}";
        }
        return $"{name}={value}";
    }
}