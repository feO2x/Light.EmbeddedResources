using System;
using FluentAssertions;
using Xunit;

namespace Light.EmbeddedResources.Tests
{
    public static class GetEmbeddedStreamTests
    {
        [Fact]
        public static void RetrieveStreamOfExistingResource()
        {
            using var stream = typeof(GetEmbeddedStreamTests).GetEmbeddedStream("EmbeddedJson.json");
            stream.Should().NotBeNull();
        }

        [Fact]
        public static void ExceptionOnStreamThatDoesNotExist()
        {
            Action act = () => typeof(GetEmbeddedStreamTests).GetEmbeddedStream("NonExisting.txt");

            var exceptionAssertion = act.Should().Throw<EmbeddedResourceNotFoundException>().Which;
            exceptionAssertion.ParamName.Should().Be("resourceName");
            exceptionAssertion.Message.Should().Contain($"There is no resource called \"{typeof(GetEmbeddedStreamTests).Namespace + ".NonExisting.txt"}\" in assembly \"{typeof(GetEmbeddedStreamTests).Assembly}\".");
        }
    }
}