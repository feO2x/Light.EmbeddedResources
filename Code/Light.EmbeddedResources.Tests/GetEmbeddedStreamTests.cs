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

            act.CheckEmbeddedResourceNotFoundException();
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

            act.CheckEmbeddedResourceNotFoundException();
        }
    }
}