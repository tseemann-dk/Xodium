using System;
using System.Collections.Generic;
using Sidekick.Shopper.Actions.ShoppingList;
using Sidekick.Shopper.State;
using Xodium.Flow;

namespace Sidekick.Shopper.Reducers
{
    public static class ShoppingListReducer
    {
        private static readonly Dictionary<Type, Reducer<ShoppingList>> handlers = new Dictionary<Type, Reducer<ShoppingList>>
        {
            [typeof(AddComponentAction)] = (s, a) => AddComponent(s, (AddComponentAction)a),
            [typeof(AddFolderAction)] = (s, a) => AddFolder(s, (AddFolderAction)a),
            [typeof(AddItemAction)] = (s, a) => AddItem(s, (AddItemAction)a),
            [typeof(ChangeFolderTitleAction)] = (s, a) => ChangeFolderTitle(s, (ChangeFolderTitleAction)a),
            [typeof(DeleteNodeAction)] = (s, a) => DeleteNode(s, (DeleteNodeAction)a),
            [typeof(MoveNodeDownAction)] = (s, a) => MoveNodeDown(s, (MoveNodeDownAction)a),
            [typeof(MoveNodeUpAction)] = (s, a) => MoveNodeUp(s, (MoveNodeUpAction)a),
        };

        public static ShoppingList Reduce(ShoppingList state, object action)
        {
            if (handlers.TryGetValue(action.GetType(), out var handler))
            {
                return handler(state, action); 
            }

            return state;
        }

        private static ShoppingList AddComponent(ShoppingList state, AddComponentAction action) =>
            state.AddComponent(action.Payload.Component);

        private static ShoppingList AddFolder(ShoppingList state, AddFolderAction action) =>
            state.AddNode(
                state.FindFolder(action.Payload.ParentFolderId), 
                action.Payload.Folder, 
                action.Payload.InsertAfterNodeId
            );

        private static ShoppingList AddItem(ShoppingList state, AddItemAction action) =>
            state.AddNode(
                state.FindFolder(action.Payload.FolderId), 
                action.Payload.Item, 
                action.Payload.InsertAfterNodeId
            );

        private static ShoppingList ChangeFolderTitle(ShoppingList state, ChangeFolderTitleAction action) => 
            state.ChangeFolderTitle(
                state.FindFolder(action.Payload.FolderId), 
                action.Payload.NewTitle
            );

        private static ShoppingList DeleteNode(ShoppingList state, DeleteNodeAction action) =>
            state.DeleteNode(
                state.FindFolder(action.Payload.FolderId), 
                action.Payload.NodeId
            );

        private static ShoppingList MoveNodeDown(ShoppingList state, MoveNodeDownAction action) =>
            state.MoveNodeDown(
                state.FindFolder(action.Payload.FolderId), 
                action.Payload.NodeId
            );

        private static ShoppingList MoveNodeUp(ShoppingList state, MoveNodeUpAction action) =>
            state.MoveNodeUp(
                state.FindFolder(action.Payload.FolderId), 
                action.Payload.NodeId
            );
    }
}
