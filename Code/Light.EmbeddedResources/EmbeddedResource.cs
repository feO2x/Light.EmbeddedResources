using System;
using System.IO;
using Light.GuardClauses;

namespace Light.EmbeddedResources
{
    public static class EmbeddedResource
    {
        public static Stream GetEmbeddedStream(this Type typeInSameNamespace, string resourceName) =>
            GetEmbeddedStreamInternal(typeInSameNamespace.MustNotBeNull(nameof(typeInSameNamespace)), resourceName);

        public static Stream GetEmbeddedStream<T>(this T instanceInSameNamespace, string resourceName) =>
#pragma warning disable 8602 // MustNotBeNullReference checks explicitly for null
            GetEmbeddedStreamInternal(instanceInSameNamespace.MustNotBeNullReference(nameof(instanceInSameNamespace)).GetType(), resourceName);
#pragma warning restore 8602

        private static Stream GetEmbeddedStreamInternal(Type typeInSameNamespace, string resourceName)
        {
            var stream = typeInSameNamespace.Assembly.GetManifestResourceStream(typeInSameNamespace, resourceName);
            if (stream == null)
                throw new EmbeddedResourceNotFoundException(nameof(resourceName), $"There is no resource called \"{typeInSameNamespace.Namespace + "." + resourceName}\" in assembly \"{typeInSameNamespace.Assembly}\".");
            return stream;
        }
    }
}