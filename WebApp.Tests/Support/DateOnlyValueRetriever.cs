using Reqnroll.Assist;

namespace WebApp.Tests.Support;

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