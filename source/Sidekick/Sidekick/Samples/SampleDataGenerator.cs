using Sidekick.Models;
using System.Linq;

namespace Sidekick.Samples
{
    public static class SampleDataGenerator
    {
        public static AppState BuildSampleAppState()
        {
            var archive = BuildSampleArchive();
            var folder = archive.Content;

            return new AppState
            {
                Global = new GlobalState
                {
                    NextElementNumber = 3,
                    NextFolderNumber = 2
                },
                CurrentArchive = new ArchiveState
                {
                    Document = archive,
                    CurrentFolderId = folder.Id,
                    FocusedNodeId = folder.Nodes.Last().Id
                },
            };
        }

        private static Archive BuildSampleArchive()
        {
            return new Archive("archive-1", "A1",
                new Folder("folder-1", "F1", "Folder 1", 1, new[]
                {
                    new Shortcut(new Element("1", "Shortcut 1", 10), 1),
                    new Shortcut(new Element("2", "Shortcut 2", 20), 1)
                })
            );
        }
    }
}
