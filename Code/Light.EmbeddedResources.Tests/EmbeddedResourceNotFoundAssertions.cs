using System;
using FluentAssertions;

namespace Light.EmbeddedResources.Tests
{
    public static class EmbeddedResourceNotFoundAssertions
    {
        public static void CheckEmbeddedResourceNotFoundException(this Action act)
        {
            var exceptionAssertion = act.Should().Throw<EmbeddedResourceNotFoundException>().Which;
            exceptionAssertion.ParamName.Should().Be("resourceName");
            exceptionAssertion.Message.Should().Contain($"There is no resource called \"{typeof(GetEmbeddedStreamTests).Namespace + "." + EmbeddedResourceNames.NonExisting}\" in assembly \"{typeof(GetEmbeddedStreamTests).Assembly}\".");
        }
    }
}