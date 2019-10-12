using System;
using System.Collections.Generic;
using Sidekick.Shopper.Actions.ShoppingSession;
using Sidekick.Shopper.Models;
using Sidekick.Shopper.State;
using Xodium.DataStructures;
using Xodium.Flow;

namespace Sidekick.Shopper.Reducers
{
    public static class ShoppingSessionReducer
    {
        private static readonly Dictionary<Type, Reducer<ShoppingSession>> handlers = new Dictionary<Type, Reducer<ShoppingSession>>
        {
            [typeof(EnterFolderAction)] = (s, a) => EnterFolder(s, (EnterFolderAction)a),
            [typeof(ExitFolderAction)] = (s, _) => ExitFolder(s),
            [typeof(FocusNodeAction)] = (s, a) => FocusNode(s, (FocusNodeAction)a),
            [typeof(Actions.ShoppingList.AddFolderAction)] = (s, a) => AddFolder(s, (Actions.ShoppingList.AddFolderAction)a),
            [typeof(Actions.ShoppingList.AddItemAction)] = (s, a) => AddItem(s, (Actions.ShoppingList.AddItemAction)a),
            [typeof(Actions.ShoppingList.DeleteNodeAction)] = (s, a) => DeleteNode(s, (Actions.ShoppingList.DeleteNodeAction)a)
        };

        public static ShoppingSession Reduce(ShoppingSession state, object action)
        {
            if (handlers.TryGetValue(action.GetType(), out var handler))
            {
                state = handler(state, action);
            }

            return state
                .WithComponentLookup(ComponentLookupReducer.Reduce(state.ComponentLookup, action))
                .WithShoppingList(ShoppingListReducer.Reduce(state.ShoppingList, action));;
        }

        private static ShoppingSession AddFolder(ShoppingSession state, Actions.ShoppingList.AddFolderAction action) =>
            state.WithFocusedNodeId(action.Payload.Folder.Id);

        private static ShoppingSession AddItem(ShoppingSession state, Actions.ShoppingList.AddItemAction action) =>
            state.WithFocusedNodeId(action.Payload.Item.Id);

        private static ShoppingSession DeleteNode(ShoppingSession state, Actions.ShoppingList.DeleteNodeAction action)
        {
            var folder = state.ShoppingList.FindFolder(action.Payload.FolderId);
            var node = folder.GetChildNode(action.Payload.NodeId) as IShoppingNode;
            var neighborId = (folder.GetNextSibling(node) ?? folder.GetPreviousSibling(node))?.Id;

            return state.WithFocusedNodeId(neighborId);
        }

        private static ShoppingSession EnterFolder(ShoppingSession state, EnterFolderAction action)
        {
            var folderId = action.Payload.FolderId;
            var currentFolder = state.GetCurrentFolder();
            var node = currentFolder.GetChildNode(action.Payload.FolderId);

            if (node == null)
                throw new KeyNotFoundException($"Node {folderId} was not found in folder {currentFolder.Id}");

            if (!(node is IShoppingFolder))
                throw new InvalidCastException($"Node {folderId} is not a folder");

            return state.WithCurrentFolderId(folderId);
        }

        private static ShoppingSession ExitFolder(ShoppingSession state)
        {
            var currentFolder = state.GetCurrentFolder();
            var parentFolderId = currentFolder?.GetParent(state.ShoppingList.Content)?.Id;

            if (parentFolderId == null) 
                return state;

            return state
                .WithCurrentFolderId(parentFolderId)
                .WithFocusedNodeId(state.CurrentFolderId);
        }

        private static ShoppingSession FocusNode(ShoppingSession state, FocusNodeAction action) =>
            state.WithFocusedNodeId(action.Payload.NodeId);
    }
}
