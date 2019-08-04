using Sidekick.Models;
using System;
using System.Linq;

namespace Sidekick.Samples
{
    public static class SampleDataGenerator
    {
        public static AppState BuildSampleAppState()
        {
            var project = BuildSampleDocument();
            var folder = project.Content;

            return new AppState
            {
                Global = new GlobalState
                {
                    NextLineNumber = 3
                },
                CurrentDocument = new ProjectState
                {
                    Document = project,
                    CurrentFolderId = folder.Id,
                    SelectedNodeId = folder.Nodes.Last().Id
                },
            };
        }

        private static Project BuildSampleDocument()
        {
            return new Project("doc-1", "D1",
                new Folder("folder-1", "F1", "Folder 1", 1, new[]
                {
                    new Line(DateTime.Today, "Line 1", 1, 10),
                    new Line(DateTime.Today, "Line 2", 1, 20),
                })
            );
        }
    }
}
