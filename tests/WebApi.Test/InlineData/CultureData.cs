using System.Collections;

namespace WebApi.Test.InlineData;

public class CultureData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "en-US" };
        yield return new object[] { "pt-BR" };
        yield return new object[] { "fr" };
        
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}