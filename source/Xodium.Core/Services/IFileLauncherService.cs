using System.IO;
using System.Threading.Tasks;

namespace Xodium.Services
{
    public interface IFileLauncherService
    {
        /// <summary>
        /// Launches the specified file using the default application
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <returns>true if success, false if cancelled by user</returns>
        Task<bool> LaunchFile(string path);

        /// <summary>
        /// Creates a temporary file containing the passed stream and launches it using the default application
        /// </summary>
        /// <param name="stream">The contents of the file</param>
        /// <param name="name">The suggested name of the file, representing the file type via its extension</param>
        /// <returns>The path to the temporary file if success, null if cancelled by user</returns>
        Task<string> LaunchFile(Stream stream, string name);
    }
}
