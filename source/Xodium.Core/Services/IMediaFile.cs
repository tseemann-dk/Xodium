using System;
using System.IO;
using System.Threading.Tasks;

namespace Xodium.Services
{
    public enum MediaFileType { Image, Video }

    public interface IMediaFile
    {
        string Path { get; }
        MediaFileType Type { get; }

        Func<Task<Stream>> GetStream { get; }
    }
}
