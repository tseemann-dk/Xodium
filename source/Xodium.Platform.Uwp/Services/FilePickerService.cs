using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Xodium.Services;

namespace Xodium.Platform.Uwp.Services
{
    public class FilePickerService : IFilePickerService
    {
        public async Task<IFile> PickSingleFile(IEnumerable<string> filters)
        {
            var picker = GetFileOpenPicker(filters);
            var file = await picker.PickSingleFileAsync();
            return file == null ? null : File.FromStorageFile(file);
        }

        public async Task<IEnumerable<IFile>> PickMultipleFiles(IEnumerable<string> filters)
        {
            var picker = GetFileOpenPicker(filters);
            var files = await picker.PickMultipleFilesAsync();
            return files.Select(File.FromStorageFile);
        }

        private static FileOpenPicker GetFileOpenPicker(IEnumerable<string> filters)
        {
            var picker = new FileOpenPicker();

            if (filters == null)
            {
                picker.FileTypeFilter.Add("*");
            }
            else
            {
                foreach (var filter in filters)
                {
                    picker.FileTypeFilter.Add(filter);
                }
            }

            return picker;
        }
    }
}
