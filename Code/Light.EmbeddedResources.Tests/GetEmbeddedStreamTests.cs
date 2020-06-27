using System;
using FluentAssertions;
using Xunit;

namespace Light.EmbeddedResources.Tests
{
    public sealed class GetEmbeddedStreamTests
    {
        [Fact]
        public static void RetrieveStreamOfExistingResourceViaType()
        {
            using var stream = typeof(GetEmbeddedStreamTests).GetEmbeddedStream(EmbeddedResourceNames.Existing);
            stream.Should().NotBeNull();
        }

        [Fact]
        public static void ExceptionOnStreamThatDoesNotExistViaType()
        {
            Action act = () => typeof(GetEmbeddedStreamTests).GetEmbeddedStream(EmbeddedResourceNames.NonExisting);

            CheckEmbeddedResourceNotFoundException(act);
        }

        [Fact]
        public void RetrieveStreamOfExistingResourceViaInstance()
        {
            using var stream = this.GetEmbeddedStream(EmbeddedResourceNames.Existing);
            stream.Should().NotBeNull();
        }

        [Fact]
        public void ExceptionOnStreamThatDoesNotExistViaInstance()
        {
            Action act = () => this.GetEmbeddedStream(EmbeddedResourceNames.NonExisting);

            CheckEmbeddedResourceNotFoundException(act);
        }

        private static void CheckEmbeddedResourceNotFoundException(Action act)
        {
            var exceptionAssertion = act.Should().Throw<EmbeddedResourceNotFoundException>().Which;
            exceptionAssertion.ParamName.Should().Be("resourceName");
            exceptionAssertion.Message.Should().Contain($"There is no resource called \"{typeof(GetEmbeddedStreamTests).Namespace + "." + EmbeddedResourceNames.NonExisting}\" in assembly \"{typeof(GetEmbeddedStreamTests).Assembly}\".");
        }
    }
}