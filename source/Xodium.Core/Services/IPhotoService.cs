using System.IO;
using System.Threading.Tasks;

namespace Xodium.Services
{
    public class Photo
    {
        public Photo(string name, Stream stream)
        {
            Name = name;
            Stream = stream;
        }

        public string Name { get; }
        public Stream Stream { get; }
    }

    public interface IPhotoService
    {
        Task<Photo> TakePhoto();
        Task<Photo> PickPhoto();
    }
}
