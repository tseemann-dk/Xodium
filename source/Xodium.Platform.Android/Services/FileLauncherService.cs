using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Support.V4.Content;
using Android.Webkit;
using Android.Widget;
using Xodium.Services;

namespace Xodium.Platform.Android.Services
{
    public class FileLauncherService : FileLauncherServiceBase
    {
        private readonly Context context;

        public FileLauncherService(Context context, IFileSystemService fileSystemService)
            : base(fileSystemService)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public override async Task<bool> LaunchFile(string path)
        {
            var source = await FileSystemService.GetFile(path);
            var folder = await FileSystemService.GetTempFolder();
            var target = await folder.CreateFile(Path.GetFileName(path), FileCreateMode.ReplaceExisting);

            using (var input = await source.Open(FileOpenMode.Read))
            using (var output = await target.Open(FileOpenMode.Write))
            {
                await input.CopyToAsync(output);
            }

            return await InternalLaunchFile(target.Path);
        }

        protected override Task<bool> InternalLaunchFile(string path)
        {
            try
            {
                /* NB!
                
                Requires a "FileProvider" registration in the <application> section of AndroidManifest.xml, i.e.:

                <application> 
                    <provider
                        android:name="android.support.v4.content.FileProvider"
                        android:authorities="${applicationid}.fileprovider"
                        android:exported="false"
                        android:grantUriPermissions="true">
                        <meta-data
                            android:name="android.support.FILE_PROVIDER_PATHS"
                            android:resource="@xml/file_provider_paths"/>
                    </provider>                
                </application>

                ... and an associated paths declaration file Resources/xml/file_provider_paths.xml, i.e:

                <?xml version="1.0" encoding="utf-8" ?>
                <paths xmlns:android="http://schemas.android.com/apk/res/android">
                    <cache-path name="cache_files" path="."/>
                </paths>

                */

                var mimeType = MimeTypeMap.Singleton.GetMimeTypeFromExtension(MimeTypeMap.GetFileExtensionFromUrl(path));
                var intent = new Intent(Intent.ActionView);
                var file = new Java.IO.File(path);

                var uri = FileProvider.GetUriForFile(context.ApplicationContext, context.ApplicationInfo.PackageName + ".fileprovider", file);

                intent.SetDataAndType(uri, mimeType);
                intent.SetFlags(ActivityFlags.NoHistory);
                intent.SetFlags(ActivityFlags.GrantReadUriPermission);

                //var chooser = Intent.CreateChooser(intent, "Open");
                context.StartActivity(intent);

                return Task.FromResult(true);
            }
            catch (Exception exception)
            {
                Toast.MakeText(context, $"Error opening {path}: {exception.Message}", ToastLength.Long).Show();
                return Task.FromResult(false);
            }
        }
    }
}
