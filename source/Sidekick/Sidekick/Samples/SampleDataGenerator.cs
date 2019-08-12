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
            var elements = new[]
            {
                new Element("1", "Element 1", 10),
                new Element("2", "Element 2", 20)
            };

            return new Archive("archive-1", "A1",
                new Folder("folder-1", "F1", "Folder 1", 1, new[]
                {
                    new Shortcut(elements[0], 1),
                    new Shortcut(elements[1], 1)
                }),
                elements
            );
        }
    }
}
