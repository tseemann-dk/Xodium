using Sidekick.Models;
using System;
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
                    NextFolderNumber = 2,
                    NextShortcutNumber = 3
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
                    new Shortcut(DateTime.Today, "Shortcut 1", 1, 10),
                    new Shortcut(DateTime.Today, "Shortcut 2", 1, 20),
                })
            );
        }
    }
}
