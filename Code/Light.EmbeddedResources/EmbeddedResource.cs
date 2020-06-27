using System;
using System.IO;
using Light.GuardClauses;

namespace Light.EmbeddedResources
{
    public static class EmbeddedResource
    {
        public static Stream GetEmbeddedStream(this Type typeInSameNamespace, string resourceName)
        {
            var stream = typeInSameNamespace.MustNotBeNull(nameof(typeInSameNamespace)).Assembly.GetManifestResourceStream(typeInSameNamespace, resourceName);
            if (stream == null)
                throw new EmbeddedResourceNotFoundException(nameof(resourceName), $"There is no resource called \"{typeInSameNamespace.Namespace + "." + resourceName}\" in assembly \"{typeInSameNamespace.Assembly}\".");

            return stream;
        }
    }
}