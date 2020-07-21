using System;
using System.IO;
using System.Threading.Tasks;

namespace Xodium.Services
{
    public class MediaFile : IMediaFile
    {
        public string Path { get; set; }
        public MediaFileType Type { get; set; }

        public Func<Task<Stream>> GetStream { get; set; }
    }
}