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
                    NextExpenseNumber = 3
                },
                CurrentDocument = new DocumentState
                {
                    Document = project,
                    CurrentFolderId = folder.Id,
                    SelectedNodeId = folder.Nodes.Last().Id
                },
            };
        }

        private static ExpenseDocument BuildSampleDocument()
        {
            return new ExpenseDocument("doc-1", "D1",
                new Folder("folder-1", "Folder 1", 1, new[]
                {
                    new Line(DateTime.Today, "Expense 1", 1, 10),
                    new Line(DateTime.Today, "Expense 2", 1, 20),
                })
            );
        }
    }
}
