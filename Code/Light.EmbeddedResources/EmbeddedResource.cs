using System;
using System.IO;
using System.Runtime.CompilerServices;
using Light.GuardClauses;

namespace Light.EmbeddedResources
{
    public static class EmbeddedResource
    {
        public static Stream GetEmbeddedStream(this Type typeInSameNamespace, string resourceName) =>
            GetEmbeddedStreamInternal(typeInSameNamespace.MustNotBeNull(nameof(typeInSameNamespace)), resourceName);

        public static Stream GetEmbeddedStream<T>(this T instanceInSameNamespace, string resourceName) =>
            GetEmbeddedStreamInternal(instanceInSameNamespace.MustNotBeNullReference(nameof(instanceInSameNamespace))!.GetType(), resourceName);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Stream GetEmbeddedStreamInternal(Type typeInSameNamespace, string resourceName)
        {
            var stream = typeInSameNamespace.Assembly.GetManifestResourceStream(typeInSameNamespace, resourceName);
            if (stream == null)
                throw new EmbeddedResourceNotFoundException(nameof(resourceName), $"There is no resource called \"{typeInSameNamespace.Namespace + "." + resourceName}\" in assembly \"{typeInSameNamespace.Assembly}\".");
            return stream;
        }

        public static void CopyEmbeddedStreamToFile(this Type typeInSameNameSpace, string resourceName, string filePath) =>
            CopyEmbeddedStreamToFileInternal(typeInSameNameSpace.MustNotBeNull(nameof(typeInSameNameSpace)), resourceName, filePath);

        public static void CopyEmbeddedStreamToFile<T>(this T instanceInSameNameSpace, string resourceName, string filePath) =>
            CopyEmbeddedStreamToFileInternal(instanceInSameNameSpace.MustNotBeNullReference(nameof(instanceInSameNameSpace))!.GetType(), resourceName, filePath);

        private static void CopyEmbeddedStreamToFileInternal(Type typeInSameNameSpace, string resourceName, string filePath)
        {
            using var embeddedStream = GetEmbeddedStreamInternal(typeInSameNameSpace, resourceName);
            var bufferSize = (int) Math.Min(embeddedStream.Length, 84975); // 84975 bytes is the largest array in 64-Bit mode that is still placed in the Small-Object-Heap
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read, bufferSize);
            embeddedStream.CopyTo(fileStream);
        }
    }
}