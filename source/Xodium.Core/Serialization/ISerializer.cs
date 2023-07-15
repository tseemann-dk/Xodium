using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Xodium.Serialization
{
    public interface ISerializer
    {
        ValueTask<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default);
        T Deserialize<T>(Stream stream);
        Task SerializeAsync<T>(Stream stream, T value, CancellationToken cancellationToken = default);
        void Serialize<T>(Stream stream, T value);
    }

    public static class SerializerExtensions
    {
        public static T Deserialize<T>(
            this ISerializer serializer,
            string source,
            Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            var bytes = encoding.GetBytes(source);
            var stream = new MemoryStream(bytes);

            return serializer.Deserialize<T>(stream);
        }

        public static ValueTask<T> DeserializeAsync<T>(
            this ISerializer serializer,
            string source,
            Encoding encoding = null,
            CancellationToken cancellationToken = default)
        {
            encoding ??= Encoding.UTF8;
            var bytes = encoding.GetBytes(source);
            var stream = new MemoryStream(bytes);

            return serializer.DeserializeAsync<T>(stream, cancellationToken);
        }

        public static string Serialize(
            this ISerializer serializer,
            object obj,
            Encoding encoding = null)
        {
            var stream = new MemoryStream();
            serializer.Serialize(stream, obj);
            stream.Position = 0;

            using var reader = new StreamReader(stream, encoding ?? Encoding.UTF8);
            return reader.ReadToEnd();
        }

        public static async Task<string> SerializeAsync(
            this ISerializer serializer,
            object obj,
            Encoding encoding = null,
            CancellationToken cancellationToken = default)
        {
            var stream = new MemoryStream();
            await serializer.SerializeAsync(stream, obj, cancellationToken);
            stream.Position = 0;

            using var reader = new StreamReader(stream, encoding ?? Encoding.UTF8);
            return await reader.ReadToEndAsync();
        }
    }
}
