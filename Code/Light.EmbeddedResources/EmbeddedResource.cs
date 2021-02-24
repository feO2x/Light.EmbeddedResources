using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Light.GuardClauses;

namespace Light.EmbeddedResources
{
    /// <summary>
    /// Provides extensions methods to easily access embedded resources.
    /// </summary>
    public static class EmbeddedResource
    {
        /// <summary>
        /// Gets an embedded resource as a string. The name is assembled from the namespace of the specified type and the resource name.
        /// </summary>
        /// <param name="typeInSameNamespace">A type that resides in the same assembly and namespace as the target embedded resource.</param>
        /// <param name="name">The case-sensitive name of the resource.</param>
        /// <param name="encoding">The encoding used to decode the embedded stream.</param>
        /// <exception cref="EmbeddedResourceNotFoundException">Thrown when the specified resource cannot be found.</exception>
        /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is an empty string.</exception>
        public static string GetEmbeddedResource(this Type typeInSameNamespace, string name, Encoding? encoding = null) =>
            GetEmbeddedInternal(typeInSameNamespace.MustNotBeNull(nameof(typeInSameNamespace)), name, encoding);

        /// <summary>
        /// Gets an embedded resource as a string. The name is assembled from the namespace of the specified instance and the resource name.
        /// </summary>
        /// <param name="instanceInSameNamespace">An instance whose type resides in the same assembly and namespace as the target embedded resource.</param>
        /// <param name="name">The case-sensitive name of the resource.</param>
        /// <param name="encoding">The encoding used to decode the embedded stream.</param>
        /// <exception cref="EmbeddedResourceNotFoundException">Thrown when the specified resource cannot be found.</exception>
        /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is an empty string.</exception>
        public static string GetEmbeddedResource<T>(this T instanceInSameNamespace, string name, Encoding? encoding = null) =>
            GetEmbeddedInternal(instanceInSameNamespace.MustNotBeNullReference(nameof(instanceInSameNamespace))!.GetType(), name, encoding);

        private static string GetEmbeddedInternal(Type typeInSameNamespace, string name, Encoding? encoding)
        {
            using var stream = GetEmbeddedStreamInternal(typeInSameNamespace, name);
            using var streamReader = encoding == null ? new StreamReader(stream) : new StreamReader(stream, encoding);
            return streamReader.ReadToEnd();
        }

        /// <summary>
        /// Gets a stream to an embedded resource. The name is assembled from the namespace of the specified type and the resource name.
        /// </summary>
        /// <param name="typeInSameNamespace">A type that resides in the same assembly and namespace as the target embedded resource.</param>
        /// <param name="name">The case-sensitive name of the resource.</param>
        /// <exception cref="EmbeddedResourceNotFoundException">Thrown when the specified resource cannot be found.</exception>
        /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is an empty string.</exception>
        public static Stream GetEmbeddedStream(this Type typeInSameNamespace, string name) =>
            GetEmbeddedStreamInternal(typeInSameNamespace.MustNotBeNull(nameof(typeInSameNamespace)), name);

        /// <summary>
        /// Gets a stream to an embedded resource. The name is assembled from the namespace of the specified instance and the resource name.
        /// </summary>
        /// <param name="instanceInSameNamespace">An instance whose type resides in the same assembly and namespace as the target embedded resource.</param>
        /// <param name="name">The case-sensitive name of the resource.</param>
        /// <exception cref="EmbeddedResourceNotFoundException">Thrown when the specified resource cannot be found.</exception>
        /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is an empty string.</exception>
        public static Stream GetEmbeddedStream<T>(this T instanceInSameNamespace, string name) =>
            GetEmbeddedStreamInternal(instanceInSameNamespace.MustNotBeNullReference(nameof(instanceInSameNamespace))!.GetType(), name);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Stream GetEmbeddedStreamInternal(Type typeInSameNamespace, string name)
        {
            var stream = typeInSameNamespace.Assembly.GetManifestResourceStream(typeInSameNamespace, name);
            if (stream == null)
                throw new EmbeddedResourceNotFoundException(nameof(name), $"There is no resource called \"{typeInSameNamespace.Namespace + "." + name}\" in assembly \"{typeInSameNamespace.Assembly}\".");
            return stream;
        }

        /// <summary>
        /// Copies an embedded resource stream to a file. The name is assembled from the namespace of the specified type and the resource name.
        /// </summary>
        /// <param name="typeInSameNamespace">A type that resides in the same assembly and namespace as the target embedded resource.</param>
        /// <param name="name">The case-sensitive name of the resource.</param>
        /// <param name="filePath">The relative or absolute path for the file that will be written to.</param>
        /// <exception cref="EmbeddedResourceNotFoundException">Thrown when the specified resource cannot be found.</exception>
        /// <exception cref="IOException">Thrown when an I/O error occured during copying.</exception>
        /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is an empty string.</exception>
        public static void CopyEmbeddedStreamToFile(this Type typeInSameNamespace, string name, string filePath) =>
            CopyEmbeddedStreamToFileInternal(typeInSameNamespace.MustNotBeNull(nameof(typeInSameNamespace)), name, filePath);

        /// <summary>
        /// Copies an embedded resource stream to a file. The name is assembled from the namespace of the specified type and the resource name.
        /// </summary>
        /// <param name="instanceInSameNamespace">An instance whose type resides in the same assembly and namespace as the target embedded resource.</param>
        /// <param name="name">The case-sensitive name of the resource.</param>
        /// <param name="filePath">The relative or absolute path for the file that will be written to.</param>
        /// <exception cref="EmbeddedResourceNotFoundException">Thrown when the specified resource cannot be found.</exception>
        /// <exception cref="IOException">Thrown when an I/O error occured during copying.</exception>
        /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is an empty string.</exception>
        public static void CopyEmbeddedStreamToFile<T>(this T instanceInSameNamespace, string name, string filePath) =>
            CopyEmbeddedStreamToFileInternal(instanceInSameNamespace.MustNotBeNullReference(nameof(instanceInSameNamespace))!.GetType(), name, filePath);

        private static void CopyEmbeddedStreamToFileInternal(Type typeInSameNameSpace, string resourceName, string filePath)
        {
            using var embeddedStream = GetEmbeddedStreamInternal(typeInSameNameSpace, resourceName);
            var bufferSize = (int) Math.Min(embeddedStream.Length, 84975); // 84975 bytes is the largest array in 64-Bit mode that is still placed in the Small-Object-Heap
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read, bufferSize);
            embeddedStream.CopyTo(fileStream);
        }
    }
}