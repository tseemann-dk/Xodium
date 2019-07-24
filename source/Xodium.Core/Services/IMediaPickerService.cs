using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xodium.Services
{
    public interface IMediaPickerService
    {
        Task<IReadOnlyList<IMediaFile>> PickPhotos(string title);
        Task<IReadOnlyList<IMediaFile>> PickVideos(string title);
    }
}
