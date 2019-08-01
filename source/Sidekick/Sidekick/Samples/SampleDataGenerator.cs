using Sidekick.Models;
using System.Linq;

namespace Sidekick.Samples
{
    public static class SampleDataGenerator
    {
        public static AppState BuildSampleAppState()
        {
            var project = BuildSampleProject();
            var folder = project.Content;

            return new AppState
            {
                Global = new GlobalState
                {
                    NextElementNumber = 1
                },
                CurrentProject = new ProjectState
                {
                    Project = project,
                    CurrentFolderId = folder.Id,
                    SelectedNodeId = folder.Nodes.Last().Id
                },
            };
        }

        private static Project BuildSampleProject()
        {
            return new Project("project-1", "P1",
                new Folder("folder-1", "F1", "Folder 1", 1, new[]
                {
                    new Line(new Element("e1", "9000001", "Element 1"), 10),
                    new Line(new Element("e2", "9000002", "Element 2"), 5),
                })
            );
        }
    }
}
