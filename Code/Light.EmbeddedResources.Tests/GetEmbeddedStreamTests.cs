using System;
using FluentAssertions;
using Xunit;

namespace Light.EmbeddedResources.Tests
{
    public sealed class GetEmbeddedStreamTests
    {
        [Fact]
        public static void RetrieveStringOfExistingResourceViaType()
        {
            var content = typeof(GetEmbeddedStreamTests).GetEmbeddedResource(EmbeddedResourceNames.Existing);
            content.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public static void ExceptionOnNonExistingStringResourceViaType()
        {
            Action act = () => typeof(GetEmbeddedStreamTests).GetEmbeddedResource(EmbeddedResourceNames.NonExisting);

            act.CheckEmbeddedResourceNotFoundException();
        }

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
        public void RetrieveStringOfExistingResourceViaInstance()
        {
            var content = this.GetEmbeddedResource(EmbeddedResourceNames.Existing);
            content.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void ExceptionOnNonExistingStringResourceViaInstance()
        {
            Action act = () => this.GetEmbeddedResource(EmbeddedResourceNames.NonExisting);

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