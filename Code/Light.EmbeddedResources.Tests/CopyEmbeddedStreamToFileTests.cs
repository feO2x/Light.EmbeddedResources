using System;
using System.IO;
using FluentAssertions;
using Xunit;

namespace Light.EmbeddedResources.Tests
{
    public sealed class CopyEmbeddedStreamToFileTests
    {
        private const string TargetFile = "TargetFile.json";

        [Fact]
        public static void CopyExistingResourceViaType()
        {
            FileInfo? fileInfo = null;
            try
            {
                typeof(CopyEmbeddedStreamToFileTests).CopyEmbeddedStreamToFile(EmbeddedResourceNames.Existing, TargetFile);
                fileInfo = new FileInfo(TargetFile);
                fileInfo.Exists.Should().BeTrue();
            }
            finally
            {
                fileInfo?.Delete();
            }
        }

        [Fact]
        public static void CopyNonExistingResourceViaType()
        {
            Action act = () => typeof(CopyEmbeddedStreamToFileTests).CopyEmbeddedStreamToFile(EmbeddedResourceNames.NonExisting, TargetFile);

            act.CheckEmbeddedResourceNotFoundException();
        }

        [Fact]
        public void CopyExistingResourceViaInstance()
        {
            FileInfo? fileInfo = null;
            try
            {
                this.CopyEmbeddedStreamToFile(EmbeddedResourceNames.Existing, TargetFile);
                fileInfo = new FileInfo(TargetFile);
                fileInfo.Exists.Should().BeTrue();
            }
            finally
            {
                fileInfo?.Delete();
            }
        }

        [Fact]
        public void CopyNonExistingResourceViaInstance()
        {
            Action act = () => this.CopyEmbeddedStreamToFile(EmbeddedResourceNames.NonExisting, TargetFile);

            act.CheckEmbeddedResourceNotFoundException();
        }
    }
}