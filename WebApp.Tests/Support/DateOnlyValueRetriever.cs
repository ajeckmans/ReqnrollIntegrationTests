using Reqnroll.Assist;

namespace WebApp.Tests.Support;

/// <summary>
/// This is needed because the DateOnly type is not supported by Reqnroll out of the box (yet).
/// </summary>
public class DateOnlyValueRetriever : IValueRetriever
{
    public bool CanRetrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
    {
        return propertyType == typeof(DateOnly);
    }

    public object Retrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
    {
        return DateOnly.Parse(keyValuePair.Value);
    }
}