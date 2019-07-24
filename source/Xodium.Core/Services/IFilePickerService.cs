using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xodium.Services
{
    public interface IFilePickerService
    {
        Task<IFile> PickSingleFile(IEnumerable<string> filters = null);
        Task<IEnumerable<IFile>> PickMultipleFiles(IEnumerable<string> filters = null);
    }
}
